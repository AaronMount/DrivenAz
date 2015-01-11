using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal static class TableEntities
   {
      public static IEnumerable<IEnumerable<T>> ToBatches<T>(this IEnumerable<T> items)         
         where T : ITableEntity
      {
         var result = items
            .GroupBy(i => i.PartitionKey)
            .SelectMany(g => g.Split(100));

         return result;
      }

      public static IEnumerable<TableBatchOperation> ToOperations<T>(this IEnumerable<IEnumerable<T>> batches, EntitiesOperationConverter<T> converter)
         where T : ITableEntity
      {
         return converter(batches);
      }

      public static T ToPessimisticConcurrency<T>(this T entity)
         where T : ITableEntity
      {
         entity.ETag = "*";
         return entity;
      }

      public static IEnumerable<T> ToPessimisticConcurrency<T>(this IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         foreach (var entity in entities)
         {
            entity.ETag = "*";

            yield return entity;
         }
      }
   }
}
