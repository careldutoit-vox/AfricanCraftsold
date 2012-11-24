using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository;
using DataRepository.Common;

namespace DataRepositoryTests
{
    public class TestDbDatabaseInitializer : IDatabaseInitializer<TestDbContext>
    {
        private readonly bool createSampleData;

        public TestDbDatabaseInitializer(bool createSampleData = false)
        {
            this.createSampleData = createSampleData;
        }

        public void InitializeDatabase(TestDbContext context)
        {
            if (context.Database.Exists()) return;

            context.Database.Delete();
            context.Database.Create();

            // Add seed data
            Seed(context);
        }

        private void Seed(TestDbContext context)
        {
            if (createSampleData)
            {
                context.ApplyTo(
                    TestData.InsertTestItems
                    );
            }

            context.SaveChanges();
        }
    }
}
