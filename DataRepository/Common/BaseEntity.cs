using System;
using System.Collections.Generic;

namespace DataRepository.Common
{
    public class BaseEntity : IEntity
    {
        public bool IsNew()
        {
            return Id == 0;
        }

        public int Id { get; set; }
        public Guid Uid { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public virtual List<ModificationHistory> ModificationHistories { get; set; }
        
        //public void AddModificationHistory(ModificationHistory modificationHistory)
        //{
        //    if (ModificationHistories == null)
        //        ModificationHistories = new List<ModificationHistory>();

        //    ModificationHistories.Add(modificationHistory);
        //}
    }
}