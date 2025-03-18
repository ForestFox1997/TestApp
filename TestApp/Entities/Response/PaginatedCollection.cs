namespace TestApp.Entities.Response
{
    /// <summary>
    /// Страница разделенной на страницы коллекции
    /// </summary>
    /// <typeparam name="T">Тип элемента коллекции</typeparam>
    public class PaginatedCollection<T>
    {
        /// <summary>
        /// Создает элемент постранично-разделенной коллекции элементов
        /// </summary>
        /// <param name="items">Элементы коллекции</param>
        /// <param name="pageNumber">Номер текущей страницы</param>
        /// <param name="pagesTotal">Общее количество страниц</param>
        public PaginatedCollection(ICollection<T> items, int pageNumber, int pagesTotal)
        {
            Items = items;
            PageNumber = pageNumber;
            PagesTotal = pagesTotal;
        }

        /// <summary>
        /// Коллекция разделенных постранично элементов
        /// </summary>
        public ICollection<T> Items { get; }

        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Всего страниц
        /// </summary>
        public int PagesTotal { get; }

    }
}
