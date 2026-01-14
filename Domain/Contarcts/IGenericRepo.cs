using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contarcts
{
    public interface IGenericRepo<TKey,TEntity> where TEntity : ModelBase<TKey>
    {
        public Task<IEnumerable<TEntity>> GatAllAsync(bool asNoTracking = false);
        public Task<TEntity>GetByIdAsync(TKey id);

        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
    }
}
