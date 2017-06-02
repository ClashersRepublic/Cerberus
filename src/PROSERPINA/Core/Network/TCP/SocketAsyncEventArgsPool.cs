using System.Collections.Generic;
using System.Net.Sockets;
using BL.Servers.CR.Core;
using System.Collections.Concurrent;

namespace BL.Servers.CR.Core.Network
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

                return null;
            }
        }

        internal void Enqueue(SocketAsyncEventArgs Args)
        {
             lock (this.Gate)
            {
                if (Args == null)
                {

                }

                this.Pool.Enqueue(Args);
            }
        }

        internal void Dispose()
        {
            lock (this.Gate)
            {
                while (this.Pool.TryDequeue(out SocketAsyncEventArgs Args));
            }
        }
    }
}