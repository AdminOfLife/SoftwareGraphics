using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    internal sealed class ConcatCollection<T> : ICollection<T>
    {
        int count;
        IEnumerable<T> allElements;

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public ConcatCollection(IEnumerable<IEnumerable<T>> collections)
        {
            allElements = collections.SelectMany(values => values);
            count = allElements.Count();
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return allElements.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex < count)
                throw new ArgumentException("array.Length - arrayIndex must be >= count.", "array");
            if (arrayIndex < 0 || arrayIndex >= count)
                throw new ArgumentOutOfRangeException("arrayIndex");

            foreach (var item in allElements)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return allElements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
