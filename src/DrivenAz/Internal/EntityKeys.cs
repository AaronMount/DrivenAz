using System.Collections.Generic;
using System.Linq;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal static class EntityKeys
   {
      public static IEnumerable<IEnumerable<EntityKey>> ToBatches(this IEnumerable<EntityKey> items)
      {
         var result = items
            .GroupBy(i => i.PartitionKey)
            .SelectMany(g => g.Split(100));

         return result;
      }

      public static IEnumerable<TableBatchOperation> ToOperations(this IEnumerable<IEnumerable<EntityKey>> batches, KeysOperationConverter converter)
      {
         return converter(batches);
      }
   }
}
