using Domain.Contarcts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Data
{
    public static class SpecficationsEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> query, SpeceficationsAbstracut<T> specefications) where T : class 
        {
            //query is teh _dbcontext set 
            var result = query;

            //apply the cartira on it 
            if (specefications.Carteria is not null)
                result = result.Where(specefications.Carteria);

            //aggraiate the all exprestions at the spresfications into teh query 

            result = specefications.InclueExpressions.Aggregate(result,(currnt,Expressions)=>currnt.Include(Expressions));

            //cheack for sorting 
            if (specefications.Orderby is not null)
                result = result.OrderBy(specefications.Orderby);
            else if (specefications.OrderbyDescending is not null)
                result = result.OrderByDescending(specefications.OrderbyDescending);
             
                return result;
       
                }
        

        }

    }

