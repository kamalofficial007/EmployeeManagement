using System.Data;

namespace UserManagement.Data
{
    public class SchemaInitializer
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _dbFilePath;
        public SchemaInitializer(IDbConnection dbConnection, IHostEnvironment environment)
        {
            // Extract the SQLite database file path from the connection string
            _dbConnection = dbConnection;
            // Extract the database file path from the connection string
            var connectionString = _dbConnection.ConnectionString;
            var startIndex = connectionString.IndexOf("Data Source=", StringComparison.OrdinalIgnoreCase) + "Data Source=".Length;
            _dbFilePath = connectionString.Substring(startIndex).Trim(';');
            Console.WriteLine($"Database File Path: {_dbFilePath}"); // Debugging line
        }

        public void Initialize()
        {
            Console.WriteLine("Database not found. Initializing schema...");
            if (!File.Exists(_dbFilePath))
            {
                File.Create(_dbFilePath).Close();
            }
            var sql = "CREATE TABLE IF NOT EXISTS Roles(" +
"Id INTEGER PRIMARY KEY AUTOINCREMENT," +
"Name TEXT NOT NULL," +
"Description TEXT);";
            sql += " CREATE TABLE IF NOT EXISTS Users(" +
                 "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                 "Username TEXT NOT NULL," +
                 "Email TEXT NOT NULL," +
                 "PasswordHash TEXT NOT NULL," +
                 "RoleId INTEGER NOT NULL," +
                 "FOREIGN KEY (RoleId) REFERENCES Roles (Id))";

            using var command = _dbConnection.CreateCommand();
            command.CommandText = sql;

            // Open connection, execute schema creation, and close connection
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();

            Console.WriteLine("Schema initialized successfully.");

        }
    }
}


