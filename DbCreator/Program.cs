using Microsoft.Data.Sqlite;

namespace DbCreator
{
    /// <summary>
    /// Утилита создает пригодную для дебага веб-приложения TestApp базу данных SQLite,
    /// и генерирует в ней тестовые таблицы
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = GetAppPathForWindows();

            OpenOrCreateDatabaseFile(path);

            CreateTablesIfTheyNotExist(Path.Combine(path, "debugDb.db"));

            Console.WriteLine("=== Database created ===");
        }

        /// <summary>
        /// Получение debug-директории проекта TestApp. Актуально для debug-сборки под Windows
        /// </summary>
        /// <returns>Путь до исполняемого файла TestApp</returns>
        static string GetAppPathForWindows()
        {
            string netDir = Directory.GetCurrentDirectory();
            string debugDir = Directory.GetParent(netDir)!.FullName;
            string binDir = Directory.GetParent(debugDir)!.FullName;
            string dbCreatorDir = Directory.GetParent(binDir)!.FullName;
            string testAppDir = Directory.GetParent(dbCreatorDir)!.FullName;
            return testAppDir + "\\TestApp\\bin\\Debug\\net8.0";
        }

        /// <summary>
        /// Создает файл базы данных SQLite, если таковой отсутствует по переданному пути
        /// </summary>
        /// <param name="path">Путь файла</param>
        static void OpenOrCreateDatabaseFile(string path)
        {
            if (!File.Exists(Path.Combine(path, "debugDb.db")))
            {
                File.Create(Path.Combine(path, "debugDb.db"));
            }
        }

        /// <summary>
        /// Создает нужные таблицы в файле БД, если они отсутствуют
        /// </summary>
        /// <param name="databasePath">Путь до файла БД</param>
        private static void CreateTablesIfTheyNotExist(string databasePath)
        {
            string query = null!;
            SqliteCommand command = null!;
            SqliteDataReader reader = null!;
            long? result = null!;

            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();

                // проверка наличия таблицы 'address'
                query = "SELECT COUNT(*) FROM sqlite_master WHERE tbl_name = 'address'";
                command = new SqliteCommand(query, connection);
                reader = command.ExecuteReader();
                reader.Read();
                result = (long)reader.GetValue(0);

                // создание таблицы 'address', если она отсутствует
                if (!Convert.ToBoolean(result))
                {
                    query = @"
                        CREATE TABLE address (
                            id            NUMERIC PRIMARY KEY,
                            streetAddress TEXT,
                            city          TEXT,
                            state         TEXT,
                            zip           NUMERIC
                        );
                            ";
                    command = new SqliteCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                // проверка наличия таблицы 'clients'
                query = "SELECT COUNT(*) FROM sqlite_master WHERE tbl_name = 'clients'";
                command = new SqliteCommand(query, connection);
                reader = command.ExecuteReader();
                reader.Read();
                result = (long)reader.GetValue(0);

                // создание таблицы 'clients', если она отсутствует
                if (!Convert.ToBoolean(result))
                {
                    query = @"
                        CREATE TABLE clients (
                            id          NUMERIC PRIMARY KEY,
                            firstName   TEXT,
                            lastName    TEXT,
                            email       TEXT,
                            phone       TEXT,
                            address     INTEGER REFERENCES address (id),
                            description TEXT
                        );
                            ";
                    command = new SqliteCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
