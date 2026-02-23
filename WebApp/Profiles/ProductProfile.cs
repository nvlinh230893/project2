using AutoMapper;
using WebApp.DTOs;
using WebApp.Data.Models;

namespace WebApp.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductEntity, ProductDto>();
        CreateMap<CreateProductDto, ProductEntity>();
        CreateMap<UpdateProductDto, ProductEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
