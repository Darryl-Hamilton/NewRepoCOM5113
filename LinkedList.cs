
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PathFinderAssessment
{
    
    public class LinkedList<T>
    {
        
        private class Element<U>
        {
            public U Data { get; }
            public Element<U>? Next { get; set; }

            public Element(U data)
            {
                Data = data;
                Next = null;
            }
        }

        
        private Element<T>? _head;
        
        // Constructor
        public LinkedList()
        {
            _head = null;
        }

        

        public bool IsEmpty()
        {
            return _head == null;
        }

        public void PushFront(T data)
        {
            
            Element<T> newElement = new Element<T>(data);
            newElement.Next = _head;
            _head = newElement;
        }

        public void PushBack(T data)
        {
            Element<T> newElement = new Element<T>(data);
            if (_head == null)
            {
                _head = newElement;
            }
            else
            {
                Element<T>? currentElement = _head;
                // Traverse to the end of the list
                while (currentElement.Next != null)
                {
                    currentElement = currentElement.Next;
                }
                // Link the new element at the end
                currentElement.Next = newElement;
            }
        }

        public void PopFront(out T data)
        {
            if (_head == null)
            {
                throw new InvalidOperationException("List empty");
            }
            data = _head.Data;
            _head = _head.Next;
        }

        public void PopBack (out T data)
        {
            if (_head == null)
            {
                throw new InvalidOperationException("List empty");
            }
            if (_head.Next == null)
            {
                // Only one element in the list
                data = _head.Data;
                _head = null;
            }
            else
            {
                Element<T>? currentElement = _head;
                // Traverse to the second last element
                while (currentElement.Next.Next != null)
                {
                    currentElement = currentElement.Next;
                }
                // currentElement is now the second last element
                data = currentElement.Next.Data;
                currentElement.Next = null; // Remove the last element
            }
        }


        
        public bool Contains(T data)
        {
            Element<T>? currentElement = _head;
            bool found = false;

            while (currentElement != null && !found) // stop when we have either reached the end, or found what we're looking for
            {
                if (EqualityComparer<T>.Default.Equals(currentElement.Data, data))
                {
                    found = true; // flag when we found the element
                }
                currentElement = currentElement.Next;
            }

            return found;
        }

       
    }
}
