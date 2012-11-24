using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Common {
    public interface IRepository<T> where T : class, new()
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        T GetByUid(Guid uid);
        T SaveOrUpdate(T entity);
        void DeleteOnSubmit(T entity);
        void Delete(int id);
        void Delete(Guid uid);
    }
}
