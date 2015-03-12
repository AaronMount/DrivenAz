using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{   
   internal static class CloudTables
   {      
      public static async Task<T> ExecuteAsync<T>(this CloudTable table, ITableEntity entity, EntityOperationConverter converter)
         where T : class, ITableEntity
      {
         var operation = converter(entity.ToPessimisticConcurrency());
         var execution = await table.ExecuteAsync(operation);

         return execution.Result as T;
      }

      public static async Task<T> ExecuteAsync<T>(this CloudTable table, EntityKey key, KeyOperationConverter converter)
         where T : class, ITableEntity
      {
         var operation = converter(key);
         var execution = await table.ExecuteAsync(operation);

         return execution.Result as T;
      }

      public static async Task<IEnumerable<T>> ExecuteBatchAsync<T>(this CloudTable table, IEnumerable<T> entities, EntitiesBatchInformationConverter<T> converter)
         where T : class, ITableEntity
      {
         var executions = await entities
            .ToPessimisticConcurrency()
            .ToBatches()
            .ToBatchInformations(converter)
            .ExecuteAsync<T>(table);

         var results = executions
            .Where(e => e != null)
            .ToList();

         return results;
      }

      public static async Task<IEnumerable<T>> ExecuteBatchAsync<T>(this CloudTable table, IEnumerable<EntityKey> keys, EntityKeyBatchInformationConverter<T> batchInformationConverter)
         where T : ITableEntity
      {
         var executions = await keys            
            .ToBatches()
            .ToBatchInformations(batchInformationConverter)
            .ExecuteAsync<T>(table);

         var results = executions
            .Where(e => e != null)
            .ToList();

         return results;
      }
   }
}
