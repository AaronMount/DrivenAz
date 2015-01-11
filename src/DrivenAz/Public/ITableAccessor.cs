using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Public
{
   public interface ITableAccessor
   {
      bool CreateTableIfNotExists<T>() where T : class, ITableEntity;
      bool DeleteTableIfExists<T>() where T : class, ITableEntity;
      T Insert<T>(T entity) where T : class, ITableEntity;
      EnumerableResult<T> InsertAll<T>(IEnumerable<T> entities) where T : class, ITableEntity;
      T Merge<T>(T entity) where T : class, ITableEntity;
      EnumerableResult<T> MergeAll<T>(IEnumerable<T> entities) where T : class, ITableEntity;
      T InsertOrMerge<T>(T entity) where T : class, ITableEntity;
      EnumerableResult<T> InsertOrMergeAll<T>(IEnumerable<T> entities) where T : class, ITableEntity;
      T Replace<T>(T entity) where T : class, ITableEntity;
      EnumerableResult<T> ReplaceAll<T>(IEnumerable<T> entities) where T : class, ITableEntity;
      T InsertOrReplace<T>(T entity) where T : class, ITableEntity;
      EnumerableResult<T> InsertOrReplaceAll<T>(IEnumerable<T> entities) where T : class, ITableEntity;
      ConditionalResult<T> Retrieve<T>(string partitionKey, string rowKey) where T : class, ITableEntity;
      ConditionalResult<T> Retrieve<T>(EntityKey key) where T : class, ITableEntity;
      EnumerableResult<T> RetrieveAll<T>(IEnumerable<EntityKey> keys) where T : class, ITableEntity;
      void Delete<T>(T entity) where T : class, ITableEntity;
      void DeleteAll<T>(IEnumerable<T> entities) where T : class, ITableEntity;
      void Delete<T>(string partitionKey, string rowKey) where T : class, ITableEntity;
   }
}