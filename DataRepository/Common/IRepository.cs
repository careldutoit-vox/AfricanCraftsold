using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Common {
    public interface IRepository<T> where T : IEntity
    {
        T Save(T entity);
        List<T> List();
        T GetGyId(int entityId);
        T GetGyUid(Guid entityUid);
        void Delete(T entity);
        void Delete(int entityId);
        void Delete(Guid entityUid);
    }
}
