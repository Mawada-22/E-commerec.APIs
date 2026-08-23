using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public abstract class BaseSpecifications<T> where T : class
    {
        //feautre for the carteria "like where"
        public Expression<Func<T,bool>> Criteria { get; set; }

        //feature for the list of experssion that i want the entity to include
        public List<Expression<Func<T,object>>> IncludeExpressions = new();

        //add prpertyy for orderby and orderbydescending

        public Expression<Func<T,object>>Orderby {  get; set; }
        public Expression<Func<T,object>>OrderbyDescending {  get; set; }

        //add features for teh pagaination

        public int Take {  get; set; }
        public int Skip { get; set; }

        public bool IsPaginated { get; set; }

        //declare the exprestion cariera foe the instructor to be as its class paramete 
        protected BaseSpecifications(Expression<Func<T, bool>> _Criteria)
        { 
            Criteria = _Criteria;
        } 

        //set the orderby properties: 
        protected void setOrderby(Expression<Func<T, object>> expression) => Orderby = expression;
        protected void setOrderbyDescending(Expression<Func<T, object>> expression) => OrderbyDescending = expression;
        
        protected BaseSpecifications()
        { 
            
        }
        // add method for adding experssion into the list 

        public void AddIncludes(Expression<Func<T, object>> expression )
        {
            IncludeExpressions.Add(expression);
        }

        public void ApplyPagination(int pageIndex, int pageSize)
        {
            IsPaginated = true;
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
        }

    }
}
