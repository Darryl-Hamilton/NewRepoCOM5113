
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderAssessment
{
    internal class Queue<T>
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();

        public void Enqueue(T item)
        {
            _list.PushBack(item);
        }

        public T Dequeue()
        {
            T item;
            _list.PopFront(out item);
            return item;
        }

        public bool IsEmpty()
        {
            return _list.IsEmpty();
        }
    }
}
