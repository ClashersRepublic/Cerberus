using System.Collections.Concurrent;

namespace CRepublic.Magic.Core.Networking
{
    internal class Pool<T>
    {
        internal readonly ConcurrentBag<T> _stack;
        internal int Count => _stack.Count;


        internal Pool()
        {
            this._stack = new ConcurrentBag<T>();
        }

        internal T Pop()
        {
            var ret = default(T);
            return !_stack.TryTake(out ret) ? default(T) : ret;
        }

        internal void Push(T item)
        {
            this._stack.Add(item);
        }
    }
}
