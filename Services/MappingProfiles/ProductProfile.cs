using AutoMapper;
using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ForMember(D => D.BrandName, options => options.MapFrom(s => s.productBrand.Name)).ReverseMap();
            CreateMap<ProductBrand, BrandDto>().ReverseMap();
            CreateMap<ProductType,TypeDto>().ReverseMap();
        
        }
    }
}
