using Domain.Contarcts;
using Domain.Entities;
using Presistance.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Repostories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbcontext _dbContext;
        private readonly ConcurrentDictionary<string,object>Repos;

        public UnitOfWork(StoreDbcontext dbcontext) 
        {
        _dbContext =dbcontext;
            Repos = new();


        }
        public IGenericRepo<TKey, TEntity> GetRepo<TKey, TEntity>() where TEntity : ModelBase<TKey>
        {
            return (IGenericRepo<TKey, TEntity>) Repos.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepo<TKey, TEntity>(_dbContext));
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
