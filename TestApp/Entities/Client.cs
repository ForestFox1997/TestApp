namespace TestApp.Entities
{
    /// <summary>
    /// Клиент и его адрес
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Phone { get; set; } = "";

        /// <summary>
        /// Адрес
        /// </summary>
        public Address Address { get; set; } = default!;
        
        /// <summary>
        /// Описание клиента
        /// </summary>
        public string Description { get; set; } = "";
    }
}
