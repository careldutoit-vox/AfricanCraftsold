using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Common
{
    public class GenericFactory<T> : IGenericFactory<T> where T : IEntity
    {
        readonly T _entity;
        private readonly IRepository<T> _repository;

        public GenericFactory(T entity, IRepository<T> repository)
        {
            _repository = repository;
            _entity = (entity.IsNew()) ? repository.Save(entity) :_repository.GetGyId(entity.Id);
        }

        public IGenericFactory<T> AddPropertyValue(Expression<Func<T, object>> property, object value)
        {
            PropertyInfo propertyInfo = null;
            if (property.Body is MemberExpression)
            {
                var memberExpression = property.Body as MemberExpression;
                if (memberExpression != null)
                    propertyInfo = memberExpression.Member as PropertyInfo;
            }
            else
            {
                var memberExpression = ((UnaryExpression) property.Body).Operand as MemberExpression;
                if (memberExpression != null)
                    propertyInfo = memberExpression.Member as PropertyInfo;
            }

            if (propertyInfo != null) 
                propertyInfo.SetValue(_entity, value, null);

            return this;
        }

        public T Save()
        {
            return _repository.Save(_entity);

        }

        public List<T> List()
        {
            return _repository.List();
        }

        public T GetGyId(int entityId)
        {
            return _repository.GetGyId(_entity.Id);
        }

        public T GetGyUid(Guid entityUid)
        {
            return _repository.GetGyUid(_entity.Uid);
        }

        public void Delete(T entity)
        {
            _repository.Delete(_entity);
        }

        public void Delete(int entityId)
        {
            _repository.Delete(_entity.Id);
        }

        public void Delete(Guid entityUid)
        {
            _repository.Delete(_entity.Uid);
        }
    }
}
