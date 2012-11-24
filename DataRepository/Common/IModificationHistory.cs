using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Common
{
    public interface IModificationHistory
    {
        string EntityName { get; set; }
        Guid Uid { get; set; }
        DateTime DateLastModified { get; set; }
        string ModifiedBy { get; set; }
    }
}
