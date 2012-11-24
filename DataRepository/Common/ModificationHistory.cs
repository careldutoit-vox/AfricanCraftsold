using System;

namespace DataRepository.Common
{
    public class ModificationHistory : IModificationHistory
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public Guid Uid { get; set; }
        public DateTime DateLastModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}