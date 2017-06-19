using System.Collections.Concurrent;

namespace Republic.Magic.Core.Networking
{
    internal class Pool<T>
    {
        internal readonly object _sync = new object();
        internal readonly ConcurrentQueue<T> _stack;

        internal Pool()
        {
            this._stack = new ConcurrentQueue<T>();
        }

        internal T Pop()
        {
            var ret = default(T);
            if (this._stack.Count > 0)
                this._stack.TryDequeue(out ret);

            return ret;
        }

        internal void Push(T item)
        {
            this._stack.Enqueue(item);
        }
    }
}
