#define CONCURRENT_STACK
//#define LIST // Use LIST to enable tracing n stuff.

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CRepublic.Magic.Core.Network
{
    internal class Pool<T>
    {
#if CONCURRENT_STACK
        private readonly ConcurrentQueue<T> _stack;
#elif LIST
        private readonly List<T> _list;
#endif


        internal Pool()
        {
#if CONCURRENT_STACK
            _stack = new ConcurrentQueue<T>();
#elif LIST
            _list = new List<T>();
#endif
        }

#if CONCURRENT_STACK
        internal int Count => _stack.Count;
#elif LIST
        internal int Count
        {
            get
            {
                lock (_list)
                    return _list.Count;
            }
        }
#endif

        internal T Pop()
        {
#if CONCURRENT_STACK
            var ret = default(T);
            if (!_stack.TryDequeue(out ret))
                return default(T);
            return ret;
#elif LIST
            lock (_list)
            {
                if (_list.Count > 0)
                {
                    var item = _list[0];
                    _list.RemoveAt(0);

                    return item;
                }
                return default(T);
            }
#endif
        }

        internal void Push(T item)
        {
#if CONCURRENT_STACK
            _stack.Enqueue(item);
#elif LIST
            lock (_list)
            {
                if (_list.Contains(item))
                {
                    //if (Constants.Verbosity > 2)
                        //Logger.Error("We're pushing an item to the pool, but its already there! Ignoring it.");
                }
                else
                {
                    _list.Add(item);
                }
            }
#endif
        }
    }
}
