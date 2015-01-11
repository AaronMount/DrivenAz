using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Public
{
   public interface IAsyncTableAccessor
   {
      Task<bool> CreateTableIfNotExistsAsync<T>()
         where T : class, ITableEntity;

      Task<bool> DeleteTableIfExistsAsync<T>()
         where T : class, ITableEntity;

      Task<T> InsertAsync<T>(T entity)
         where T : class, ITableEntity;

      Task<EnumerableResult<T>> InsertAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity;

      Task<T> MergeAsync<T>(T entity)
         where T : class, ITableEntity;

      Task<EnumerableResult<T>> MergeAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity;

      Task<T> InsertOrMergeAsync<T>(T entity)
         where T : class, ITableEntity;

      Task<EnumerableResult<T>> InsertOrMergeAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity;

      Task<T> ReplaceAsync<T>(T entity)
         where T : class, ITableEntity;

      Task<EnumerableResult<T>> ReplaceAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity;

      Task<T> InsertOrReplaceAsync<T>(T entity)
         where T : class, ITableEntity;

      Task<EnumerableResult<T>> InsertOrReplaceAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity;

      Task<ConditionalResult<T>> RetrieveAsync<T>(string partitionKey, string rowKey)
         where T : class, ITableEntity;

      Task<ConditionalResult<T>> RetrieveAsync<T>(EntityKey key)
         where T : class, ITableEntity;

      Task<EnumerableResult<T>> RetrieveAllAsync<T>(IEnumerable<EntityKey> keys)
         where T : class, ITableEntity;

      Task DeleteAsync<T>(T entity)
         where T : class, ITableEntity;

      Task DeleteAllAsync<T>(IEnumerable<T> entities)
         where T : class, ITableEntity;

      Task DeleteAsync<T>(string partitionKey, string rowKey)
         where T : class, ITableEntity;
   }
}