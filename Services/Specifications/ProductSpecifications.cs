using Domain.Contracts;
using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductSpecifications : BaseSpecifications<Product>
    {
        //to include brand and type for getting product by id 
        public ProductSpecifications(int id) : base(P=>P.Id==id)
        {
            AddIncludes(P=>P.productBrand);
            AddIncludes(P=>P.productType);

        }
        public ProductSpecifications(ProductSpecParams Params) :base(product=>
        (!Params.BrandId.HasValue || product.ProductBrandId==Params.BrandId)
        &&
        (!Params.TypeId.HasValue || product.ProductTypeId==Params.TypeId)
        &&
        (string.IsNullOrWhiteSpace(Params.Search)||product.Name.ToLower().Contains(Params.Search.ToLower().Trim()))
        )
        { 
            AddIncludes(P => P.productBrand);
            AddIncludes(P => P.productType);
            //sort here - string values from the storefront ("name" is the default)
            switch (Params.Sort?.ToLower())
            {
                case "priceasc":
                    setOrderby(p => p.Price);
                    break;

                case "pricedesc":
                    setOrderbyDescending(p => p.Price);
                    break;

                case "namedesc":
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
