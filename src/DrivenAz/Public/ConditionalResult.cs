using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DrivenAz.Public
{
   public struct ConditionalResult<T> : IEnumerable<T>
   {
      private readonly T _value;
      private readonly bool _hasValue;

      public ConditionalResult(T value)
      {
         _value = value;
         _hasValue = !Equals(value, null);
      }

      public bool HasValue
      {
         get { return _hasValue; }
      }

      public T Value
      {
         get
         {
            if (Equals(_value, null))
            {
               throw new NoNullAllowedException();
            }

            return _value;
         }
      }

      public IEnumerator<T> GetEnumerator()
      {
         return _hasValue
            ? (new List<T>() {_value}).GetEnumerator()
            : (new List<T>()).GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}
