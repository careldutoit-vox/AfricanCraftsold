using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.Common;

namespace DataRepository.Repositories
{
    public class EntityRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        protected readonly IDbContext Context;
        protected readonly IDbSet<T> Entities;

        public EntityRepository(IDbContext context)
        {
            Context = context;
            Entities = context.Set<T>();
        }

        public virtual T SaveOrUpdate(T entity)
        {
            if (Entities.Find(entity.Id) == null)
            {
                if (entity.DateCreated == DateTime.MinValue)
                    entity.DateCreated = DateTime.Now;

                if (string.IsNullOrWhiteSpace(entity.CreatedBy))
                    entity.CreatedBy = "OAuthUser";
                
                Entities.Add(entity);
                Context.SaveChanges();
            }

            Context.SaveChanges();

            if (entity.ModificationHistories == null)
                entity.ModificationHistories = new List<ModificationHistory>();

            entity.ModificationHistories.Add(new ModificationHistory
                                              {
                                                  EntityName = entity.GetType().Name,
                                                  Uid = entity.Uid,
                                                  DateLastModified = DateTime.Now,
                                                  ModifiedBy = "OAuthUser"
                                              });

            Context.SaveChanges();

            return Entities.Find(entity.Id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Entities;
        }

        public virtual T GetById(int id)
        {
            return Entities.Find(id);
        }

        public virtual T GetByUid(Guid uid)
        {
            return Entities.FirstOrDefault(entityToDelete => entityToDelete.Uid == uid);
        }

        public virtual void DeleteOnSubmit(T entity)
        {
            Entities.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;

            Entities.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(Guid uid)
        {
            var entity = GetByUid(uid);
            if (entity == null) return;

            Entities.Remove(entity);
            Context.SaveChanges();
        }
    }
}
