using System.ComponentModel;

namespace TestApp.Entities.Request
{
    /// <summary>
    /// Клиент и его адрес
    /// </summary>
    public class ClientRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        internal long Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [DefaultValue("")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Фамилия
        /// </summary>
        [DefaultValue("")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [DefaultValue("")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Номер телефона
        /// </summary>
        [DefaultValue("")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Адрес
        /// </summary>
        public required Address Address { get; set; }

        /// <summary>
        /// Описание клиента
        /// </summary>
        [DefaultValue("")]
        public string Description { get; set; } = string.Empty;
    }
}