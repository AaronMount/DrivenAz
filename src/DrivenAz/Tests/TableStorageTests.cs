using System.Linq;
using DrivenAz.Public;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;

namespace DrivenAz.Tests
{
   [TestClass]
   public class TableStorageTests
   {         
      [TestMethod]
      public void TableAccessor_CanRetrieveAnExistingEntityByKeys()
      {
         var accessor = CreateCustomerTable();
         var actual = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");

         Assert.IsTrue(actual.HasValue);
         Assert.AreEqual("222-222-2222", actual.Value.PhoneNumber);
      }

      [TestMethod]
      public void TableAccessor_CanDeleteAnExistingEntity()
      {
         var accessor = CreateCustomerTable();
         var existing = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");
         
         accessor.Delete(existing.Value);

         var actual = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");

         Assert.IsFalse(actual.HasValue);         
      }

      [TestMethod]
      public void TableAccessor_CanDeleteAnExistingEntityByKey()
      {
         var accessor = CreateCustomerTable();

         accessor.Delete<CustomerEntity>("Dalton", "Nick");

         var actual = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");

         Assert.IsFalse(actual.HasValue);
      }

      [TestMethod]
      public void TableAccessor_CanInsertANewEntity()
      {
         var accessor = CreateCustomerTable();
         var entity = new CustomerEntity("Leatherwood", "Anthony") {PhoneNumber = "444-444-4444"};
         
         accessor.Insert(entity);

         var actual = accessor.Retrieve<CustomerEntity>("Leatherwood", "Anthony");

         Assert.IsTrue(actual.HasValue);
         Assert.AreEqual("444-444-4444", actual.Value.PhoneNumber);
      }

      [TestMethod]
      public void TableAccessor_CanBatchRetrieveEntitiesByKeys()
      {
         var accessor = CreateCustomerTable();
         var keys = new[]
            {
               new EntityKey("Mount", "Aaron"),
               new EntityKey("Dalton", "Nick"),
               new EntityKey("Brown", "Paul"),
            };

         var actual = accessor.RetrieveAll<CustomerEntity>(keys);
         
         Assert.AreEqual(3, actual.Count);
         Assert.AreEqual("333-333-3333", actual.Last().PhoneNumber);
      }

      [TestMethod]
      public void TableAccessor_CanBatchInsertNewEntities()
      {
         var accessor = CreateCustomerTable();
         var entities = new[]
            {
               new CustomerEntity("Leatherwood", "Anthony") {PhoneNumber = "444-444-4444"},
               new CustomerEntity("Peppa", "Pig") {PhoneNumber = "555-555-5555"},
               new CustomerEntity("Cash", "Johnny") {PhoneNumber = "666-666-6666"},
            };

         accessor.InsertAll(entities);

         var actual = accessor.Retrieve<CustomerEntity>("Peppa", "Pig");

         Assert.IsTrue(actual.HasValue);
         Assert.AreEqual("555-555-5555", actual.Value.PhoneNumber);
      }

      [TestMethod]
      public void TableAccessor_CanBatchDeleteEntities()
      {
         var accessor = CreateCustomerTable();
         var keys = new[]
            {
               new EntityKey("Mount", "Aaron"),
               new EntityKey("Dalton", "Nick"),
               new EntityKey("Brown", "Paul"),
            };

         var existing = accessor.RetrieveAll<CustomerEntity>(keys);

         accessor.DeleteAll(existing);

         var actual = accessor.RetrieveAll<CustomerEntity>(keys);

         Assert.AreEqual(0, actual.Count);         
      }

      [TestMethod]
      public void TableAccessor_ConflictingMergesSucceedWithoutError()
      {
         var accessor = CreateCustomerTable();
         var version1 = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");
         var version2 = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");

         version1.Value.PhoneNumber = "000-000-0000";
         version2.Value.PhoneNumber = "999-999-9999";
         
         accessor.Merge(version1.Value);
         accessor.Merge(version2.Value);

         var actual = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");

         Assert.IsTrue(actual.HasValue);
         Assert.AreEqual("999-999-9999", actual.Value.PhoneNumber);
      }

      [TestMethod]      
      public void TableAccessor_ConflictingDeletesSucceedWithoutError()
      {
         var accessor = CreateCustomerTable();
         var version1 = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");
         var version2 = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");

         accessor.Delete(version1.Value);
         accessor.Delete(version2.Value);

         Assert.IsTrue(true);
      }

      [TestMethod]
      public void TableAccessor_ConflictingBatchDeletesSucceedWithoutError()
      {
         var accessor = CreateCustomerTable();
         var version1 = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");
         var version2 = accessor.Retrieve<CustomerEntity>("Dalton", "Nick");

         accessor.DeleteAll(new[] {version1.Value});
         accessor.DeleteAll(new[] {version2.Value});

         Assert.IsTrue(true);
      }

      private static ITableAccessor CreateCustomerTable()
      {
         var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
         var accessor = StorageFactory.CreateAccessor(account);

         accessor.DeleteTableIfExists<CustomerEntity>();
         accessor.CreateTableIfNotExists<CustomerEntity>();
         PopulateCustomers(accessor);

         return accessor;
      }

      private static void PopulateCustomers(ITableAccessor accessor)
      {         
         var customers = new []
            {
               new CustomerEntity("Mount", "Aaron") {Email = "amount@unlimitedsystems.com", PhoneNumber = "111-111-1111"},
               new CustomerEntity("Dalton", "Nick") {Email = "ndalton@unlimitedsystems.com", PhoneNumber = "222-222-2222"},
               new CustomerEntity("Brown", "Paul") {Email = "pbrown@unlimitedsystems.com", PhoneNumber = "333-333-3333"},
            };

         accessor.InsertOrMergeAll(customers);
      }
   }
}
