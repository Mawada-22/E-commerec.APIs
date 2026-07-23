using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly ConcurrentDictionary<string,object>Repos;

        public UnitOfWork(StoreDbContext dbcontext) 
        {
        _dbContext =dbcontext;
            Repos = new();


        }
        public IGenericRepo<TKey, TEntity> GetRepo<TKey, TEntity>() where TEntity : ModelBase<TKey>
        {
            return (IGenericRepo<TKey, TEntity>) Repos.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepo<TKey, TEntity>(_dbContext));
        }

        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
