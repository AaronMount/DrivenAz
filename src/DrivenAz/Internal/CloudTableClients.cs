using Microsoft.WindowsAzure.Storage.Table;

namespace DrivenAz.Internal
{
   internal static class CloudTableClients
   {
      public static CloudTable GetTableReference<T>(this CloudTableClient client)
      {
         var tablename = Tables.GetName<T>();
         var table = client.GetTableReference(tablename);

         return table;
      }
   }
}
