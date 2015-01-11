using System.Collections.Generic;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal class TableAccessor : ITableAccessor
   {
      private readonly IAsyncTableAccessor _accessor;

      public TableAccessor(IAsyncTableAccessor accessor)
      {
         _accessor = accessor;
      }

      public bool CreateTableIfNotExists<T>() 
         where T : class, ITableEntity
      {
         return _accessor.CreateTableIfNotExistsAsync<T>().GetAwaiter().GetResult();
      }

      public bool DeleteTableIfExists<T>() 
         where T : class, ITableEntity
      {
         return _accessor.DeleteTableIfExistsAsync<T>().GetAwaiter().GetResult();
      }

      public T Insert<T>(T entity) 
         where T : class, ITableEntity
      {
         return _accessor.InsertAsync(entity).GetAwaiter().GetResult();
      }

      public EnumerableResult<T> InsertAll<T>(IEnumerable<T> entities) 
         where T : class, ITableEntity
      {
         return _accessor.InsertAllAsync(entities).GetAwaiter().GetResult();
      }

      public T Merge<T>(T entity) 
         where T : class, ITableEntity
      {
         return _accessor.MergeAsync(entity).GetAwaiter().GetResult();
      }

      public EnumerableResult<T> MergeAll<T>(IEnumerable<T> entities)
         where T : class, ITableEntity
      {
         return _accessor.MergeAllAsync(entities).GetAwaiter().GetResult();
      }

      public T InsertOrMerge<T>(T entity) 
         where T : class, ITableEntity
      {
         return _accessor.InsertOrMergeAsync(entity).GetAwaiter().GetResult();
      }

      public EnumerableResult<T> InsertOrMergeAll<T>(IEnumerable<T> entities) 
         where T : class, ITableEntity
      {
         return _accessor.InsertOrMergeAllAsync(entities).GetAwaiter().GetResult();
      }

      public T Replace<T>(T entity) 
         where T : class, ITableEntity
      {
         return _accessor.ReplaceAsync(entity).GetAwaiter().GetResult();
      }

      public EnumerableResult<T> ReplaceAll<T>(IEnumerable<T> entities) 
         where T : class, ITableEntity
      {
         return _accessor.ReplaceAllAsync(entities).GetAwaiter().GetResult();
      }

      public T InsertOrReplace<T>(T entity) 
         where T : class, ITableEntity
      {
         return _accessor.InsertOrReplaceAsync(entity).GetAwaiter().GetResult();
      }

      public EnumerableResult<T> InsertOrReplaceAll<T>(IEnumerable<T> entities) 
         where T : class, ITableEntity
      {
         return _accessor.InsertOrReplaceAllAsync(entities).GetAwaiter().GetResult();
      }

      public ConditionalResult<T> Retrieve<T>(string partitionKey, string rowKey) 
         where T : class, ITableEntity
      {
         return _accessor.RetrieveAsync<T>(partitionKey, rowKey).GetAwaiter().GetResult();
      }

      public ConditionalResult<T> Retrieve<T>(EntityKey key) 
         where T : class, ITableEntity
      {
         return _accessor.RetrieveAsync<T>(key).GetAwaiter().GetResult();
      }

      public EnumerableResult<T> RetrieveAll<T>(IEnumerable<EntityKey> keys) 
         where T : class, ITableEntity
      {
         return _accessor.RetrieveAllAsync<T>(keys).GetAwaiter().GetResult();
      }

      public void Delete<T>(string partitionKey, string rowKey)
         where T : class, ITableEntity
      {
         _accessor.DeleteAsync<T>(partitionKey, rowKey).GetAwaiter().GetResult();
      }

      public void Delete<T>(T entity) 
         where T : class, ITableEntity
      {
         _accessor.DeleteAsync(entity).GetAwaiter().GetResult();
      }

      public void DeleteAll<T>(IEnumerable<T> entities) 
         where T : class, ITableEntity
      {
         _accessor.DeleteAllAsync(entities).GetAwaiter().GetResult();
      }
   }
}
