using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contarcts
{
    public abstract class SpeceficationsAbstracut<T> where T : class
    {
        //feautre for the carteria "like where"
        public Expression<Func<T,bool>> Carteria { get; set; }

        //feature for the list of experssion that i want the entity to include
        public List<Expression<Func<T,object>>> InclueExpressions = new();

        //add prpertyy for orderby and orderbydescending

        public Expression<Func<T,object>>Orderby {  get; set; }
        public Expression<Func<T,object>>OrderbyDescending {  get; set; }

        //declare the exprestion cariera foe the instructor to be as its class paramete 
        protected SpeceficationsAbstracut(Expression<Func<T, bool>> _Carteria)
        { 
            Carteria = _Carteria;
        } 

        //set the orderby properties: 
        protected void setOrderby(Expression<Func<T, object>> expression) => Orderby = expression;
        protected void setOrderbyDescending(Expression<Func<T, object>> expression) => OrderbyDescending = expression;
        
        protected SpeceficationsAbstracut()
        { 
            
        }
        // add method for adding experssion into the list 

        public void AddIncludes(Expression<Func<T, object>> expression )
        {
            InclueExpressions.Add(expression);
        }
    }
}
