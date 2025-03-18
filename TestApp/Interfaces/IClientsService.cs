using TestApp.Entities;
using TestApp.Entities.Enums;
using TestApp.Entities.Request;
using TestApp.Entities.Response;

namespace TestApp.Interfaces
{
    /// <summary>
    /// Предоставляет интерфейс взаимодействия с источником данных,
    /// содержащим сведения о клиентах и их адресах
    /// </summary>
    public interface IClientsService
    {
        /// <summary>
        /// Получить всех клиентов, или одного конкретного, если указан идентификатор
        /// </summary>
        /// <param name="id">Идентификатор конкретного клиента</param>
        /// <param name="sortBy">Поле по которому нужно провести сортировку</param>
        /// <param name="filter">Фильтр-слово, элементы содержащие его будут исключены</param>
        /// <returns>Коллекция клиентов</returns>
        public IEnumerable<Client> GetClients(long? id = null, string filter = null!, SortingFields? sortBy = null);

        /// <summary>
        /// Получить всех клиентов постранично
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="itemPerPage">Количество элементов на странице</param>
        /// <param name="sortBy">Поле по которому нужно провести сортировку</param>
        /// <param name="filter">Фильтр-слово, элементы содержащие его будут исключены</param>
        /// <returns>Страница с коллекцией клиентов</returns>
        public PaginatedCollection<Client> GetClientByPages(
            int pageNumber = 1, int itemPerPage = 5, string filter = null!, SortingFields? sortBy = null);

        /// <summary>
        /// Обновляет клиента и его адрес
        /// </summary>
        /// <returns>Обновленный клиент, если обновление не прошло, то null</returns>
        public Client UpdateClient(ClientRequest requestClient);

        /// <summary>
        /// Создает клиента и его адрес
        /// </summary>
        /// <param name="clientRequest">Параметры клиента и его адреса</param>
        /// <returns>Параметры созданного клиента и его адреса, null если клиента создать не удалось</returns>
        public Client AddClient(ClientRequest clientRequest);

        /// <summary>
        /// Удаляет выбранного клиента и его адрес
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Результат удаления</returns>
        public bool RemoveClient(long id);

        /// <summary>
        /// Установить адрес у клиентов
        /// </summary>
        /// <param name="filterWithNewAddress">Параметры клиентов и адрес, который нужно им присвоить</param>
        /// <returns>Список клиентов с обновленным адресом</returns>
        public IEnumerable<Client> UpdateAddress(ClientRequest filterWithNewAddress);
    }
}
