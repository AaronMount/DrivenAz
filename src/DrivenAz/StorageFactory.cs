using DrivenAz.Internal;
using DrivenAz.Public;
using Microsoft.WindowsAzure.Storage;

namespace DrivenAz
{
   public static class StorageFactory
   {
      public static IAsyncTableAccessor CreateAsyncAccessor(CloudStorageAccount account)
      {
         return new AsyncTableAccessor(account, new EventHub());
      }

      public static ITableAccessor CreateAccessor(CloudStorageAccount account)
      {
         var accessor = CreateAsyncAccessor(account);

         return new TableAccessor(accessor);
      }
   }
}
