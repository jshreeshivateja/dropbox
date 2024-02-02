using System.Text;

// Using Local File Storage to store files.
namespace Dropbox
{
    public static class FileStorage
    {
        private static string fileBasePath = "C:\\Users\\sjuluri.FAREAST\\Desktop";

        // GetFile method to read content from the file
        public static byte[] GetFile(string fileId)
        {
            try
            {
                string filePath = GetFilePath(fileId);
                if (File.Exists(filePath))
                {
                    string fileData = File.ReadAllText(filePath);
                    return Encoding.ASCII.GetBytes(fileData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                throw;
            }
            return null;
        }

        // CreateFile method to write content to the file
        public static void CreateorUpdateFile(string fileId, byte[] content)
        {
            try
            {
                string filePath = GetFilePath(fileId);
                File.WriteAllText(filePath, Encoding.ASCII.GetString(content));
                Console.WriteLine("File written successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
                throw;
            }
        }

        public static void DeleteFile(string fileId)
        {
            try
            {
                string filePath = GetFilePath(fileId);
                File.Delete(filePath);
                Console.WriteLine("File deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                throw;
            }
        }

        private static string GetFilePath(string fileId)
        {
            return fileBasePath + fileId;
        }
    }

}