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
               int failedIndex = -1;
               var responseCodeIndex = e.Message.LastIndexOf(" ", StringComparison.InvariantCulture);
               if (responseCodeIndex > -1)
               {
                  if (Int32.TryParse(e.Message.Substring(responseCodeIndex + 1), out failedIndex))
                  {
                     throw new DrivenAzStorageException<T>(e, batchInformation.Entities[failedIndex]);
                  }
               }
               throw;
            }
         }

         var results = executions.Select(e => (T) e.Result)
            .ToList();

         return results;
      }
   }
}
