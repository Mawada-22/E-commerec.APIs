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
        public ProductSpecfications(string?sort, int? BrandId, int? TypeId) :base(product=>
        (!BrandId.HasValue || product.ProductBrandId==BrandId)
        &&
        (!TypeId.HasValue || product.ProductTypeId==TypeId)
        )
        {
            AddIncludes(P => P.productBrand);
            AddIncludes(P => P.productType);
            //sort here 

            if(!string.IsNullOrEmpty(sort)) 
            {
                switch (sort.ToLower().Trim()) 
                {
                    case "priceasc":
                        setOrderby(P => P.Price);
                        break;

                    case "pricedesc":
                        setOrderbyDescending(P => P.Price);
                        break;

                    case "nameasc":
                        setOrderby(P => P.Name);
                        break;

                    case "namedesc":
                        setOrderbyDescending(P => P.Name);
                        break;

                    default:
                        setOrderby(P=>P.Name);
                        break;



                }
            }

        }



    }
}
