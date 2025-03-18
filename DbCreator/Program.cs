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

            FillTestRecords(Path.Combine(path, "debugDb.db"));

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
            return testAppDir + "\\TestApp";
        }

        /// <summary>
        /// Создает файл базы данных SQLite, если таковой отсутствует по переданному пути
        /// </summary>
        /// <param name="path">Путь файла</param>
        static void OpenOrCreateDatabaseFile(string path)
        {
            if (!File.Exists(Path.Combine(path, "debugDb.db")))
            {
                File.Create(Path.Combine(path, "debugDb.db")).Close();
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
                            id            INTEGER PRIMARY KEY,
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
                            id          INTEGER PRIMARY KEY,
                            firstName   TEXT,
                            lastName    TEXT,
                            email       TEXT,
                            phone       TEXT,
                            address     INTEGER REFERENCES address (id),
                            description TEXT,
                            isRemoved   INTEGER
                        );
                            ";
                    command = new SqliteCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        /// <summary>
        /// Заполняет базу данных некоторым количеством тестовых записей
        /// </summary>
        /// <param name="databasePath">Путь до файла БД</param>
        private static void FillTestRecords(string databasePath)
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();

                var query = @$"
                    INSERT INTO address VALUES
                        (1, 'ул. Авиаторов', 'Новокузнецк', 'Кемеровская область', 654000),
                        (2, 'ул. Аграрная', 'Белово', 'Кемеровская область', 652600),
                        (3, 'ул. Бережная', 'Новосибирск', 'Новосибирская область', 630000),
                        (4, 'ул. Братская', 'Бердск', 'Новосибирская область', 633000 ),
                        (5, 'ул. Вербная', 'Саратов', 'Саратовская область', 410000),
                        (6, 'ул. Дарвина', 'Пермь', 'Пермсикй край', 614000),
                        (7, 'ул. Ильича', 'Казань', 'Татарстан', 420000),
                        (8, 'ул. Испытателей', 'Казань', 'Татарстан', 420000),
                        (9, 'ул. Колхозная', 'НИжний Новгород', 'Нижегородская область', 603000),
                        (10, 'ул. Машинная', 'Краснодар', 'Краснодарский край', 350000);

                    INSERT INTO clients VALUES
                        (1, 'Никита', 'Зеленский', 'nikita1555@mail.ru', 89609873547 , 1 , 'Никитка))', 0),
                        (2, 'Александр', 'Васин', 'aleks@adsadgfd.com', 89254567841 , 3 , 'Описание', 0),
                        (3, 'Денис', 'Пахоменко', 'deniska@funnayman.net', 89976543289 , 2 , '', 0),
                        (4, 'Данил', 'Павлов', 'killer666@mail.ru', 89615789653 , 4 , 'Funny man!', 0),
                        (5, 'Оля', 'Веселова', 'cougar98765@gmail.com', 89056549371 , 7 , '', 0),
                        (6, 'Вика', 'Стоянова', 'vikusya.cool@rambler.ru', 89033336587 , 8 , '', 0),
                        (7, 'Джонатан', 'Девис', 'j.devil@gmail.com', 89586666666 , 5 , 'Cool man!', 0),
                        (8, 'Василий', 'Яснин', 'vasyan1488@gmail.com', 89065473298 , 6 , 'VASYAN!', 0),
                        (9, 'Геннадий', 'Бояренко', 'Genaryok@proton.com', 89056589321 , 9 , 'Bad boy', 0),
                        (10, 'Ирина', 'Ильина', 'irka-konfetka@mail.com', 899654783659 , 10 , '', 0);
                            ";
                var command = new SqliteCommand(query, connection);
                _ = command.ExecuteNonQuery();
            }
        }
    }
}
