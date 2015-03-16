using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{   
   internal class AsyncTableAccessor : IAsyncTableAccessor
   {
      private readonly CloudTableClient _client;
      private readonly EventHub _eventHub;

      public AsyncTableAccessor(CloudStorageAccount account, EventHub eventHub)
      {
         _eventHub = eventHub;
         _eventHub.OperationCompleted += OperationCompleted;
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

      public Task<EnumerableResult<T>> ExecuteQueryAsync<T>(TableQuery<T> query)
         where T : ITableEntity, new()
      {
         return Task.Factory.StartNew(() =>
            {
               var result = _client.GetTableReference<T>()
                  .ExecuteQuery(query);

               return result.ToResults();
            });
      }

      public async Task<T> InsertAsync<T>(T entity)
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.Insert);

         OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.Insert, new[] { entity }));

         return result;
      }

      public async Task<EnumerableResult<T>> InsertAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.InsertAll, _eventHub);

         return output.ToResults();
      }

      public async Task<T> MergeAsync<T>(T entity)
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.Merge);

         OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.Merge, new[] { entity }));

         return result;
      }

      public async Task<EnumerableResult<T>> MergeAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.MergeAll, _eventHub);

         return output.ToResults();
      }

      public async Task<T> InsertOrMergeAsync<T>(T entity)
         where T : class, ITableEntity
      {         
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.InsertOrMerge);

         OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.InsertOrMerge, new[] { entity }));

         return result;
      }

      public async Task<EnumerableResult<T>> InsertOrMergeAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.InsertOrMergeAll, _eventHub);

         return output.ToResults();
      }

      public async Task<T> ReplaceAsync<T>(T entity)
         where T : class, ITableEntity
      {
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.Replace);

         OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.Replace, new[] { entity }));

         return result;
      }

      public async Task<EnumerableResult<T>> ReplaceAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.ReplaceAll, _eventHub);

         return output.ToResults();
      }

      public async Task<T> InsertOrReplaceAsync<T>(T entity)
         where T : class, ITableEntity
      {         
         var result = await _client.GetTableReference<T>()
            .ExecuteAsync<T>(entity, TableOperation.InsertOrReplace);

         OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.InsertOrReplace, new[] { entity }));

         return result;
      }

      public async Task<EnumerableResult<T>> InsertOrReplaceAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         var output = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(entities, OperationConverter.InsertOrReplace, _eventHub);

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

         OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.Retrieve, new[] { entity }));

         return entity.ToResult();
      }

      public async Task<EnumerableResult<T>> RetrieveAllAsync<T>(IEnumerable<EntityKey> keys)
         where T : class, ITableEntity
      {
         var entities = await _client.GetTableReference<T>()
            .ExecuteBatchAsync<T>(keys, OperationConverter.RetrieveAll<T>, _eventHub);

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

            OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.Delete, new[] { entity }));
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

            OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.Delete, new [] { entity }));
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
         var entityArray = entities.ToArray();
         foreach (var entity in entityArray)
         {
            await DeleteAsync(entity);
         }

         OperationCompleted(this, new DrivenAzOperationCompletedArgs(TableOperationType.Delete, entityArray));

         //var output = await _client.GetTableReference<T>()
         //   .ExecuteBatchAsync<T>(entities, OperationConverter.DeleteAll);

         //return output.ToResults();
      }

      public event EventHandler<DrivenAzOperationCompletedArgs> OperationCompleted = delegate { };
   }
}
