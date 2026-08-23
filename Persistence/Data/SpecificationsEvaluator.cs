using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> query, BaseSpecifications<T> specifications) where T : class 
        {
            //query is teh _dbcontext set 
            var result = query;

            //apply the cartira on it 
            if (specifications.Criteria is not null)
                result = result.Where(specifications.Criteria);

            //aggraiate the all exprestions at the spresfications into teh query 

            result = specifications.IncludeExpressions.Aggregate(result,(currnt,Expressions)=>currnt.Include(Expressions));

            //cheack for sorting 
            if (specifications.Orderby is not null)
                result = result.OrderBy(specifications.Orderby);
            else if (specifications.OrderbyDescending is not null)
                result = result.OrderByDescending(specifications.OrderbyDescending);

            //pagination
            if(specifications.IsPaginated)
                result=result.Skip(specifications.Skip).Take(specifications.Take);  
             
                return result;
       
                }
        

        }

    }

