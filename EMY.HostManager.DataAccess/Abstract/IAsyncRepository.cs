using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EMY.HostManager.DataAccess.Abstract
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByPrimaryKey(object id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task Add(T entity, int userRef);
        Task Update(T entity, int userRef);
        Task Remove(int entityID, int userRef);
        Task HardRemove(int entityID);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWithNoTrack();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWhereWithNoTrack(Expression<Func<T, bool>> predicate);
        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);
    }
}
