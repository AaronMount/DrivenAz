using System;
using System.Collections.Concurrent;
using System.Linq;
using DrivenAz.Public;

namespace DrivenAz.Internal
{
   internal static class Tables
   {
      private static readonly ConcurrentDictionary<Type, string> _tableNames = new ConcurrentDictionary<Type, string>();

      public static string GetName<T>()         
      {
         var type = typeof(T);
         var result = _tableNames.GetOrAdd(type, ExtractName);

         return result;
      }

      private static string ExtractName(Type type)
      {
         var attribute = type.GetCustomAttributes(typeof (DrivenTableAttribute), true)
            .Cast<DrivenTableAttribute>()
            .SingleOrDefault();

         return attribute != null
            ? attribute.TableName
            : type.Name;
      }
   }
}