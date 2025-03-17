using System.Security.AccessControl;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace MasterCore8.Helpers
{
    public interface IFileService
    {
        string Upload(IFormFile file, string pathFolder="uploads/", string newname="", int limit=20, Dictionary<string,string> allowtype=null);
        string Base64SaveImage(string base64, string pathFolder = "uploads/", string newname = "", string extension = "png");
        string GetRootPath();
    }

    
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        private string FileServicePath;

        public FileService(IWebHostEnvironment env,IConfiguration configuration)
        {
            this.env = env;
            this.configuration = configuration;
            this.FileServicePath = string.IsNullOrEmpty(configuration["FileServicePath"])  
                ? Path.Combine(env.WebRootPath,"fileservice/") 
                : Path.Combine(configuration["FileServicePath"]);
        }

        public string Upload(IFormFile file, string pathFolder="uploads/", string newname="",int limit=20, Dictionary<string,string> allowtype=null)
        {
            try
            {
                // https://en.wikipedia.org/wiki/Media_type
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
                if(allowtype == null){
                    allowtype = new Dictionary<string, string>();
                    allowtype.Add(".pdf","application/pdf");
                    allowtype.Add(".doc","application/msword");
                    allowtype.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    allowtype.Add(".xls", "application/vnd.ms-excel");
                    allowtype.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    allowtype.Add(".ppt", "application/vnd.ms-powerpoint");
                    allowtype.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
                    allowtype.Add(".zip", "application/zip");
                    allowtype.Add(".jpg", "image/jpeg");
                    allowtype.Add(".png", "image/png");
                    allowtype.Add(".gif", "image/gif");
                    allowtype.Add(".mp4", "audio/mp4");
                }              

                if(file.Length > limit*1024*1024){
                    // size มากกว่า Limit (MB)
                    throw new ArgumentNullException("",$"FileUpload of maximum {limit}MB");

                }else if(!allowtype.ContainsValue(file.ContentType)){
                    // not allow tye file upload
                    throw new ArgumentNullException("",$"Allow Upload Type {string.Join(", ",allowtype.Keys)}");
                }

                var uploadDirecotroy = pathFolder;      
                var uploadPath = Path.Combine(FileServicePath, uploadDirecotroy);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                if(!string.IsNullOrEmpty(newname)){
                    fileName = newname + Path.GetExtension(file.FileName);
                }
                var filePath = Path.Combine(uploadPath, fileName);

                using (var strem = File.Create(filePath))
                {
                    file.CopyTo(strem);
                }
                if(File.Exists(filePath)){
                    Console.WriteLine(filePath);
                    return fileName;
                }
                return null;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public string Base64SaveImage(string base64, string pathFolder = "uploads/", string newname = "", string extension = "png")
        {
            try
            {       
                var uploadDirecotroy = pathFolder;      
                var uploadPath = Path.Combine(FileServicePath, uploadDirecotroy);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                
                var fileName = Guid.NewGuid() + "." + extension;
                if(!string.IsNullOrEmpty(newname)){
                    fileName = newname + "." + extension;
                }
                var filePath = Path.Combine(uploadPath, fileName);

                var _base64 = base64.Substring(base64.IndexOf(',') + 1).Trim('\0');

                byte[] imageBytes = Convert.FromBase64String(_base64);

                File.WriteAllBytes(filePath, imageBytes);

                if(File.Exists(filePath)){
                    Console.WriteLine(filePath);
                    return fileName;
                }
                return null;
            }
            catch (System.Exception)
            {                
                throw;
            }
        }

        public string GetRootPath()
        {
            try
            {
                return FileServicePath;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }

}