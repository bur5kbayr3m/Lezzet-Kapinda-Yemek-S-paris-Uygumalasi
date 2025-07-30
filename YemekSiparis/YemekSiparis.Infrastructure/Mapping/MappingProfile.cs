using AutoMapper;
using YemekSiparis.Domain.Entities;
using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Infrastructure 
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Store, StoreDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

        }
    }
}
