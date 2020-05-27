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
        public static List<string> GetAllFiles(string path)
        {
            try
            {
                var directories = Directory.GetDirectories(path);
                var files = Directory.GetFiles(path);
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

        public static bool InsertFile(string filename, IFormFile file)
        {
            try
            {
                var fileStream = new FileStream(FileManipulate.ROOT + "\\" + filename, FileMode.Create);
                file.CopyTo(fileStream);
                fileStream.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CopyFile(string oldpath, string newpath)
        {
            try
            {
                FileStream fromCopyStream, toCopyStream;
                using (fromCopyStream = new FileStream(ROOT + "\\" + oldpath, FileMode.Open))
                {
                    using (toCopyStream = new FileStream(ROOT + "\\" + newpath, FileMode.Create))
                    {
                        fromCopyStream.CopyTo(toCopyStream);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    
        public static string InsertFiles(IFormFileCollection files, string filename)
        {
            string result = "";
            foreach(var file in files)
            {
                try
                {
                    var fileStream = new FileStream(FileManipulate.ROOT + "\\" + filename + file.FileName, FileMode.Create);
                    file.CopyTo(fileStream);
                    fileStream.Dispose();
                    result += "Файл " + file.FileName + " успешно добавлен!\n";
                }
                catch
                {
                    result += "Файл " + file.FileName + "не удалось добавить.\n";
                }
            }
            return result;
        }


    }
}
