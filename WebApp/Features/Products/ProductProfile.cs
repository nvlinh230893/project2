using AutoMapper;
using WebApp.Data.Models;

namespace WebApp.Features.Products;

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
