using AdminPanel.Database.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdminPanel.Database.Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly MyDbContext db;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(MyDbContext context)
        {
            this.db = context;
            dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> GetAsync(object id)
        {
            var ety = db.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.First().ClrType;
            var rid = id.ConvertObject(ety);
            return await dbSet.FindAsync(rid);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            await dbSet.LoadAsync();
            return dbSet.AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync( string fieldName, string value)
        {
           return await dbSet.AsTracking()
                             .Where(e => EF.Functions.Like(EF.Property<string>(e, fieldName), value))
                             .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var ls = dbSet.AsNoTracking().Where(predicate);
            return await ls.ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity item)
        {
            var res = await dbSet.AddAsync(item);
            await db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task RemoveAsync(object id)
        {
            TEntity entity = await dbSet.FindAsync(id);
            if (entity == null) return;

            if (db.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public async Task UpdateAsync(TEntity item)
        {
            dbSet.Update(item);
            await db.SaveChangesAsync();
        }

        public async Task<IPagedList<TEntity>> ToPagedListAsync(Expression<Func<TEntity, bool>> predicate = null, int PageIndex = 1, int PageSize = 20 )
        {

            IQueryable<TEntity> query = dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
                    
            return await query.ToPagedListAsync(PageIndex, PageSize);
        }


    }
}
