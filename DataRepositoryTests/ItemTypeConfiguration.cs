using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DataRepository.Common;
using DataRepositoryTests.Models;

namespace DataRepositoryTests
{
    internal class ItemTypeConfiguration : EntityTypeConfiguration<Item>
    {
        #region Ctor

        public ItemTypeConfiguration()
        {
            HasKey(i => i.Id);

            Property(i => i.Name).
                IsRequired().
                HasMaxLength(100);

            Property(i => i.Description).
                IsRequired().
                HasMaxLength(4000);

            Property(i => i.UserName).
                IsRequired().
                HasMaxLength(100);

            HasOptional(i => i.ModificationHistories)
                .WithRequired()
                .Map(i => i.MapKey("Id,Uid"));
        }

        #endregion
    }
}