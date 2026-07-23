using Domain.Contracts;
using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductCountSpecifications : BaseSpecifications<Product>
    {
        public ProductCountSpecifications(ProductSpecParams Params) : base(product =>
         (!Params.BrandId.HasValue || product.ProductBrandId == Params.BrandId)
         &&
         (!Params.TypeId.HasValue || product.ProductTypeId == Params.TypeId)
         )
        {
           

        }
    }
}
