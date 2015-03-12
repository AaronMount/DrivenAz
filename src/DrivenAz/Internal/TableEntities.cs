using System.Collections.Generic;
using System.Linq;
using DrivenAz.Public;
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

      public static IEnumerable<TableBatchInformation<T>> ToBatchInformations<T>(this IEnumerable<IEnumerable<T>> batches, EntitiesBatchInformationConverter<T> converter)
         where T : ITableEntity
      {
         return converter(batches);
      }

      public static IEnumerable<TableBatchInformation<T>> ToBatchInformations<T>(this IEnumerable<IEnumerable<EntityKey>> batches, EntityKeyBatchInformationConverter<T> converter)
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
