namespace DataRepository.Common
{
    public class GenericFluentFactory<T> where T : IEntity
    {
        public static IGenericFactory<T> Init(T entity, IRepository<T> repository)
        {
            return new GenericFactory<T>(entity, repository);
        }
    }
}
