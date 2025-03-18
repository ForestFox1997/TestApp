using System.Text.RegularExpressions;
using TestApp.Interfaces;
using TestApp.Entities;
using TestApp.Entities.Enums;
using TestApp.Entities.Request;
using TestApp.Entities.Response;

namespace TestApp.Services
{
    /// <summary>
    /// Сервис взаимодействия с источником данных о клиентах и их адресах
    /// </summary>
    public class ClientsService : IClientsService
    {
        private readonly IClientsGateway _clientsGateway;
        private readonly IVariousRequests _variousRequestsGateway;

        public ClientsService(IClientsGateway clientsGateway, IVariousRequests variousRequestsGateway)
        {
            _clientsGateway = clientsGateway;
            _variousRequestsGateway = variousRequestsGateway;
        }

        /// <summary>
        /// Провести валидацию клиента и его адреса
        /// </summary>
        /// <param name="clientRequest">Клиент и его адрес</param>
        /// <returns>Результат валидации</returns>
        private bool ValidateClient(ClientRequest clientRequest)
        {
            if (!Regex.IsMatch(clientRequest.FirstName, "^[a-zA-Zа-яА-Я]*$") ||
                !Regex.IsMatch(clientRequest.LastName, "^[a-zA-Zа-яА-Я]*$"))
            {
                return false;
            }

            if (!Regex.IsMatch(clientRequest.Email, "^[A-Z0-9._%+-]+@[A-Z0-9-]+.+.[A-Z]{2,4}$"))
            {
                return false;
            }

            if (!Regex.IsMatch(clientRequest.Phone, @"^\+?[1-9][0-9]{5,15}$"))
            {
                return false;
            }

            if (!ValidateAddress(clientRequest.Address))
            {
                return false;
            }

            return true;
        }

        private bool ValidateAddress(Address address)
        {
            if (!Regex.IsMatch(address.StreetAddress, "^[a-zA-Zа-яА-Я0-9 .-]*$") ||
                !Regex.IsMatch(address.City, "^[a-zA-Zа-яА-Я-]*$") ||
                !Regex.IsMatch(address.State, "^[a-zA-Zа-яА-Я]*$") ||
                !Regex.IsMatch(address.Zip.ToString(), "^[0-9]{6,6}$"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Исключает из коллекции результаты, поля которых содержат строку-фильтр
        /// </summary>
        /// <param name="elements">Изначальная коллекция элементов</param>
        /// <param name="filter">Строка для исключения элемента</param>
        /// <returns>Отфильтрованная коллекция</returns>
        private IEnumerable<Client> ClearResultByFilter(IEnumerable<Client> elements, string filter)
        {
            List<Client> clients = [.. elements];
            for (int i = 0; i < clients.Count; i++)
            {
                var client = clients[i];
                var address = client.Address;

                var clientProperties = client.GetType().GetProperties().Where(p => p.GetType() != typeof(Address));
                if (clientProperties.Any(p => p.GetValue(client)!.ToString()!.Contains(filter)))
                {
                    clients.Remove(client);
                    continue;
                }

                var addressProperties = address.GetType().GetProperties();
                if (addressProperties.Any(p => p.GetValue(address)!.ToString()!.Contains(filter)))
                {
                    clients.Remove(client);
                    continue;
                }
            }

            return clients;
        }

        public IEnumerable<Client> GetClients(long? id = null, string filter = null!, SortingFields? sortBy = null)
        {
            var result = Enumerable.Empty<Client>();

            if (id != null)
            {
                var client = _clientsGateway.GetClient((long)id);
                if (client != null)
                {
                    result = [client];
                }
            }
            else
            {
                result = _clientsGateway.GetClients(sortBy);
                if (!string.IsNullOrEmpty(filter))
                {
                    result = ClearResultByFilter(result, filter);
                }
            }

            return result;
        }

        public PaginatedCollection<Client> GetClientByPages(int pageNumber = 1,
            int itemPerPage = 5, string filter = null!, SortingFields? sortBy = null)
        {
            var clients = _clientsGateway.GetClientsByPage(pageNumber, itemPerPage, sortBy);
            if (!string.IsNullOrEmpty(filter))
            {
                clients = ClearResultByFilter(clients, filter);
            }

            var totalPages = (int)Math.Ceiling(_variousRequestsGateway.GetClientsCount() / (double)itemPerPage);

            var pageWithClients = new PaginatedCollection<Client>(
                (ICollection<Client>)clients, pageNumber, totalPages);

            return pageWithClients;
        }

        public Client UpdateClient(ClientRequest clientRequest)
        {
            var clientIsValid = ValidateClient(clientRequest);

            if (!clientIsValid)
            {
                return null!;
            }

            var updatedClient = _clientsGateway.UpdateClient(clientRequest);

            return updatedClient!;
        }

        public Client AddClient(ClientRequest clientRequest)
        {
            var clientIsValid = ValidateClient(clientRequest);

            if (!clientIsValid)
            {
                return null!;
            }

            var createdClient = _clientsGateway.AddClient(clientRequest);
            
            return createdClient;
        }

        public bool RemoveClient(long id)
        {
            var removingResult = _clientsGateway.RemoveClient(id);

            return removingResult;
        }

        public IEnumerable<Client> UpdateAddress(ClientRequest filterWithNewAddress)
        {
            var clientIsValid = ValidateAddress(filterWithNewAddress.Address);

            if (!clientIsValid)
            {
                return Enumerable.Empty<Client>();
            }

            var clientsWithUpdatedAddress = _clientsGateway.UpdateAddress(filterWithNewAddress);

            return clientsWithUpdatedAddress;
        }
    }
}
