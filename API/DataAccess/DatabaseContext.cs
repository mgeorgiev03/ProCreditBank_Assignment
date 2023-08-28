using DataAccess.Entities;
using System.Data;
using System.Data.SQLite;

namespace DataAccess
{
    public class DatabaseContext 
    {
        string connectionString = @"Data Source=Assignment.db;Version=3;";
        bool isDbCreated = false;

        public async Task CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "CREATE TABLE IF NOT EXISTS MESSAGES (MessageId INTEGER AUTO_INCREMENT PRIMARY KEY, strMessage TEXT)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }

        }

        public async Task InsertMessage(MT799 message)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                if(!isDbCreated)
                {
                    await CreateTable();
                    isDbCreated = true;
                }

                string query = "INSERT INTO MESSAGES (MessageId, strMessage) VALUES (@Value1, @Value2)";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", message.Id);
                    cmd.Parameters.AddWithValue("@Value2", message.Message);
                    await cmd.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }

        public async Task<MT799> GetMessage(int id)
        {
            MT799 messageFromDb = new MT799();
            messageFromDb.Id = id;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "Select strMessage From Messages Where MessageId = @Value1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", id);

                    using (SQLiteDataReader reader = (SQLiteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            messageFromDb.Message = reader.GetString(0);
                        }

                        await reader.CloseAsync();
                    }
                }

                await connection.CloseAsync();
            }

            return messageFromDb;
        }

        public async Task<ICollection<MT799>> GetMessages()
        {
            ICollection<MT799> messages = new List<MT799>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "Select * From Messages";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = (SQLiteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                            messages.Add(new MT799(reader.GetInt32(0), reader.GetString(1)));

                        reader.Close();
                    }
                }

                await connection.CloseAsync();
            }

            return messages;
        }

        public async Task<int> GetCount()
        {
            int count = 0;
            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {

                await connection.OpenAsync();

                if (!isDbCreated)
                {
                    await CreateTable();
                    isDbCreated = true;
                }

                string query = "SELECT COUNT(DISTINCT MessageId) FROM Messages;";
                using(SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {

                    using(SQLiteDataReader reader = (SQLiteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            count = reader.GetInt32(0);
                        }

                        await reader.CloseAsync();
                    }
                }

                await connection.CloseAsync();
            }

            return count;
        }

        public async Task UpdateMessage(MT799 message)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "Update Messages Set strMessage = @Value2 Where MessageId = @Value1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", message.Id);
                    cmd.Parameters.AddWithValue("@Value2", message.Message);
                    await cmd.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }

        public async Task DeleteMessage(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "Delete From Messages Where MessageId = @Value1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", id);
                    await cmd.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }
    }
}
