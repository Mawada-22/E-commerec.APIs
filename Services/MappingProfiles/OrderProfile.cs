using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryCost, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.SubTotal + s.DeliveryMethod.Price))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}
