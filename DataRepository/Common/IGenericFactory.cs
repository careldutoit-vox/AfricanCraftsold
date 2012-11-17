using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Common
{
    public interface IGenericFactory<T>
    {
        IGenericFactory<T> AddPropertyValue(Expression<Func<T, object>> property, object value);
        T Save();
        List<T> List();
        T GetGyId(int entityId);
        T GetGyUid(Guid entityUid);
        void Delete(T entity);
        void Delete(int entityId);
        void Delete(Guid entityUid);
    }
}
