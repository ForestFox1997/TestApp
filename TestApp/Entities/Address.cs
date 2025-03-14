namespace TestApp.Entities
{
    /// <summary>
    /// Адрес клиента
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        internal long Id { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public string StreetAddress { get; set; } = "";

        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; } = "";

        /// <summary>
        /// Регион
        /// </summary>
        public string State { get; set; } = "";

        /// <summary>
        /// Почтовый код
        /// </summary>
        public string Zip { get; set; } = "";
    }
}
