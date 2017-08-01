using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Magic.ClashOfClans.Network
{
    internal static class Gateway
    {
        private static Socket s_listener;
        private static Pool<SocketAsyncEventArgs> s_argsPool;
        private static Pool<byte[]> s_bufferPool;

        private static Semaphore s_semaphore;
        private static int s_buffersCreated;
        private static int s_argsCreated;

        public static int NumberOfBuffers => s_bufferPool.Count;
        public static int NumberOfBuffersCreated => s_buffersCreated;
        public static int NumberOfBuffersInUse => s_buffersCreated - s_bufferPool.Count;

        public static int NumberOfArgs => s_argsPool.Count;
        public static int NumberOfArgsCreated => s_argsCreated;
        public static int NumberOfArgsInUse => s_argsCreated - s_argsPool.Count;

        public static void Initialize()
        {
            var numThreads = Environment.ProcessorCount * 2;
            s_semaphore = new Semaphore(numThreads, numThreads);

            s_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s_argsPool = new Pool<SocketAsyncEventArgs>();

            const int PRE_ALLOC_SAEA = 128;
            for (int i = 0; i < PRE_ALLOC_SAEA; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                s_argsPool.Push(args);

                s_argsCreated++;
            }

            s_bufferPool = new Pool<byte[]>();
        }

        public static void Listen()
        {
            const int PORT = 9339;
            const int BACK_LOG = 100;

            var endPoint = new IPEndPoint(IPAddress.Any, PORT);

            s_listener.Bind(endPoint);
            s_listener.Listen(BACK_LOG);

            var args = GetArgs();
            StartAccept(args);

            Logger.Say($"Listening on {endPoint}...");
        }

        public static void Send(this Message message)
        {
            var buffer = default(byte[]);
            var client = message.Client;

            try { message.Encode(); }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while encoding message {message.GetType()}");
                return;
            }

            try { buffer = message.GetRawData(); }
            catch (Exception ex)
            {
                // Exit early since buffer will be null, we can't send a null buffer to the client.
                ExceptionLogger.Log(ex, $"Exception while constructing message {message.GetType()}");
                return;
            }

            var socket = client.Socket;

            var args = GetArgs();
            args.UserToken = client;
            args.SetBuffer(buffer, 0, buffer.Length);

            try { message.Process(client.Level); }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while processing outgoing message {message.GetType()}");
            }

            StartSend(args);
        }

        private static void StartSend(SocketAsyncEventArgs e)
        {
            var client = (Client)e.UserToken;
            var socket = client.Socket;

            try
            {
                while (!socket.SendAsync(e))
                    ProcessSend(e);
            }
            catch (ObjectDisposedException)
            {
                Drop(e);
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while starting receive");
            }
        }

        private static void ProcessSend(SocketAsyncEventArgs e)
        {
            var client = (Client)e.UserToken;
            var transferred = e.BytesTransferred;

            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                Drop(e);
            }
            else
            {
                try
                {
                    var offset = e.Offset;
                    var count = e.Count;
                    if (transferred < count)
                    {
                        // Move the offset so it points to the next piece of data to send.
                        e.SetBuffer(offset + transferred, count - transferred);
                        StartSend(e);
                    }
                    else
                    {
                        // We done with sending can recycle EventArgs.
                        Recycle(e);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while processing send");
                }
            }
        }

        private static void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                // Avoid StackOverflowExceptions cause we can.
                while (!s_listener.AcceptAsync(e))
                    ProcessAccept(e, false);
            }
            catch (Exception ex)
            {
                // If this is reached, we won't start listening again, therefore no more clients.
                ExceptionLogger.Log(ex, "Exception while starting to accept(critical)");
                // Could try to resurrect listeners or something here.
            }
        }

        private static void ProcessAccept(SocketAsyncEventArgs e, bool startNew)
        {
            var acceptSocket = e.AcceptSocket;
            if (e.SocketError != SocketError.Success)
            {
                Logger.Error($"Failed to accept new socket: {e.SocketError}.");
                Drop(e);

                // Get a new args from pool, since we dropped the previous one.
                e = GetArgs();
            }
            else
            {
                try
                {
                    if (Constants.Verbosity > 3)
                        Logger.Say($"Accepted connection at {acceptSocket.RemoteEndPoint}.");

                    var client = new Client(acceptSocket);
                    // Register the client in the ResourceManager.
                    ResourcesManager.AddClient(client);

                    var args = GetArgs();
                    var buffer = GetBuffer();
                    args.UserToken = client;
                    args.SetBuffer(buffer, 0, buffer.Length);

                    StartReceive(args);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while processing accept");
                }
            }

            // Clean up for reuse.
            e.AcceptSocket = null;
            if (startNew)
                StartAccept(e);
        }

        private static void StartReceive(SocketAsyncEventArgs e)
        {
            var client = (Client)e.UserToken;
            var socket = client.Socket;

            try
            {
                while (!socket.ReceiveAsync(e))
                    ProcessReceive(e, false);
            }
            catch (ObjectDisposedException)
            {
                Drop(e);
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while start receive");
            }
        }

        private static void ProcessReceive(SocketAsyncEventArgs e, bool startNew)
        {
            var client = (Client)e.UserToken;
            var transferred = e.BytesTransferred;

            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                Drop(e);
            }
            else
            {
                try
                {
                    var buffer = e.Buffer;
                    var offset = e.Offset; // 0 anyways.
                    for (int i = 0; i < transferred; i++)
                        client.DataStream.Add(buffer[offset + i]);

                    var level = client.Level;
                    var message = default(Message);
                    while (client.TryGetPacket(out message))
                    {
                        try { message.Process(level); }
                        catch (Exception ex)
                        {
                            ExceptionLogger.Log(ex, $"Exception while processing incoming message {message.GetType()}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while processing receive");
                }

                if (startNew)
                    StartReceive(e);
            }
        }

        private static void AsyncOperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            const int TIME_OUT = 10000;

            var semaphoreAcquired = false;
            try
            {
                semaphoreAcquired = s_semaphore.WaitOne(TIME_OUT);
                if (semaphoreAcquired)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        switch (e.LastOperation)
                        {
                            case SocketAsyncOperation.Accept:
                                ProcessAccept(e, true);
                                break;

                            case SocketAsyncOperation.Receive:
                                ProcessReceive(e, true);
                                break;

                            case SocketAsyncOperation.Send:
                                ProcessSend(e);
                                break;

                            default:
                                throw new Exception("Unexpected operation.");
                        }
                    }
                    else
                    {
                        if (Constants.Verbosity > 1)
                            Logger.SayInfo($"A socket operation wasn't successful => {e.LastOperation}. Dropping connection.");

                        Drop(e);

                        // If the last operation was an accept operation, continue accepting
                        // for new connections.
                        if (e.LastOperation == SocketAsyncOperation.Accept)
                        {
                            var args = GetArgs();
                            StartAccept(args);
                        }
                    }
                }
                else
                {
                    Logger.Error("SEMAPHORE DID NOT RESPOND IN TIME!");
                    Drop(e);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception occurred while processing async operation(potentially critical). Dropping connection");
                Drop(e);
            }
            finally
            {
                if (semaphoreAcquired)
                    s_semaphore.Release();
            }
        }

        private static void Drop(SocketAsyncEventArgs e)
        {
            if (e == null)
                return;

            var client = e.UserToken as Client;
            if (client != null)
            {
                // If the resource manager did not mange to kill the socket, we do it here.
                if (!ResourcesManager.DropClient(client.GetSocketHandle()))
                    KillSocket(client.Socket);
            }
            else if (e.LastOperation == SocketAsyncOperation.Accept)
            {
                KillSocket(e.AcceptSocket);
            }

            // Recycle the object along with its buffer.
            Recycle(e);
        }

        private static void Recycle(SocketAsyncEventArgs e)
        {
            if (e == null)
                return;

            var buffer = e.Buffer;
            e.UserToken = null;
            e.AcceptSocket = null;
            e.SetBuffer(null, 0, 0);

            Recycle(buffer);

            s_argsPool.Push(e);
        }

        private static void Recycle(byte[] buffer)
        {
            if (buffer == null)
                return;

            if (buffer.Length == Constants.BufferSize)
                s_bufferPool.Push(buffer);
        }

        private static void KillSocket(Socket socket)
        {
            if (socket == null)
                return;

            try { socket.Shutdown(SocketShutdown.Both); }
            catch { /* Swallow */ }
            try { socket.Dispose(); }
            catch { /* SWallow */ }
        }

        private static SocketAsyncEventArgs GetArgs()
        {
            var args = s_argsPool.Pop();
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;

                Interlocked.Increment(ref s_argsCreated);
            }
            return args;
        }

        private static byte[] GetBuffer()
        {
            var buffer = s_bufferPool.Pop();
            if (buffer == null)
            {
                buffer = new byte[Constants.BufferSize];

                Interlocked.Increment(ref s_buffersCreated);
            }
            return buffer;
        }
    }
}
