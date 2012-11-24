using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DataRepository.Common;
using DataRepository.Repositories;
using DataRepositoryTests.Models;

namespace DataRepositoryTests
{
    [TestClass]
    public class InMemoryRepositoryTests
    {
        readonly IRepository<Item> _repository = new InMemoryRepository<Item>();

        [TestMethod]
        public void InMemoryRepository_SaveOrUpdate_Inserts_New_Item()
        {
            var expectedItem = AddExpectedItem();
            expectedItem = _repository.SaveOrUpdate(expectedItem);

            var result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);

            AssertResult(expectedItem, result);
        }

        [TestMethod]
        public void InMemoryRepository_SaveOrUpdate_Updates_Existing_Item()
        {
            var expectedItem = AddExpectedItem();
            expectedItem = _repository.SaveOrUpdate(expectedItem);
            expectedItem.Name = "Updated";
            expectedItem = _repository.SaveOrUpdate(expectedItem);

            var result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);

            AssertResult(expectedItem, result);
            Assert.AreEqual("Updated", result.Name);
        }

        [TestMethod]
        public void InMemoryRepository_SaveOrUpdate_Returns_ListCount_One_Greater_After_Saving_One_Item()
        {
            var expectedCount = _repository.GetAll().Count() + 1;
            AddItems(1);
            Assert.AreEqual(expectedCount, _repository.GetAll().Count());
        }

        [TestMethod]
        public void InMemoryRepository_SaveOrUpdate_Returns_ListCount_Two_Greater_After_Saving_Two_Items()
        {
            var expectedCount = _repository.GetAll().Count() + 2;
            AddItems(2);

            Assert.AreEqual(expectedCount, _repository.GetAll().Count());
        }

        [TestMethod]
        public void InMemoryRepository_GetById_Returns_Item_By_Id()
        {
            AddItems(2);

            var expectedItem = _repository.GetById(2);
            AssertResult(expectedItem, _repository.GetById(2));
        }

        [TestMethod]
        public void InMemoryRepository_GetById_Returns_New_Item_When_Not_Found()
        {
            AddItems(2);

            var expectedItem = new Item();
            var result = _repository.GetById(3);
            Assert.IsNotNull(result);

            Assert.AreEqual(expectedItem.Id, result.Id);
            Assert.AreEqual(expectedItem.Uid, result.Uid);
            Assert.AreEqual(expectedItem.Name, result.Name);
            Assert.AreEqual(expectedItem.Description, result.Description);
            Assert.AreEqual(expectedItem.UserName, result.UserName);
        }

        [TestMethod]
        public void InMemoryRepository_GetByUid_Returns_Item_By_Uid()
        {
            AddItems(2);
            var expectedItem = _repository.GetById(2);

            AssertResult(expectedItem, _repository.GetByUid(expectedItem.Uid));
        }

        [TestMethod]
        public void InMemoryRepository_Delete_Item_Deletes_Item()
        {
            var expectedItem = AddExpectedItem();
            var result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);
            AssertResult(expectedItem, result);

            _repository.DeleteOnSubmit(expectedItem);
            result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void InMemoryRepository_Delete_By_Id_Deletes_Item()
        {
            var expectedItem = AddExpectedItem();
            var result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);
            AssertResult(expectedItem, result);

            _repository.Delete(result.Id);
            result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void InMemoryRepository_Delete_By_Uid_Deletes_Item()
        {
            var expectedItem = AddExpectedItem();
            var result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);
            AssertResult(expectedItem, result);

            _repository.Delete(result.Uid);
            result = _repository.GetAll().FirstOrDefault(item => item == expectedItem);

            Assert.IsNull(result);
        }

        private Item AddExpectedItem()
        {
            var expectedItem = new Item()
                                   {
                                       Name = string.Format("Name_{0}", "Test"),
                                       Description = string.Format("Description_{0}", "Test"),
                                       UserName = string.Format("UserName_{0}", "Test")
                                   };

            expectedItem = _repository.SaveOrUpdate(expectedItem);
            return expectedItem;
        }
        
        private void AddItems(int numberOfItemsToAdd)
        {
            for (var i = 1; i < numberOfItemsToAdd + 1; i++)
            {
                var product = new Item()
                                  {
                                      Name = string.Format("Name_{0}", i),
                                      Description = string.Format("Description_{0}", i),
                                      UserName = string.Format("UserName_{0}", i)
                                  };

                _repository.SaveOrUpdate(product);

                //GenericFluentFactory<Item>
                //    .Init(product, _repository)
                //    .SaveOrUpdate();
            }
        }

        private static void AssertResult(Item expected, Item result)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Uid, result.Uid);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.UserName, result.UserName);
        }

        private void AddProduct(string productName, string productDescription, string productUserName)
        {
            //GenericFluentFactory<Product>
            //    .Init(new Product(), _repository)
            //    .AddPropertyValue(p => p.Name, productName)
            //    .AddPropertyValue(p => p.Description, productDescription)
            //    .AddPropertyValue(p => p.UserName, productUserName)
            //    .SaveOrUpdate();

            // ToDo: Mmmm, this breaks because of lack of type checking
            //GenericFluentFactory<Product>
            //    .Init(new Product(), _repository)
            //    .AddPropertyValue(p => p.DateCreated, productName)
            //    .AddPropertyValue(p => p.Name, productName)
            //    .AddPropertyValue(p => p.Description, productDescription)
            //    .AddPropertyValue(p => p.UserName, productUserName)
            //    .SaveOrUpdate();

            // Better...
            //GenericFluentFactory<Item>
            //    .Init(new Item()
            //              {
            //                  Name = productName,
            //                  Description = productDescription,
            //                  UserName = productUserName
            //              }, _repository)
            //    .SaveOrUpdate();

            // Best - Do not use stupid fluent factory...
        }
    }
}
