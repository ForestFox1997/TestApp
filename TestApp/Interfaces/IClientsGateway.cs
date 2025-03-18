using TestApp.Entities;
using TestApp.Entities.Enums;
using TestApp.Entities.Request;

namespace TestApp.Interfaces
{
    /// <summary>
    /// Интерфейс слоя взаимодействия с источником данных по клиентам и их адресам
    /// </summary>
    public interface IClientsGateway
    {
        /// <summary>
        /// Получить всех клиентов из источника данных
        /// </summary>
        /// <param name="sortBy">Поле по которому нужно провести сортировку</param>
        /// <returns>Коллекция клиентов</returns>
        IEnumerable<Client> GetClients(SortingFields? sortBy = null);

        /// <summary>
        /// Получить клиентов, используя постраничное разделение 
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="itemPerPage">Количество клиентов на странице</param>
        /// <param name="sortBy">Поле по которому нужно провести сортировку</param>
        /// <returns></returns>
        IEnumerable<Client> GetClientsByPage(int pageNumber, int itemPerPage, SortingFields? sortBy = null);

        /// <summary>
        /// Получить клиента по идентификатору из источника данных
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        Client GetClient(long id);

        /// <summary>
        /// Обновить сведения о клиенете и его адресе
        /// </summary>
        /// <returns>Флаг успешности обновления</returns>
        public Client UpdateClient(ClientRequest clientRequest);

        /// <summary>
        /// Добавляет сведения о клиенте и его адрес
        /// </summary>
        /// <param name="clientRequest">Параметры клиента и адреса</param>
        /// <returns>Созданный клиент</returns>
        public Client AddClient(ClientRequest clientRequest);

        /// <summary>
        /// Удаляет (помечает удаленным) клиента и его адрес
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Результат удаления, если false - клиент не найден</returns>
        public bool RemoveClient(long id);

        /// <summary>
        /// Установить адрес у клиентов
        /// </summary>
        /// <param name="filterWithNewAddress">Параметры клиентов и адрес, который нужно им присвоить</param>
        /// <returns>Список клиентов с обновленным адресом</returns>
        public IEnumerable<Client> UpdateAddress(ClientRequest filterWithNewAddress);
    }
}
