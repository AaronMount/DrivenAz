using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal class TableBatchInformation<T> where T : ITableEntity
   {
      private readonly TableBatchOperation _operation;
      private readonly T[] _entities;

      public TableBatchOperation Operation { get { return _operation; } }
      public T[] Entities { get { return _entities; } }

      public TableBatchInformation(TableBatchOperation operation, IEnumerable<T> entities)
      {
         _operation = operation;
         _entities = entities.ToArray();
      }
   }
}
