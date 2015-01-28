using System.Collections.Generic;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal delegate TableOperation EntityOperationConverter(ITableEntity entity);
   internal delegate IEnumerable<TableBatchOperation> EntitiesOperationConverter<in T>(IEnumerable<IEnumerable<T>> batches);
   internal delegate TableOperation KeyOperationConverter(EntityKey key);
   internal delegate IEnumerable<TableBatchOperation> KeysOperationConverter(IEnumerable<IEnumerable<EntityKey>> keys);

   internal static class OperationConverter
   {
      public static TableOperation Retrieve<T>(EntityKey key)
         where T : ITableEntity
      {
         var result = TableOperation.Retrieve<T>(key.PartitionKey, key.RowKey);

         return result;
      }
      
      public static IEnumerable<TableBatchOperation> InsertOrMergeAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches)
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.InsertOrMerge(instance);
            }

            yield return operation;
         }
      }

      public static IEnumerable<TableBatchOperation> InsertOrReplace<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches)
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.InsertOrReplace(instance);
            }

            yield return operation;
         }
      }

      public static IEnumerable<TableBatchOperation> RetrieveAll<T>(this IEnumerable<IEnumerable<EntityKey>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches)
         {            
            foreach (var instance in batch)
            {
               /* NOTE: only one retrieve allowed for each record.  it throws otherwise.
                */
               var operation = new TableBatchOperation();

               operation.Retrieve<T>(instance.PartitionKey, instance.RowKey);

               yield return operation;
            }
         }
      }

      public static IEnumerable<TableBatchOperation> DeleteAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches)
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Delete(instance);
            }

            yield return operation;
         }
      }

      public static IEnumerable<TableBatchOperation> InsertAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches)
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Insert(instance);
            }

            yield return operation;
         }
      }

      public static IEnumerable<TableBatchOperation> MergeAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches)
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Merge(instance);
            }

            yield return operation;
         }
      }

      public static IEnumerable<TableBatchOperation> ReplaceAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches)
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Replace(instance);
            }

            yield return operation;
         }
      }
   }
}