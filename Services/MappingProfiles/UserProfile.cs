using AutoMapper;
using Domain.Entities.Identity;
using Shared.DTOs;

namespace Services.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
