using System;
using System.Collections.Generic;
using System.Linq;
using DataRepository.Common;

namespace DataRepository.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        readonly List<T> _context = new List<T>();

        public virtual T SaveOrUpdate(T entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _context.Count() + 1;
                entity.Uid = entity.Uid == new Guid() ? Guid.NewGuid() : entity.Uid;
                entity.DateCreated = DateTime.Now;
                // ToDo: Gonna need to see how to integrate this with OAuth
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

        public virtual IQueryable<T> GetAll()
        {
            return _context.AsQueryable();
        }

        public T GetById(int entityId)
        {
            var returnEntity = _context.FirstOrDefault(entity => entity.Id == entityId);

            return returnEntity ?? new T();
        }

        public T GetByUid(Guid entityUid)
        {
            return _context[FindIndexByUid(entityUid)];
        }

        public void DeleteOnSubmit(T entity)
        {
            _context.RemoveAt(FindIndexById(entity.Id));
        }

        public void Delete(int id)
        {
            _context.RemoveAt(FindIndexById(id));
        }

        public void Delete(Guid uid)
        {
            _context.RemoveAt(FindIndexByUid(uid));
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
