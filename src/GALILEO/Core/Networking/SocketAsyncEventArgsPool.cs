using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using Republic.Magic.Extensions;
using SharpRaven.Data;

namespace Republic.Magic.Core.Networking
{
    internal class SocketAsyncEventArgsPool
    {
        internal readonly ConcurrentQueue<SocketAsyncEventArgs> Pool;

        internal readonly object Gate = new object();

        internal SocketAsyncEventArgsPool()
        {
            this.Pool = new ConcurrentQueue<SocketAsyncEventArgs>();
        }

        internal SocketAsyncEventArgs Dequeue()
        {
            lock (this.Gate)
            {
                if (this.Pool.TryDequeue(out SocketAsyncEventArgs args))
                {
                    return args;
                }
                /*(if (this.Pool.Count > 0)
                {
                    return this.Pool.Pop();
                }*/

                return null;
            }
        }

        internal void Enqueue(SocketAsyncEventArgs Args)
        {
            lock (this.Gate)
            {
                if (Args == null)
                {
                    Resources.Exceptions.RavenClient.Capture(  new SentryEvent("Items added to a SocketAsyncEventArgsPool is null"));
                    throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
                }
                this.Pool.Enqueue(Args);
                /*if (this.Pool.Count < Constants.MaxPlayers)
                {
                    this.Pool.Push(Args);
                }*/
            }
        }

        internal void Dispose()
        {
            lock (this.Gate)
            {
                while (this.Pool.TryDequeue(out SocketAsyncEventArgs Args))
                {
                    // do nothing
                }
                //this.Pool.Clear();
            }
        }
    }
}