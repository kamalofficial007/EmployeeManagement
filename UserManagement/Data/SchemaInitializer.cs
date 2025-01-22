using System.Data;

namespace UserManagement.Data
{
    public class SchemaInitializer
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _dbFilePath;
        private readonly string _contentRootPath;

        public static string ApplicationFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UserManagement"); } }
        public static string SQLiteDatabaseFolder { get { return Path.Combine(ApplicationFolder, "database\\"); } }

        public SchemaInitializer(IDbConnection dbConnection, IHostEnvironment environment)
        {
            // Extract the SQLite database file path from the connection string
            _dbConnection = dbConnection;
            // Use IHostEnvironment to get the root directory of the project
            _contentRootPath = environment.ContentRootPath;
            var connectionString = _dbConnection.ConnectionString;
            var startIndex = connectionString.IndexOf("Data Source=", StringComparison.OrdinalIgnoreCase) + "Data Source=".Length;
            _dbFilePath = connectionString.Substring(startIndex);
        }

        public void Initialize()
        {
            // Check if the database file already exists
            string dbFilePath = SQLiteDatabaseFolder + _dbFilePath;
            if (File.Exists(dbFilePath))
            {
                Console.WriteLine("Database already exists. Skipping schema initialization.");
                return;
            }
            else
            {
                Console.WriteLine("Database not found. Initializing schema...");
                var schemaFilePath = Path.Combine(_contentRootPath, "Data", "schema.sql");
                if (File.Exists(schemaFilePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));

                    var sql = File.ReadAllText(schemaFilePath);

                    using var command = _dbConnection.CreateCommand();
                    command.CommandText = sql;

                    // Open connection, execute schema creation, and close connection
                    _dbConnection.Open();
                    command.ExecuteNonQuery();
                    _dbConnection.Close();

                    Console.WriteLine("Schema initialized successfully.");
                }
                else
                {
                    Console.WriteLine($"Schema file not found at: {schemaFilePath}");
                    throw new FileNotFoundException("Schema file is missing.");

                }
            }
        }
    }
}
            