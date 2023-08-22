using DataAccess.Entities;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace DataAccess
{
    public class DatabaseContext 
    {
        string connectionString = @"Data Source=APIDatabase.db;Version=3;"; 

        public void CreateConnecton()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO MyTable (ColumnName) VALUES (@Value)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", "SomeValue");
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

        }
    }
}
