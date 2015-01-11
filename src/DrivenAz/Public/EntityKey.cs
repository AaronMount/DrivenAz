namespace DrivenAz.Public
{
   public class EntityKey
   {
      public readonly string PartitionKey;
      public readonly string RowKey;

      public EntityKey(string partitionKey, string rowKey)
      {
         PartitionKey = partitionKey;
         RowKey = rowKey;
      }
   }
}