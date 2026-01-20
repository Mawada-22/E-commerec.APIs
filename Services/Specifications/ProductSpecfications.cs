using Domain.Contarcts;
using Domain.Entities;
using Shared;
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
        public ProductSpecfications(ProductSpecParams Params) :base(product=>
        (!Params.BrandId.HasValue || product.ProductBrandId==Params.BrandId)
        &&
        (!Params.TypeId.HasValue || product.ProductTypeId==Params.TypeId)
        &&
        (string.IsNullOrWhiteSpace(Params.Search)||product.Name.ToLower().Contains(Params.Search.ToLower().Trim()))
        )
        { 
            AddIncludes(P => P.productBrand);
            AddIncludes(P => P.productType);
            //sort here 
            switch (Params.Sort)
            {
                case ProductSortingOptions.PriceAsc:
                    setOrderby(p => p.Price);
                    break;

                case ProductSortingOptions.PriceDesc:
                    setOrderbyDescending(p => p.Price);
                    break;

                case ProductSortingOptions.NameDesc:
                    setOrderbyDescending(p => p.Name);
                    break;

                default:
                    setOrderby(p => p.Name);
                    break;
            }
            ApplyPagination(Params.PageIndex, Params.PageSize);

        }
        



    }
}
