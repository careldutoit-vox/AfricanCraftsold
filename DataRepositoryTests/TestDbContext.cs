using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.Common;
using DataRepositoryTests.Models;

namespace DataRepositoryTests
{
    public class TestDbContext : DbContext, IDbContext
    {
        public TestDbContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public IDbSet<Item> Items { get; set; }
        public IDbSet<ModificationHistory> ModificationHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ItemTypeConfiguration());
            modelBuilder.Configurations.Add(new ModificationHistoryTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
