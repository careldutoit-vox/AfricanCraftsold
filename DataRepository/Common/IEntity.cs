using System;
using System.Collections.Generic;

namespace DataRepository.Common
{
    public interface IEntity
    {
        int Id { get; set; }
        Guid Uid { get; set; }
        DateTime DateCreated { get; set; }
        string CreatedBy { get; set; }
        List<IModificationHistory> ModificationHistories { get; set; }

        bool IsNew();
        void AddModificationHistory(IModificationHistory modificationHistory);
    }
}
