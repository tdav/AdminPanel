using AdminPanel.Database.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdminPanel.Database.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(object id);

        Task<IEnumerable<TEntity>> GetAllAsync();
        
        Task<IEnumerable<TEntity>> GetAllAsync(string fieldName, string value);

        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity item);

        Task RemoveAsync(object id);

        Task UpdateAsync(TEntity item);

        Task<IPagedList<TEntity>> ToPagedListAsync(Expression<Func<TEntity, bool>> predicate = null, int PageIndex=1, int PageSize=20);
    }
}
