namespace TestApp.Interfaces
{
    /// <summary>
    /// Разные запросы к БД
    /// </summary>
    public interface IVariousRequests
    {
        /// <summary>
        /// Возвращает общее количество вслиентов
        /// </summary>
        /// <returns>Количество клиентов</returns>
        public long GetClientsCount();

        /// <summary>
        /// Возвращает список городов и количество клиентов в них,
        /// либо конкретный город и количество клиентов в нем
        /// </summary>
        /// <param name="currentCity">Город, для которого нужно вернуть количество
        /// клиентов, если null, будет возвращен список всех городов</param>
        /// <returns>Список из городов и количества клиентов в них</returns>
        public IEnumerable<(string City, long ClientsCount)> GetClients(string currentCity = null!);

        /// <summary>
        /// Вернуть города, в которых проживает более одного клиента
        /// </summary>
        /// <returns>Коллекция городов с более чем одним клиентом</returns>
        public IEnumerable<string> GetCountWithManyClients();
    }
}