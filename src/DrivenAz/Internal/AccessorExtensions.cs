using System.Collections.Generic;
using System.Linq;
using DrivenAz.Public;

namespace DrivenAz.Internal
{
   internal static class AccessorExtensions
   {
      public static ConditionalResult<T> ToResult<T>(this T instance)
      {
         return new ConditionalResult<T>(instance);
      }

      public static EnumerableResult<T> ToResults<T>(this IEnumerable<T> instances)
      {
         return new EnumerableResult<T>(instances.ToList());
      }
   }
}
