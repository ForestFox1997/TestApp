using Microsoft.AspNetCore.Mvc;
using TestApp.Interfaces;
using TestApp.Entities;
using TestApp.Entities.Enums;
using TestApp.Entities.Request;
using TestApp.Entities.Response;

namespace TestApp.Controllers
{
    /// <summary>
    /// Позволяет получать, изменять и удалять сведения о клиентах и связанных с ними адресов
    /// </summary>
    [ApiController]
    [Route("clients")]
    public class ClientsController : Controller
    {
        private readonly IClientsService _clientService;

        public ClientsController(IClientsService clientsService)
        {
            _clientService = clientsService;
        }

        /// <summary>
        /// Возвращает сведения о клиентах и их адресах
        /// </summary>
        /// <param name="id">Идентификатор нужного клиента, если не указан, возвращаются все клиенты</param>
        /// <param name="sortingByColumn">Поле для сортировки, допустимые значения: firstName, lastName, email</param>
        /// <param name="filter">Фильтр, поля которого определяют возвращаемые элементы</param>
        /// <returns>Сведения о клиентах и адресах</returns>
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Client>> Get(long? id = null, string filter = null!,  string sortingByColumn = null!)
        {
            SortingFields? sortingFiled = null;
            if (sortingByColumn != null)
            {
                if (!Enum.TryParse<SortingFields>(sortingByColumn, true, out _))
                {
                    return BadRequest();
                }
                else
                {
                    Enum.TryParse<SortingFields>(sortingByColumn, true, out var filed);
                    sortingFiled = filed;
                }
            }

            var result = _clientService.GetClients(id, filter, sortingFiled);

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Постранично возвращает сведения о клиентах и их адресах
        /// </summary>
        /// <param name="pageNumber">Идентификатор нужного клиента, если не указан, возвращаются все клиенты</param>
        /// <param name="itemPerPage">Количество элементов на странице (от 1 до 10)</param>
        /// <param name="sortingByColumn">Поле для сортировки, допустимые значения: firstName, lastName, email</param>
        /// <param name="filter">Фильтр, поля которого определяют возвращаемые элементы</param>
        /// <returns>Сведения о клиентах и адресах</returns>
        [HttpGet]
        [Route("byPages")]
        public ActionResult<PaginatedCollection<Client>> GetByPages(int pageNumber = 1,
            int itemPerPage = 5, string filter = null!, string sortingByColumn = null!)
        {
            if (pageNumber < 1 || itemPerPage is < 1 or > 10)
            {
                return BadRequest();
            }

            SortingFields? sortingFiled = null;
            if (sortingByColumn != null)
            {
                if (!Enum.TryParse<SortingFields>(sortingByColumn, true, out _))
                {
                    return BadRequest();
                }
                else
                {
                    Enum.TryParse<SortingFields>(sortingByColumn, true, out var filed);
                    sortingFiled = filed;
                }
            }

            var pageWithClients = _clientService.GetClientByPages(pageNumber, itemPerPage, filter, sortingFiled);

            return Ok(pageWithClients);
        }

        /// <summary>
        /// Добавляет сведения о клиенте и его адресе
        /// </summary>
        /// <param name="clientRequest">Сведения о клиенте и его адресе</param>
        /// <returns>Созданный клиент</returns>
        [HttpPut]
        [Route("")]
        public ActionResult<Client> Add(ClientRequest clientRequest)
        {
            var createdClient = _clientService.AddClient(clientRequest);

            if (createdClient == null)
            {
                return BadRequest();
            }

            return Ok(createdClient);
        }

        /// <summary>
        /// Обновляет сведения о клиенте и его адресе
        /// </summary>
        /// <returns>Результат выполнения операции</returns>
        [HttpPatch]
        [Route("{id}")]
        public ActionResult<Client> Patch(long id, ClientRequest client)
        {
            client.Id = id;

            var updatedClient = _clientService.UpdateClient(client);

            if (updatedClient == null)
            {
                return BadRequest();
            }

            return Ok(updatedClient);
        }

        /// <summary>
        /// Удаляет сведения о клиенте и его адресе
        /// </summary>
        /// <returns>Результат выполнения операции</returns>
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(long id)
        {
            var deletingResult = _clientService.RemoveClient(id);

            if (!deletingResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Меняет адрес у клиентов, которые удовлетворяют переданным в фильтре условиям
        /// </summary>
        /// <param name="filterWithNewAddress">Фильтр-условие, определяющее клиентов и их адрес</param>
        /// <returns>Результат операции/количество сущностей</returns>
        [HttpPatch]
        [Route("changeAddress")]
        public ActionResult<IEnumerable<Client>> ChangeAddress(ClientRequest filterWithNewAddress)
        {
            var clientsWithUpdatedAddress = _clientService.UpdateAddress(filterWithNewAddress);

            if (!clientsWithUpdatedAddress.Any())
            {
                return BadRequest();
            }

            return Ok(clientsWithUpdatedAddress);
        }
    }
}
