using AutoMapper;
using SalesOrderApp.Application.DTOs;
using SalesOrderApp.Application.Interfaces;

namespace SalesOrderApp.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ItemDto>> GetAllAsync()
        {
            var items = await _uow.Items.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ItemDto>>(items);
        }
    }
}
