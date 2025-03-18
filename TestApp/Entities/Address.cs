using System.ComponentModel;

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
        [DefaultValue("")]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Город
        /// </summary>
        [DefaultValue("")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Регион
        /// </summary>
        [DefaultValue("")]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Почтовый код
        /// </summary>
        [DefaultValue(0)]
        public long Zip { get; set; }
    }
}
