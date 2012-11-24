using System;
using System.Data.Entity.Infrastructure;
using DataRepository.Common;
using DataRepository.Repositories;
using DataRepositoryTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;


namespace DataRepositoryTests
{
    /// <summary>
    /// Summary description for EntityRepositoryTests
    /// </summary>
    [TestClass]
    public class EntityRepositoryTests
    {
        private TestDbContext _context;
        private EntityRepository<Item> _itemRepository;

        private static Guid TestGuid
        {
            get { return new Guid("12345678-1234-1234-1234-123456789012"); }
        }

        private static string TestItemName
        {
            get { return "TestItem"; }
        }

        private static string TestItemDescription
        {
            get { return "Test: <strong>Html for the win</strong>"; }
        }

        private static DateTime TestItemDateCreated
        {
            get { return new DateTime(2012, 11, 18, 13, 05, 31, 643); }
        }

        private static string TestUserName
        {
            get { return "OAuthUser"; }
        }

        [TestInitialize()]
        public void SetUp()
        {
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            SetupContext();
            _context.Database.Delete();
            _context.Database.Create();

            var item = new Item
            {
                Uid = TestGuid,
                Name = TestItemName,
                Description = TestItemDescription,
                DateCreated = TestItemDateCreated,
                UserName = TestUserName,
            };

            SaveAndLoadEntity(item);

            _itemRepository = new EntityRepository<Item>(_context);  
        }

        private void AddTestItems(int numberOfItems)
        {
            for (int i = 2; i < numberOfItems + 2; i++)
            {
                
            }

            var item = new Item
                           {
                               Uid = TestGuid,
                               Name = TestItemName,
                               Description = TestItemDescription,
                               DateCreated = TestItemDateCreated,
                               UserName = TestUserName,
                           };

            SaveAndLoadEntity(item);
        }

        /// <summary>
        /// Persistance test helper
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        private T SaveAndLoadEntity<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            var id = entity.Id;

            _context.Dispose();
            SetupContext();

            var fromDb = _context.Set<T>().Find(id);
            Assert.IsNotNull(fromDb);
            return fromDb;
        }

        private void SetupContext()
        {
            _context = new TestDbContext("TestDbContext");
        }

        public EntityRepositoryTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Database_SaveChanges_Can_persists_Item()
        {
            var item = new Item
            {
                Uid = Guid.NewGuid(),
                Name = "SomeItem",
                Description = "<strong>Html for the win</strong>",
                UserName = "TestUser",
                DateCreated = DateTime.Now,
                CreatedBy = "TestUser"
            };

            SaveAndLoadEntity(item);
        }

        [TestMethod]
        public void EntityRepository_GetById_On_Existing_Item_retrieves_Item()
        {
            var expected = new Item
            {
                Id = 1,
                Uid = TestGuid,
                Name = TestItemName,
                Description = TestItemDescription,
                DateCreated = TestItemDateCreated,
                UserName = TestUserName,
            };

            var actual = _itemRepository.GetById(1);

            AssertResult(expected, actual);
        }

        [TestMethod]
        public void EntityRepository_SaveChanges_On_Existing_Item_saves_Item_Changes()
        {
            var expected = new Item
            {
                Id = 1,
                Uid = TestGuid,
                Name = string.Format("{0}-Updated", TestItemName),
                Description = TestItemDescription,
                DateCreated = TestItemDateCreated,
                UserName = TestUserName,
            };

            var updated = _itemRepository.GetById(1);
            updated.Name = expected.Name;
            _itemRepository.SaveOrUpdate(updated);

            var actual = _itemRepository.GetById(1);
            AssertResult(expected, actual);
        }

        [TestMethod]
        public void EntityRepository_SaveChanges_On_Existing_Item_saves_Item_ModificationHistory()
        {
            var expected = new Item
            {
                Id = 1,
                Uid = TestGuid,
                Name = string.Format("{0}-Updated", TestItemName),
                Description = TestItemDescription,
                DateCreated = TestItemDateCreated,
                UserName = TestUserName,
            };

            var updated = _itemRepository.GetById(1);
            updated.Name = expected.Name;

            _itemRepository.SaveOrUpdate(updated);
            var actual = _itemRepository.GetById(1);

            Assert.AreEqual(1, actual.ModificationHistories.Count);
        }

        [TestMethod]
        public void EntityRepository_SaveChanges_Twice_On_Existing_Item_saves_Item_ModificationHistory_Twice()
        {
            var expected = new Item
            {
                Id = 1,
                Uid = TestGuid,
                Name = string.Format("{0}-Updated", TestItemName),
                Description = TestItemDescription,
                DateCreated = TestItemDateCreated,
                UserName = TestUserName,
            };

            var actual = _itemRepository.GetById(1);
            actual.Name = expected.Name;
            actual = _itemRepository.SaveOrUpdate(actual);
            expected.Description = string.Format("{0}-Updated", TestItemDescription);
            actual = _itemRepository.SaveOrUpdate(actual);

            Assert.AreEqual(2, actual.ModificationHistories.Count);
        }

        private static void AssertResult(Item expected, Item actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Uid, actual.Uid);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.UserName, actual.UserName);
        }

       
    }
}
