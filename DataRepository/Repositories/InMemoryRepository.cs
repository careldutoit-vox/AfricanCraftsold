using System;
using System.Collections.Generic;
using System.Linq;
using DataRepository.Common;

namespace DataRepository.Repositories
{
    public class InMemoryRepository<T> : IRepository<T>where T : IEntity, new()
    {
        readonly List<T> _context = new List<T>();
        
        public T Save(T entity)
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

        public List<T> List()
        {
            return _context;
        }

        public T GetGyId(int entityId)
        {
            var returnEntity = _context.FirstOrDefault(entity => entity.Id == entityId);

            return !(returnEntity == null) ? returnEntity : new T();
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
