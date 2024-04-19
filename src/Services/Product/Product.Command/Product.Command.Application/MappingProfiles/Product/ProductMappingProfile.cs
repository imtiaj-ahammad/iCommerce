using AutoMapper;

namespace Product.Command.Application;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductCommand, Product.Command.Domain.Product>()
                .ForMember(dest => dest.ExtraId,
                    opt =>
                        opt.MapFrom(src => Guid.NewGuid().ToString()));
    }
}