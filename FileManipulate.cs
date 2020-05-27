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

namespace MyFileStorage
{
    public static class FileManipulate
    {
        public const string ROOT = "D:\\MyFileStorage";
        public static List<string> GetAllFiles(string Path)
        {
            try
            {
                var directories = Directory.GetDirectories(Path);
                var files = Directory.GetFiles(Path);
                var result = new List<string>();
                result.AddRange(directories);
                result.AddRange(files);
                result.ForEach(str => str.Replace("D:\\MyFileStorage\\", ""));
                for (int i = 0; i <= result.Count - 1; i++)
                {
                    result[i] = result[i].Replace(ROOT + "\\", "");
                }
                return result;
            }
            catch
            {
                throw new Exception("Не удалось обработать директорию");
            }
        }
    }
}
