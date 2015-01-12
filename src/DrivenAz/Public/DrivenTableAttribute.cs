using System;

namespace DrivenAz.Public
{
   [AttributeUsage(AttributeTargets.Class)]
   public class DrivenTableAttribute : Attribute
   {
      private readonly string _tableName;

      public DrivenTableAttribute(string tableName)
      {
         _tableName = tableName;
      }

      public string TableName
      {
         get { return _tableName; }
      }
   }
}
