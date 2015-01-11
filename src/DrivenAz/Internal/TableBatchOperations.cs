using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal static class TableBatchOperations
   {
      public async static Task<IEnumerable<T>> ExecuteAsync<T>(this IEnumerable<TableBatchOperation> operations, CloudTable table)
      {
         var executions = new List<TableResult>();

         foreach (var operation in operations)
         {            
            var execution = await table.ExecuteBatchAsync(operation);

            executions.AddRange(execution);
         }

         var results = executions.Select(e => (T) e.Result)
            .ToList();

         return results;
      }
   }
}
