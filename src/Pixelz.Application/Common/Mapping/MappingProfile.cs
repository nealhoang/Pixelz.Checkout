namespace Pixelz.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<OrderAddress, AddressDto>()
            .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()));

        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
           .ConvertUsing(typeof(PagedResultConverter<,>));
    }
}
