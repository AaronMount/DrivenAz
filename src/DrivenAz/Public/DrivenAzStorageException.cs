using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Public
{
   public class DrivenAzStorageException<T> : Exception where T : ITableEntity
   {
      private readonly StorageException _storageException;
      private readonly T _entity;

      public StorageException StorageException { get { return _storageException; } }
      public T Entity { get { return _entity; } }
      
      public DrivenAzStorageException(StorageException storageException, T entity)
      {
         _storageException = storageException;
         _entity = entity;
      }
   }
}
