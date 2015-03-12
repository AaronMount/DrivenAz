using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal static class TableBatchOperations
   {
      public async static Task<IEnumerable<T>> ExecuteAsync<T>(this IEnumerable<TableBatchInformation<T>> batchInformations, CloudTable table) where T : ITableEntity
      {
         var executions = new List<TableResult>();

         foreach (var batchInformation in batchInformations)
         {
            try
            {
               var execution = await table.ExecuteBatchAsync(batchInformation.Operation);
               executions.AddRange(execution);
            }
            catch (StorageException e)
            {
               var failedIndex = Int32.Parse(e.Message.Substring(e.Message.LastIndexOf(" ", StringComparison.InvariantCulture) + 1));
               throw new DrivenAzStorageException<T>(e, batchInformation.Entities[failedIndex]);
            }
         }

         var results = executions.Select(e => (T) e.Result)
            .ToList();

         return results;
      }
   }
}
