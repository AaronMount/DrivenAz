using System;
using System.Collections.Generic;
using System.Linq;

namespace DrivenAz.Internal
{
   internal static class Enumerables
   {
      public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> items, int size)
      {
         for (var i = 0; i < Math.Ceiling(items.Count() / (Double) size); i++)
         {
            /* TODO: hate the skip take.  i just copied this from somehwere
             */
            yield return new List<T>(items.Skip(size * i).Take(size));
         }
      }
   }
}
