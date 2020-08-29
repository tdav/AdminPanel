using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Database.Core
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        TContext DbContext { get; }

        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;

        int SaveChanges(bool ensureAutoHistory = false);

        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);

        int ExecuteSqlCommand(string sql, params object[] parameters);
    }
}
