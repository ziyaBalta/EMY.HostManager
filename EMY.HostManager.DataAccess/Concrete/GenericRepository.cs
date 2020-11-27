using EMY.HostManager.DataAccess.Abstract;
using EMY.HostManager.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EMY.HostManager.DataAccess.Concrete
{
    public class GenericRepository<T> : IAsyncRepository<T> where T : class
    {
        private DbContext _context = null;
        private DbSet<T> table = null;


        public GenericRepository(DbContext context)
        {
            this._context = context;
            table = _context.Set<T>();
        }

        public async Task<int> SaveChanges()
        {
            var res = await _context.SaveChangesAsync();
            return res;
        }

        public async Task<T> GetByPrimaryKey(object id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await table.FirstOrDefaultAsync<T>(predicate);
        }

        async Task IAsyncRepository<T>.Add(T entity, int UserRef)
        {
            if (entity is BaseEntity b)
            {
                b.CreatorID = b.LastUpdaterID = UserRef;
                b.CreatedDate = b.LastUpdateDate = DateTime.Now;
                b.IsDeleted = false;
            }

            await table.AddAsync(entity);
            await SaveChanges();
        }

        public async Task Update(T entity, int userRef)
        {
            if (entity is BaseEntity b)
            {
                b.LastUpdaterID = userRef;
                b.LastUpdateDate = DateTime.Now;
            }
            table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
        }

        public async Task Remove(int entityID, int userRef)
        {
            T existing = await table.FindAsync(entityID);
            if (existing is BaseEntity b)
            {
                b.DeleterID = userRef;
                b.IsDeleted = true;
                b.DeletedDate = DateTime.Now;

            }
            await SaveChanges();
        }

        public async Task HardRemove(int entityID)
        {
#if DEBUG
            T existing = await table.FindAsync(entityID);
            table.Remove(existing);
            await SaveChanges();
#endif
#if RELEASE
            throw new MethodAccessException("You can not hard remove in RELEASE mode!");
#endif
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var res = await table.ToListAsync();
            return res;
        }

        public async Task<IEnumerable<T>> GetAllWithNoTrack()
        {
            var res = await table.AsNoTracking().ToListAsync();
            return res;
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            var res = await table.Where(predicate).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<T>> GetWhereWithNoTrack(Expression<Func<T, bool>> predicate)
        {
            var res = await table.AsNoTracking().Where(predicate).ToListAsync();
            return res;
        }

        public async Task<int> CountAll()
        {
            var res = await table.AsNoTracking().CountAsync();
            return res;
        }

        public async Task<int> CountWhere(Expression<Func<T, bool>> predicate)
        {
            var res = await table.AsNoTracking().CountAsync(predicate);
            return res;
        }
    }
}
