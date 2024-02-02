using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dropbox.Controllers
{
    [ApiController]
    public class DropboxFileController : ControllerBase
    {
        [HttpPost("/files/upload")]
        public ActionResult<string> UploadFile()
        {
            try
            {
                var fileId = Guid.NewGuid().ToString();

                DropboxFile file = GetDropboxFile(Request.Body);
                DropboxFileMetadata metadata = file.Metadata;
                metadata.FileName = file.FileName;

                FileStorage.CreateorUpdateFile(fileId, file.Data);

                Database db = new Database();
                db.UpdateFileMetadata(fileId, metadata);

                return Ok(fileId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("/files/{fileId}")]
        public ActionResult<byte[]> ReadFile(string fileId)
        {
            try
            {
                byte[] data = FileStorage.GetFile(fileId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("/files/{fileId}")]
        public ActionResult UpdateFile(string fileId)
        {
            try
            {
                // Assuming fileContent is passed in body.
                DropboxFile file = GetDropboxFile(Request.Body);

                if (file.Data != null)
                {
                    FileStorage.CreateorUpdateFile(fileId, file.Data);
                }

                if (file.Metadata != null)
                {
                    Database db = new Database();
                    db.UpdateFileMetadata(fileId, file.Metadata);
                }
                return Ok("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("/files/{fileId}")]
        public ActionResult DeleteFile(string fileId)
        {
            try
            {
                FileStorage.DeleteFile(fileId);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("/files")]
        public ActionResult<List<DropboxFileMetadata>> ListFiles()
        {
            try
            {
                Database db = new Database();
                return Ok(db.ListFiles());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal Server Error");
            }
        }

        private DropboxFile GetDropboxFile(Stream body)
        {
            try
            {
                string strContent;
                using (StreamReader reader = new StreamReader(body))
                {
                    strContent = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<DropboxFile>(strContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
