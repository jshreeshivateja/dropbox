using System.Data.SQLite;

// Using SQLite Database to store metadata.
namespace Dropbox
{
    public class Database
    {
        private string connectionString = "myConnectionString";

        public Database(string connectionString = null)
        {
            this.connectionString = connectionString;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create table if not exists in the constructor
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Files (
                    FileId TEXT PRIMARY KEY,
                    FileName TEXT,
                    CreatedAt DATETIME,
                    SizeInBytes INTEGER,
                    FileType TEXT
                )";
                using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public DropboxFileMetadata GetFileMetadata(string fileId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string getEntryByFileIdQuery = "SELECT * FROM Files WHERE FileId = @fileId";
                    using (SQLiteCommand getEntryByFileIdCommand = new SQLiteCommand(getEntryByFileIdQuery, connection))
                    {
                        getEntryByFileIdCommand.Parameters.AddWithValue("@fileId", fileId);

                        using (SQLiteDataReader reader = getEntryByFileIdCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DropboxFileMetadata fileMetadata = new DropboxFileMetadata()
                                {
                                    FileName = (string)reader["FileName"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    SizeInBytes = (long)reader["SizeInBytes"],
                                    FileType = (string)reader["FileType"]
                                };
                                return fileMetadata;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateFileMetadata(string fileId, DropboxFileMetadata fileMetadata)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertOrUpdateQuery = @"
                    INSERT OR REPLACE INTO Files (FileId, FileName, CreatedAt, SizeInBytes, FileType)
                    VALUES (@fileId, @fileName, @createdAt, @sizeInBytes, @fileType)";

                using (SQLiteCommand command = new SQLiteCommand(insertOrUpdateQuery, connection))
                {
                    command.Parameters.AddWithValue("@fileId", fileId);
                    command.Parameters.AddWithValue("@fileName", fileMetadata.FileName);
                    command.Parameters.AddWithValue("@createdAt", fileMetadata.CreatedAt);
                    command.Parameters.AddWithValue("@sizeInBytes", fileMetadata.SizeInBytes);
                    command.Parameters.AddWithValue("@fileType", fileMetadata.FileType);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<DropboxFileMetadata> ListFiles()
        {
            List<DropboxFileMetadata> result = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string getAllEntriesQuery = "SELECT * FROM Files";
                using (SQLiteCommand command = new SQLiteCommand(getAllEntriesQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DropboxFileMetadata fileMetadata = new DropboxFileMetadata()
                            {
                                FileName = (string)reader["FileName"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                SizeInBytes = (long)reader["SizeInBytes"],
                                FileType = (string)reader["FileType"]
                            };
                            result.Add(fileMetadata);
                        }
                        return result;
                    }
                }
            }
        }
    }    
}

