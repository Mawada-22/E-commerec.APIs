using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepo<TKey, TEntity> : IGenericRepo<TKey, TEntity> where TEntity : ModelBase<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepo(StoreDbContext storeDbcontext) 
        {
            _dbContext= storeDbcontext;
        }
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync(BaseSpecifications<TEntity> specifications)
       => await SpecificationsEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
            => asNoTracking ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync() : await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(BaseSpecifications<TEntity> specifications)
        {
            return await SpecificationsEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetByIdAsync(BaseSpecifications<TEntity> specifications)
        
            => await SpecificationsEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        

        public void Update(TEntity entity) => _dbContext.Update(entity);
    }
}
