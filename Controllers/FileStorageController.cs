using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyFileStorage.Controllers
{
    [ApiController]
    [Route("[controller]/path/to")]
    public class FileStorageController : ControllerBase
    {
        private string Root { get; } = "D:\\MyFileStorage";

        private readonly ILogger<FileStorageController> _logger;

        public List<string> CollectDirectory(string Path)
        {
            var directories = Directory.GetDirectories(Path);
            var files = Directory.GetFiles(Path);
            var result = new List<string>();
            result.AddRange(directories);
            result.AddRange(files);
            result.ForEach(str => str.Replace("D:\\MyFileStorage\\", ""));
            for (int i = 0; i <= result.Count - 1; i++)
            {
                result[i] = result[i].Replace(Root+"\\", "");
            }
            return result;
        }

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
                if (Directory.GetFiles(Root).Length == 0 && Directory.GetDirectories(Root).Length == 0)
                {
                    return Ok("Директория пустая");
                }
                else
                {
                    return new JsonResult(CollectDirectory(Root));
                }
            }
            else
            {
                string path = Root + "\\" + filename;
                if (Directory.Exists(path))
                {
                    if (Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0)
                    {
                        return Ok("Директория пустая");
                    }
                    else
                    {
                        return new JsonResult(CollectDirectory(path));
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
                } else
                {
                    return BadRequest("Данный файл или директория не существует");
                }
            }
        }
        

    
    }
}
