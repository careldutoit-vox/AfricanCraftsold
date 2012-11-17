using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Common
{
    public class GenericRepository<T> : IRepository<T> where T : IEntity
    {
        readonly List<T> _context = new List<T>();
        
        public T Save(T entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _context.Count() + 1;
                entity.Uid = entity.Uid == new Guid() ? Guid.NewGuid() : entity.Uid;
                entity.DateCreated = DateTime.Now;
                // ToDo: Gonna need to see how to integrate this into OAuth
                entity.CreatedBy = "Devlin"; //UserPrincipal.Current.DisplayName;
                _context.Add(entity);
            }
            else
            {
                var index = FindIndexById(entity.Id);
                if (index < 0)
                {
                    _context.Add(entity);
                }
                else
                {
                    _context[index] = entity;
                }
            }

            return entity;
        }

        public List<T> List()
        {
            return _context.ToList();
        }

        public T GetGyId(int entityId)
        {
            return _context[FindIndexById(entityId)];
        }

        public T GetGyUid(Guid entityUid)
        {
            return _context[FindIndexByUid(entityUid)];
        }

        public void Delete(T entity)
        {
            _context.RemoveAt(FindIndexById(entity.Id));
        }

        public void Delete(int entityId)
        {
            _context.RemoveAt(FindIndexById(entityId));
        }

        public void Delete(Guid entityUid)
        {
            _context.RemoveAt(FindIndexByUid(entityUid));
        }

        private int FindIndexById(int entityId)
        {
            return _context.FindIndex(p => p.Id == entityId);
        }

        private int FindIndexByUid(Guid entityUid)
        {
            return _context.FindIndex(p => p.Uid == entityUid);
        }
    }
}
