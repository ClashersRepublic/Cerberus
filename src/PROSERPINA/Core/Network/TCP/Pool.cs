using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Core.Network.TCP
{
    internal class Pool<T>
    {
        internal readonly object Sync = new object();
        internal readonly ConcurrentQueue<T> Stack;

        internal Pool()
        {
            this.Stack = new ConcurrentQueue<T>();
        }

        internal T Pop()
        {
            var Ret = default(T);

            if (this.Stack.Count > 0)
                this.Stack.TryDequeue(out Ret);

            return Ret;
        }

        internal void Push (T item)
        {
            this.Stack.Enqueue(item);
        }
    }
}
