using System;

namespace DrivenAz.Public
{
   [AttributeUsage(AttributeTargets.Class)]
   public class TableStorageAttribute : Attribute
   {
      private readonly string _tableName;

      public TableStorageAttribute(string tableName)
      {
         _tableName = tableName;
      }

      public string TableName
      {
         get { return _tableName; }
      }
   }
}
