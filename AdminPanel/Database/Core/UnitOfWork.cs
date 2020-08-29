using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Database.Core
{
    public class UnitOfWork : IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        private readonly TContext db;
        public UnitOfWork(MyDbContext _db)
        {
            db = _db;
        }

        public void Dispose()
        {
            db.Dispose();             
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters) => db.Database.ExecuteSqlRaw(sql, parameters);

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            // what's the best way to support custom reposity?
            if (hasCustomRepository)
            {
                var customRepo = _context.GetService<IRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        public int SaveChanges(bool ensureAutoHistory = false)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            throw new NotImplementedException();
        }
    }
}
