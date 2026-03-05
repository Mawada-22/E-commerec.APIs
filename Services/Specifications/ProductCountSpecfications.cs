using Domain.Contarcts;
using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductCountSpecfications : SpeceficationsAbstracut<Product>
    {
        public ProductCountSpecfications(ProductSpecParams Params) : base(product =>
         (!Params.BrandId.HasValue || product.ProductBrandId == Params.BrandId)
         &&
         (!Params.TypeId.HasValue || product.ProductTypeId == Params.TypeId)
         )
        {
           

        }
    }
}
