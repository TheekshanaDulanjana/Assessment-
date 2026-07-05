using AutoMapper;
using SalesOrderApp.Application.DTOs;
using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDto>();
            CreateMap<Item, ItemDto>();

            CreateMap<SalesOrder, SalesOrderListDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Client!.CustomerName));

            CreateMap<SalesOrder, SalesOrderDetailDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Client!.CustomerName))
                .ForMember(d => d.Address1, o => o.MapFrom(s => s.Client!.Address1))
                .ForMember(d => d.Address2, o => o.MapFrom(s => s.Client!.Address2))
                .ForMember(d => d.Address3, o => o.MapFrom(s => s.Client!.Address3))
                .ForMember(d => d.Suburb, o => o.MapFrom(s => s.Client!.Suburb))
                .ForMember(d => d.State, o => o.MapFrom(s => s.Client!.State))
                .ForMember(d => d.PostCode, o => o.MapFrom(s => s.Client!.PostCode))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items.OrderBy(i => i.LineNumber)));

            CreateMap<SalesOrderItem, SalesOrderItemDto>();
        }
    }
}
