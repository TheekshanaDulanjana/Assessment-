using AutoMapper;
using SalesOrderApp.Application.DTOs;
using SalesOrderApp.Application.Interfaces;

namespace SalesOrderApp.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ClientService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ClientDto>> GetAllAsync()
        {
            var clients = await _uow.Clients.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ClientDto>>(clients);
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            var client = await _uow.Clients.GetByIdAsync(id);
            return client is null ? null : _mapper.Map<ClientDto>(client);
        }
    }
}
