using System.Collections.Generic;
using System.Net.Sockets;

namespace BL.Servers.CoC.Core.Networking
{
    internal class SocketAsyncEventArgsPool
    {
        internal readonly Stack<SocketAsyncEventArgs> Pool;

        internal readonly object Gate = new object();

        internal SocketAsyncEventArgsPool()
        {
            this.Pool = new Stack<SocketAsyncEventArgs>();
        }

        internal SocketAsyncEventArgs Dequeue()
        {
            lock (this.Gate)
            {
                if (this.Pool.Count > 0)
                {
                    return this.Pool.Pop();
                }

                return null;
            }
        }

        internal void Enqueue(SocketAsyncEventArgs Args)
        {
            lock (this.Gate)
            {
                // if (this.Pool.Count < Constants.MaxPlayers)
                {
                    this.Pool.Push(Args);
                }
            }
        }

        internal void Dispose()
        {
            lock (this.Gate)
            {
                this.Pool.Clear();
            }
        }
    }
}