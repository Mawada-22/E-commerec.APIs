using AutoMapper;
using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile() 
        { 
        CreateMap<BasketItem,BasketItemDto>().ReverseMap();
        CreateMap<CustmoreBasket, BasketDto>().ReverseMap();
        }

    }
}
