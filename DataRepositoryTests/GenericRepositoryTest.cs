using System;
using DataRepository.Common;
using DataRepository.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataRepositoryTests
{
    [TestClass]
    public class GenericRepositoryTest
    {
        readonly IRepository<Product> _repository = new GenericRepository<Product>();

        [TestMethod]
        public void Empty_GenericRepository_Returns_ListCount_One_After_Adding_Product()
        {
            AddProducts(1);
            Assert.AreEqual(1, _repository.List().Count);
        }

        [TestMethod]
        public void Empty_GenericRepository_Returns_ListCount_Two_After_Adding_2_Products()
        {
            AddProducts(2);

            Assert.AreEqual(2, _repository.List().Count);
        }

        private void AddProducts(int numberOfProductsToAdd)
        {
            for (int i = 0; i < numberOfProductsToAdd; i++)
            {
                AddProduct(string.Format("Name_{0}", i), string.Format("Description_{0}", i), string.Format("UserName_{0}", i));
            }
        }

        private void AddProduct(string productName, string productDescription, string productUserName)
        {
            GenericFluentFactory<Product>
                .Init(new Product(), _repository)
                .AddPropertyValue(p => p.Name,  productName)
                .AddPropertyValue(p => p.Description, productDescription)
                .AddPropertyValue(p => p.UserName, productUserName)
                .Save();
        }
    }
}
