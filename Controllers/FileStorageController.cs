using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MyFileStorage.Controllers
{
    [ApiController]
    [Route("[controller]/path/to")]
    public class FileStorageController : ControllerBase
    {

        private readonly ILogger<FileStorageController> _logger;

        public FileStorageController(ILogger<FileStorageController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        [HttpGet("{*filename}")]
        public ActionResult GetProcessing(string filename)
        {
            if (filename == null || filename.Trim() == "")
            {
                try
                {
                    if (Directory.GetFiles(FileManipulate.ROOT).Length == 0 && Directory.GetDirectories(FileManipulate.ROOT).Length == 0)
                    {
                        return Ok("Директория пустая");
                    }
                    else
                    {
                        try
                        {
                            return new JsonResult(FileManipulate.GetAllFiles(FileManipulate.ROOT));
                        }
                        catch
                        {
                            return BadRequest("Ошибка просмотра директории");
                        }
                    }
                }
                catch
                {
                    return BadRequest("Ошибка просмотра директории, вероятно она удалена");
                }
            }
            else
            {
                string path = FileManipulate.ROOT + "\\" + filename;
                if (Directory.Exists(path))
                {
                    if (Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0)
                    {
                        return Ok("Директория пустая");
                    }
                    else
                    {
                        try
                        {
                            return new JsonResult(FileManipulate.GetAllFiles(path));
                        }
                        catch
                        {
                            return BadRequest("Ошибка просмотра директории");
                        }
                    }

                }
                else if (System.IO.File.Exists(path))
                {
                    try
                    {
                        FileStream fileStream = new FileStream(path, FileMode.Open);
                        return File(fileStream, "application/unknown", filename);
                    }
                    catch
                    {
                        return BadRequest("Не удалось отправить файл");
                    }
                }
                else
                {
                    return NotFound("Данный файл или директория не существует");
                }
            }
        }
        
        [HttpHead("{*filename}")]
        public ActionResult HeadProcessing(string filename)
        {
            try
            {
                var path = FileManipulate.ROOT + "\\" + filename;
                if (System.IO.File.Exists(path))
                {
                    var fileInfo = new FileInfo(path);

                    Response.Clear();
                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                    Response.Headers.ContentLength = fileInfo.Length;
                    Response.Headers.Add("File-Extension", fileInfo.Extension);
                    Response.Headers.Add("Creation-Time", fileInfo.CreationTime.ToString("dd.MM.yyy HH:mm:ss"));
                    Response.Headers.Add("Last-Write-Time", fileInfo.LastWriteTime.ToString("dd.MM.yyy HH:mm:ss"));
                    return Ok();
                }
                else
                {
                    return NotFound("Данный файл не найден");
                }
            }
            catch
            { 
                return BadRequest("Некорректный запрос");
            }
        }
        

        [HttpDelete("{*filename}")]
        public ActionResult DeleteProcessing(string filename)
        {
            try
            {
                var path = FileManipulate.ROOT + "\\" + filename;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return Ok("файл " + filename + " успешно удален!");

                }
                else if (Directory.Exists(path))
                {
                    if (filename != "" && filename != null)
                    {
                        Directory.Delete(path, true);
                        return Ok("Директория " + filename + " и все содержащиеся в ней файлы успешно удалены!");
                    }
                    else
                    {
                        return BadRequest("Нельзя удалять корневой каталог");
                    }
                }
                else
                {
                    return BadRequest("Некорректный запрос!");
                }
            }
            catch
            {
                return BadRequest("Некорректный запрос!");
            }
        }

       
    }
}
