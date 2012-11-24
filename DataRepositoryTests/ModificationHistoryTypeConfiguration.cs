using System.Data.Entity.ModelConfiguration;
using DataRepository.Common;

namespace DataRepositoryTests
{
    public class ModificationHistoryTypeConfiguration : EntityTypeConfiguration<ModificationHistory>
    {
        #region Ctor

        public ModificationHistoryTypeConfiguration()
        {
            HasKey(history => new { history.Id, history.Uid });

            Property(history => history.EntityName)
                .IsRequired()
                .HasMaxLength(250);

            Property(history => history.Uid)
                .IsRequired();

            Property(history => history.DateLastModified)
                .IsRequired();

            Property(history => history.ModifiedBy).
                IsRequired().
                HasMaxLength(100);
        }

        #endregion
    }
}
