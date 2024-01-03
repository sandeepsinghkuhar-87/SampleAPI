using AutoMapper;
using SampleAPI.Entities;
using SampleAPI.Requests;
using SampleAPI.Response;

namespace SampleAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrderRequest, Order>().ReverseMap();
            CreateMap<OrderResponse, Order>().ReverseMap();
        }
    }
}
