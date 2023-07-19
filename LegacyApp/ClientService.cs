using LegacyApp.Repositories;

namespace LegacyApp.Services
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepository;

        public ClientService(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client GetClientById(int id)
        {
            return _clientRepository.GetById(id);
        }
    }
}