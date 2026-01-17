using Domain.Contarcts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductSpecfications : SpeceficationsAbstracut<Product>
    {
        //to include brand and type for getting product by id 
        public ProductSpecfications(int id) : base(P=>P.Id==id)
        {
            AddIncludes(P=>P.productBrand);
            AddIncludes(P=>P.productType);

        }
        public ProductSpecfications()
        {
            AddIncludes(P => P.productBrand);
            AddIncludes(P => P.productType); 

        }


    }
}
