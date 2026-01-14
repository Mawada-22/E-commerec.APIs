using Domain.Contarcts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Repostories
{
    public class GenericRepo<TKey, TEntity> : IGenericRepo<TKey, TEntity> where TEntity : ModelBase<TKey>
    {
        private readonly StoreDbcontext _dbContext;

        public GenericRepo(StoreDbcontext storeDbcontext) 
        {
            _dbContext= storeDbcontext;
        }
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GatAllAsync(bool asNoTracking = false)
            => asNoTracking ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity) => _dbContext.Update(entity);
    }
}
