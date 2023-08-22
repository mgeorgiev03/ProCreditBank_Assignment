using DataAccess.Entities;
using System.Data.SQLite;

namespace DataAccess
{
    public class DatabaseContext 
    {
        string connectionString = @"Data Source=Assignment.db;Version=3;";

        public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "CREATE TABLE MESSAGES (MessageId INTEGER PRIMARY KEY, strMessage TEXT)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

        }

        public void InsertMessage(MT799 message)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO MESSAGES (MessageId, strMessage) VALUES (@Value1, @Value2)";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", message.Id);
                    cmd.Parameters.AddWithValue("@Value2", message.Message);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public MT799 GetMessage(int id)
        {
            MT799 messageFromDb = new MT799();
            messageFromDb.Id = id;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "Select strMessage From Messages Where MessageId = @Value1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messageFromDb.Message = reader.GetString(0);
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }

            return messageFromDb;
        }

        public ICollection<MT799> GetMessages()
        {
            ICollection<MT799> messages = new List<MT799>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "Select * From Messages";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            messages.Add(new MT799(reader.GetInt32(0), reader.GetString(1)));

                        reader.Close();
                    }
                }

                connection.Close();
            }

            return messages;
        }

        public void UpdateMessage(MT799 message)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "Update Messages Set strMessage = @Value2 Where MessageId = @Value1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", message.Id);
                    cmd.Parameters.AddWithValue("@Value2", message.Message);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void DeleteMessage(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "Delete From Messages Where MessageId = @Value1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Value1", id);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
