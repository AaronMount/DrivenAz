using System.Collections.Generic;
using System.Threading.Tasks;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{   
   internal class AsyncTableAccessor : IAsyncTableAccessor
   {
      private readonly CloudTableClient _client;

      public AsyncTableAccessor(CloudStorageAccount account)
      {
         _client = account.CreateCloudTableClient();
      }

      public async Task<bool> CreateTableIfNotExistsAsync<T>()
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .CreateIfNotExistsAsync();

         return result;
      }

      public async Task<bool> DeleteTableIfExistsAsync<T>()
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .DeleteIfExistsAsync();

         return result;
      }

      public async Task<T> InsertAsync<T>(T entity)
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.Insert);

         return result;
      }

      public async Task<EnumerableResult<T>> InsertAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.InsertAll);

         return output.ToResults();
      }

      public async Task<T> MergeAsync<T>(T entity)
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.Merge);

         return result;
      }

      public async Task<EnumerableResult<T>> MergeAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.MergeAll);

         return output.ToResults();
      }

      public async Task<T> InsertOrMergeAsync<T>(T entity)
         where T : class, ITableEntity
      {         
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.InsertOrMerge);

         return result;
      }

      public async Task<EnumerableResult<T>> InsertOrMergeAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.InsertOrMergeAll);

         return output.ToResults();
      }

      public async Task<T> ReplaceAsync<T>(T entity)
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.Replace);

         return result;
      }

      public async Task<EnumerableResult<T>> ReplaceAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.ReplaceAll);

         return output.ToResults();
      }

      public async Task<T> InsertOrReplaceAsync<T>(T entity)
         where T : class, ITableEntity
      {         
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.InsertOrReplace);

         return result;
      }

      public async Task<EnumerableResult<T>> InsertOrReplaceAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.InsertOrReplace);

         return output.ToResults();
      }

      public async Task<ConditionalResult<T>> RetrieveAsync<T>(string partitionKey, string rowKey)
         where T : class, ITableEntity
      {
         var key = new EntityKey(partitionKey, rowKey);

         return await RetrieveAsync<T>(key);         
      }

      public async Task<ConditionalResult<T>> RetrieveAsync<T>(EntityKey key)
         where T : class, ITableEntity
      {         
         var entity = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(key, OperationConverter.Retrieve<T>);

         return entity.ToResult();
      }

      public async Task<EnumerableResult<T>> RetrieveAllAsync<T>(IEnumerable<EntityKey> keys)
         where T : class, ITableEntity
      {
         var entities = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(keys, OperationConverter.RetrieveAll<T>);

         return entities.ToResults();
      }

      public async Task DeleteAsync<T>(string partitionKey, string rowKey)
         where T : class, ITableEntity
      {         
         try
         {         
            var entity = new DynamicTableEntity(partitionKey, rowKey);

            await _client.GetTableReference<T>()
               .ExecuteAsync<T>(entity, TableOperation.Delete);
         }
         catch (StorageException e)
         {
            if (!e.Message.Contains("(404) Not Found"))
            {
               throw;
            }
         }
      }

      public async Task DeleteAsync<T>(T entity)
         where T : class, ITableEntity
      {
         try
         {
            await _client.GetTableReference<T>()
               .ExecuteAsync<T>(entity, TableOperation.Delete);
         }
         catch (StorageException e)
         {
            if (!e.Message.Contains("(404) Not Found"))
            {
               throw;
            }
         }
      }

      public async Task DeleteAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         /* NOTE: no way to quietly delete non-existant entities in batch mode.
          *       iterating and deleting instead until there's a way to do this.
          *       this is the one true bummer to this whole system.
          */
         foreach (var entity in entities)
         {
            await DeleteAsync(entity);
         }

         //var output = await _client.GetTableReference<T>()
         //   .ExecuteBatchAsync<T>(entities, OperationConverter.DeleteAll);

         //return output.ToResults();
      }
   }
}
