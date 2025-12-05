
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderAssessment
{
    internal class Stack<T>
    {
        

        private readonly LinkedList<T> _list = new LinkedList<T>();
        
        public void Push(T data)
        {
            _list.PushFront(data);
        }

        public T Pop()
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
