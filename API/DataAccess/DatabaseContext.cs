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

                string query = "CREATE TABLE IF NOT EXISTS MESSAGES (MessageId INTEGER AUTO_INCREMENT PRIMARY KEY, block1 TEXT, block2 TEXT, block3 TEXT)";
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

                string query = "INSERT INTO MESSAGES (MessageId, block1, block2, block3) VALUES (@Value1, @Value2, @Value3, @Value4)";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", message.Id);
                    cmd.Parameters.AddWithValue("@Value2", message.Block1);
                    cmd.Parameters.AddWithValue("@Value3", message.Block2);
                    cmd.Parameters.AddWithValue("@Value4", message.Block3);
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
                            messageFromDb.Block1 = reader.GetString(0);
                            messageFromDb.Block2 = reader.GetString(1);
                            messageFromDb.Block3 = reader.GetString(2);
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
                            messages.Add(new MT799(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));

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

                string query = "Update Messages Set block1 = @Value2, block2 = @Value3, block3 = @Value4 Where MessageId = @Value1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", message.Id);
                    cmd.Parameters.AddWithValue("@Value2", message.Block1);
                    cmd.Parameters.AddWithValue("@Value3", message.Block2);
                    cmd.Parameters.AddWithValue("@Value4", message.Block3);
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
