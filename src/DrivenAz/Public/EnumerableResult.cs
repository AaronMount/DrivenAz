using System;
using System.Collections;
using System.Collections.Generic;

namespace DrivenAz.Public
{
   public struct EnumerableResult<T> : IReadOnlyList<T>
   {
      private readonly List<T> _items;

      public EnumerableResult(List<T> items)
      {
         if (items == null) throw new ArgumentNullException("items");

         _items = items;
      }

      public int Count
      {
         get { return _items.Count; }
      }

      public T this[int index]
      {
         get { return _items[index]; }
      }

      public IEnumerator<T> GetEnumerator()
      {
         return _items.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}
