using Microsoft.AspNetCore.Mvc;
using TestApp.Entities;

namespace TestApp.Controllers
{
    /// <summary>
    /// Позволяет получать, изменять и удалять сведения о клиентах и связанных с ними адресов
    /// </summary>
    [ApiController]
    [Route("clients")]
    public class ClientsController : Controller
    {
        /// <summary>
        /// Возвращает сведения о клиенте и его адресе
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Сведения о клиенте и адресе</returns>
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Client> Get(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновляет сведения о клиенте и его адресе
        /// </summary>
        /// <returns>Результат выполнения операции</returns>
        [HttpPatch]
        [Route("{id}")]
        public ActionResult Patch()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаляет сведения о клиенте и его адресе
        /// </summary>
        /// <returns>Результат выполнения операции</returns>
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Меняет адрес у клиентов, которые удовлетворяют переданным в фильтре условиям
        /// </summary>
        /// <param name="filter">Фильтр-условие, определяющее клиентов</param>
        /// <returns>Результат операции/количество сущностей</returns>
        [HttpPatch]
        [Route("changeAddress")]
        public ActionResult ChangeAddress(object filter)
        {
            throw new NotImplementedException();
        }
    }
}
