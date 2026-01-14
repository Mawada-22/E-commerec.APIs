using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contarcts
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync();

        IGenericRepo<TKey,TEntity> GetRepo<TKey, TEntity>() where TEntity:ModelBase<TKey>;
    }
}
