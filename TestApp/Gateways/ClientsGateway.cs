using System.Data.Common;
using Microsoft.Data.Sqlite;
using TestApp.Interfaces;
using TestApp.Entities;
using TestApp.Entities.Enums;
using TestApp.Entities.Request;

namespace TestApp.Gateways
{
    /// <summary>
    /// Слой взаимодействия с БД
    /// </summary>
    public class ClientsGateway : IClientsGateway, IVariousRequests
    {
        private readonly string _connectionString;

        public ClientsGateway(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TestAppDb")
                ?? throw new ArgumentNullException("TestAppDb", "Не найдена connection string");
            DatabaseCheck();
        }

        /// <summary>
        /// Производит проверку, что файл базы данных из connection string существует, и в нем находятся таблицы
        /// 'clients' и 'address'. Выкидывает исключение, если какая-то часть проверки успешно не завершилась
        /// </summary>
        /// <exception cref="FileNotFoundException">Файл базы данных не был найден</exception>
        /// <exception cref="DbException">Проблема с соединением БД, либо доступом к таблицам</exception>
        private void DatabaseCheck()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                if (!File.Exists(connection.DataSource))
                {
                    throw new FileNotFoundException("Не найден файл базы данных", connection.DataSource);
                }

                try
                {
                    connection.Open();

                    var query = "SELECT COUNT(*) FROM clients LEFT JOIN address ON clients.address = address.id";
                    var command = new SqliteCommand(query, connection);
                    var reader = command.ExecuteReader();
                    _ = reader.Read();

                    connection.Close();
                }
                catch (Exception e)
                {
                    if (e as DbException != null)
                    {
                        throw (DbException)e;
                    }

                    throw;
                }
            }
        }

        public IEnumerable<Client> GetClients(SortingFields? sortBy = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // TODO !! Написать

                //_ = GenerateSqlQueryForUpdateClientsAddress(filterFields);

                var query = $@"
                    SELECT * FROM clients
                    LEFT JOIN address ON clients.address = address.id
                    WHERE isRemoved = 0
                    {(sortBy == null ? "" : $" ORDER BY {sortBy}")}
                            ";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();

                var clients = new List<Client>();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt64(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Phone = reader.GetString(4),
                        Address = new Address
                        {
                            Id = reader.GetInt64(8),
                            StreetAddress = reader.GetString(9),
                            City = reader.GetString(10),
                            State = reader.GetString(11),
                            Zip = reader.GetInt64(12)
                        },
                        Description = reader.GetString(6)
                    });
                }

                return clients;
            }
        }

        /// <summary>
        /// Возвращает коллекцию клиентов и их адресов по их идентификаторам
        /// </summary>
        /// <param name="identifiers">Идентификаторы клиентов</param>
        /// <returns></returns>
        private IEnumerable<Client> GetClients(IEnumerable<long> identifiers)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = $@"
                    SELECT * FROM clients LEFT JOIN address ON clients.address = address.id
                    WHERE clients.id IN ({string.Join(',', identifiers)}) AND isRemoved = 0
                    ";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();

                var clients = new List<Client>();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt64(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Phone = reader.GetString(4),
                        Address = new Address
                        {
                            Id = reader.GetInt64(8),
                            StreetAddress = reader.GetString(9),
                            City = reader.GetString(10),
                            State = reader.GetString(11),
                            Zip = reader.GetInt64(12)
                        },
                        Description = reader.GetString(6)
                    });
                }

                return clients;
            }
        }

        public IEnumerable<Client> GetClientsByPage(int pageNumber, int itemPerPage, SortingFields? sortBy = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var clientsCount = GetClientsCount();

                var query = $@"
                    SELECT * FROM clients
                    LEFT JOIN address ON clients.address = address.id
                    {(sortBy == null ? "" : $" ORDER BY {sortBy}")}
                    LIMIT {itemPerPage} OFFSET {(pageNumber - 1) * itemPerPage}
                            ";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();

                var clients = new List<Client>();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt64(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Phone = reader.GetString(4),
                        Address = new Address
                        {
                            Id = reader.GetInt64(8),
                            StreetAddress = reader.GetString(9),
                            City = reader.GetString(10),
                            State = reader.GetString(11),
                            Zip = reader.GetInt64(12)
                        },
                        Description = reader.GetString(6)
                    });
                }

                return clients;
            }
        }

        public Client GetClient(long id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                Client client = null!;

                var query = $@"
                    SELECT * FROM clients LEFT JOIN address ON clients.address = address.id
                    WHERE clients.id = {id} AND isRemoved = 0
                            ";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    client = new Client
                    {
                        Id = reader.GetInt64(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Phone = reader.GetString(4),
                        Address = new Address
                        {
                            Id = reader.GetInt64(8),
                            StreetAddress = reader.GetString(9),
                            City = reader.GetString(10),
                            State = reader.GetString(11),
                            Zip = reader.GetInt64(12)
                        },
                        Description = reader.GetString(6)
                    };
                }

                return client;
            }
        }

        public Client AddClient(ClientRequest clientRequest)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT MAX(address.id), MAX(clients.id) FROM address, clients";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();
                reader.Read();
                var identifiers = new { MaxAddressId = reader.GetInt64(0), MaxClientId = reader.GetInt64(1) };

                query = @$"
                    INSERT INTO address VALUES (
                        {identifiers.MaxAddressId + 1},
                        '{clientRequest.Address.StreetAddress}',
                        '{clientRequest.Address.City}',
                        '{clientRequest.Address.State}',
                        {clientRequest.Address.Zip})";
                command = new SqliteCommand(query, connection);
                _ = command.ExecuteNonQuery();

                query = @$"
                    INSERT INTO clients VALUES (
                        {identifiers.MaxClientId + 1},
                        '{clientRequest.FirstName}',
                        '{clientRequest.LastName}',
                        '{clientRequest.Email}',
                        '{clientRequest.Phone}',
                        {identifiers.MaxAddressId + 1},
                        '{clientRequest.Description}',
                        0)";
                command = new SqliteCommand(query, connection);
                _ = command.ExecuteNonQuery();

                var newClient = GetClient(identifiers.MaxClientId + 1);

                return newClient;
            }
        }

        public Client UpdateClient(ClientRequest clientRequest)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                Client client = GetClient(clientRequest.Id);
                if (client == null)
                {
                    return null!;
                }
                else
                {
                    var query = @$"
                        UPDATE address
                        SET
                            streetAddress = '{clientRequest.Address.StreetAddress}',
                            city = '{clientRequest.Address.City}',
                            state = '{clientRequest.Address.State}',
                            zip = {clientRequest.Address.Zip}
                        WHERE id = {client.Address.Id}
                                ";
                    var command = new SqliteCommand(query, connection);
                    _ = command.ExecuteNonQuery();

                    query = @$"
                        UPDATE clients
                        SET
                            firstName = '{clientRequest.FirstName}',
                            lastName = '{clientRequest.LastName}',
                            email = '{clientRequest.Email}',
                            phone = '{clientRequest.Phone}',
                            description = '{clientRequest.Description}'
                        WHERE id = {client.Id}
                                ";
                    command = new SqliteCommand(query, connection);
                    _ = command.ExecuteNonQuery();

                    var updatedClient = GetClient(clientRequest.Id);

                    return updatedClient;
                }
            }
        }

        public bool RemoveClient(long id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = $"SELECT COUNT(*) FROM clients WHERE id = {id}";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();
                reader.Read();

                var clientExists = Convert.ToBoolean(reader.GetInt32(0));
                if (!clientExists)
                {
                    return false;
                }
                else
                {
                    query = $"UPDATE clients SET isRemoved = 1 WHERE id = {id}";
                    command = new SqliteCommand(query, connection);
                    _ = command.ExecuteNonQuery();

                    return true;
                }
            }
        }

        public IEnumerable<Client> UpdateAddress(ClientRequest filterWithNewAddress)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = GenerateSqlQueryForUpdateClientsAddress(filterWithNewAddress);
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();

                var addressesForUpdate = new List<long>();
                while (reader.Read())
                {
                    addressesForUpdate.Add(reader.GetInt64(0));
                }

                query = $@"
                    UPDATE address
                    SET
                        streetAddress = '{filterWithNewAddress.Address.StreetAddress}',
                        city = '{filterWithNewAddress.Address.City}',
                        state = '{filterWithNewAddress.Address.State}',
                        zip = '{filterWithNewAddress.Address.Zip}'
                    WHERE id IN ({string.Join(',', addressesForUpdate)})
                        ";
                command = new SqliteCommand(query, connection);
                _ = command.ExecuteNonQuery();

                var clientsIdentifiers = new List<long>();
                query = $@"
                    SELECT clients.id
                    FROM address
                        LEFT JOIN clients ON address.id = clients.address
                    WHERE address.id IN ({string.Join(",", addressesForUpdate)}) 
                        ";
                command = new SqliteCommand(query, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clientsIdentifiers.Add(reader.GetInt64(0));
                }

                var clientsWithUpdatedAddress = GetClients(clientsIdentifiers);

                return clientsWithUpdatedAddress;
            }
        }

        /// <summary>
        /// Генерирует текст SQL запроса, возвращающего идентификаторы адресов клиентов,
        /// на основании того, какие поля задействованы в фильтре
        /// </summary>
        /// <param name="filterWithNewAddress">Фильтр - набор задействованных полей</param>
        /// <returns>SQL запрос, возвращающий идентификаторы адресов клиентов в соотв. с фильтром</returns>
        private string GenerateSqlQueryForUpdateClientsAddress(ClientRequest filterWithNewAddress)
        {
            var query = "SELECT address FROM clients WHERE";

            if (!string.IsNullOrEmpty(filterWithNewAddress.FirstName))
            {
                query += $" firstName = '{filterWithNewAddress.FirstName}' AND";
            }

            if (!string.IsNullOrEmpty(filterWithNewAddress.LastName))
            {
                query += $" lastName = '{filterWithNewAddress.LastName}' AND";
            }

            if (!string.IsNullOrEmpty(filterWithNewAddress.Email))
            {
                query += $" email = '{filterWithNewAddress.Email}' AND";
            }

            if (!string.IsNullOrEmpty(filterWithNewAddress.Phone))
            {
                query += $" phone = '{filterWithNewAddress.Phone}' AND";
            }

            if (!string.IsNullOrEmpty(filterWithNewAddress.Description))
            {
                query += $" description = '{filterWithNewAddress.Description}'";
            }

            if (query.EndsWith("AND"))
            {
                query = query[..^3];
            }

            return query;
        }
        #region Реализация разных запросов к БД
        public long GetClientsCount()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT COUNT(id) FROM clients";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();
                reader.Read();
                var result = reader.GetInt64(0);

                return result;
            }
        }

        public IEnumerable<(string City, long ClientsCount)> GetClients(string? currentCity = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = @$"
                    SELECT city, COUNT(clients.id)
                    FROM address
                        INNER JOIN clients WHERE address.id = clients.address
                        {(currentCity == null ? "" : "AND city = {currentCity}")}
                    GROUP BY city
                            ";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();
                reader.Read();

                var clientCountByCityList = new List<(string City, long ClientsCount)>();
                while (reader.Read())
                {
                    clientCountByCityList.Add((reader.GetString(0), reader.GetInt64(1)));
                }

                return clientCountByCityList;
            }
        }

        public IEnumerable<string> GetCountWithManyClients()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = @$"                
                    WITH clientsCountByCities AS (
                        SELECT city, COUNT(clients.id) AS clientsCount
                        FROM address INNER JOIN clients
                        WHERE address.id = clients.address
                        GROUP BY city)
                    SELECT city FROM clientsCountByCities
                    WHERE clientsCount > 1
                            ";
                var command = new SqliteCommand(query, connection);
                var reader = command.ExecuteReader();

                var cities = new List<string>();
                while (reader.Read())
                {
                    cities.Add(reader.GetString(0));
                }

                return cities;
            }
        }
        #endregion Реализация разных запросов к БД
    }
}
