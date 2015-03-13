using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivenAz.Internal;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Public
{
   public class DrivenAzOperationCompletedArgs
   {
      private readonly TableOperationType _operationType;
      private readonly ITableEntity[] _entities;

      public TableOperationType OperationType
      {
         get { return _operationType; }
      }

      public ITableEntity[] Entities
      {
         get { return _entities; }
      }

      internal DrivenAzOperationCompletedArgs(TableOperationType operationType, IEnumerable<ITableEntity> entities)
      {
         _operationType = operationType;
         _entities = entities.ToArray();
      }
   }
}
