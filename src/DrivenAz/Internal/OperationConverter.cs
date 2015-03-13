using System.Collections.Generic;
using System.Linq;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal delegate TableOperation EntityOperationConverter(ITableEntity entity);
   internal delegate IEnumerable<TableBatchInformation<T>> EntitiesBatchInformationConverter<T>(IEnumerable<IEnumerable<T>> batches) where T : ITableEntity;
   internal delegate IEnumerable<TableBatchInformation<T>> EntityKeyBatchInformationConverter<T>(IEnumerable<IEnumerable<EntityKey>> batches) where T : ITableEntity;
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
      
      public static IEnumerable<TableBatchInformation<T>> InsertOrMergeAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches.Select(b => b.ToArray()))
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.InsertOrMerge(instance);
            }

            yield return new TableBatchInformation<T>(operation, batch, TableOperationType.InsertOrMerge);
         }
      }

      public static IEnumerable<TableBatchInformation<T>> InsertOrReplace<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches.Select(b => b.ToArray()))
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.InsertOrReplace(instance);
            }

            yield return new TableBatchInformation<T>(operation, batch, TableOperationType.InsertOrReplace);
         }
      }

      public static IEnumerable<TableBatchInformation<T>> RetrieveAll<T>(this IEnumerable<IEnumerable<EntityKey>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches.Select(b => b.ToArray()))
         {            
            foreach (var instance in batch)
            {
               /* NOTE: only one retrieve allowed for each record.  it throws otherwise.
                */
               var operation = new TableBatchOperation();

               operation.Retrieve<T>(instance.PartitionKey, instance.RowKey);

               yield return new TableBatchInformation<T>(operation, new T[]{}, TableOperationType.Retrieve); ;
            }
         }
      }

      public static IEnumerable<TableBatchInformation<T>> DeleteAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches.Select(b => b.ToArray()))
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Delete(instance);
            }

            yield return new TableBatchInformation<T>(operation, batch, TableOperationType.Delete);
         }
      }

      public static IEnumerable<TableBatchInformation<T>> InsertAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches.Select(b => b.ToArray()))
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Insert(instance);
            }

            yield return new TableBatchInformation<T>(operation, batch, TableOperationType.Insert);
         }
      }

      public static IEnumerable<TableBatchInformation<T>> MergeAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches.Select(b => b.ToArray()))
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Merge(instance);
            }

            yield return new TableBatchInformation<T>(operation, batch, TableOperationType.Merge);
         }
      }

      public static IEnumerable<TableBatchInformation<T>> ReplaceAll<T>(this IEnumerable<IEnumerable<T>> batches)
         where T : ITableEntity
      {
         foreach (var batch in batches.Select(b => b.ToArray()))
         {
            var operation = new TableBatchOperation();

            foreach (var instance in batch)
            {
               operation.Replace(instance);
            }

            yield return new TableBatchInformation<T>(operation, batch, TableOperationType.Replace);
         }
      }
   }
}