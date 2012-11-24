using DataRepository.Common;
using DataRepositoryTests.Models;

namespace DataRepositoryTests
{
    public static class TestData
    {

        public static void InsertTestItems(TestDbContext context)
        {
            var item1 = new Item { Name = "Test Item 1", Description = "Test Item 1" };
            var item2 = new Item { Name = "Test Item 2", Description = "Test Item 2" };

            context.Set<Item>().Add(item1);
            context.Set<Item>().Add(item2);
        }
    }
}