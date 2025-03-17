using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MasterCore8.Models;
using Microsoft.AspNetCore.Authorization;
using MasterCore8.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MasterCore8.Models.ViewModels;
using MasterCore8.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Drawing;
using System.Drawing.Imaging;

namespace MasterCore8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileservice;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IFileService fileservice,IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _fileservice = fileservice;
            _env = env;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userid = User.FindFirst("id").Value;
            var myDept = User.FindFirst("dept").Value;            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet("UserImg/{id?}")]
        public async Task<IActionResult> GetUserIMG(string id="")
        {
            var default_path = Path.Combine(_env.WebRootPath,"images/","avatar.png");
            if(string.IsNullOrEmpty(id)){
                var stream = System.IO.File.OpenRead(default_path);
                return File(stream, "image/png");
            }            
            try {
                HttpClient client = new HttpClient();
                string URL = $"URL";
                var stream = await client.GetStreamAsync(URL);
                Image myimg = Helper.ScaleImage(Bitmap.FromStream(stream),300,300);
                MemoryStream ms = new MemoryStream();
                myimg.Save(ms,ImageFormat.Jpeg);                
                Response.Headers.Add("Cache-Control","private,max-age=604800");
                return File(ms.ToArray(),"image/jpeg");
            }
            catch (Exception) {
                var stream = System.IO.File.OpenRead(default_path);
                return File(stream, "image/png");
            }          
        }

        [HttpPost]
        public ActionResult CKUpload(string CKEditor = "",string CKEditorFuncNum=null, IFormFile upload=null, string ckCsrfToken=""){
            string filepath="";
            string url = "";
            string message = "Not Found File";
            var output = "";
            try
            {
                if(upload != null){
                    string newname = "Ckupload_"+ DateTime.Now.ToString("yyyyMMddhhmmss");
                    filepath = _fileservice.Upload(upload, "ckuploads/", newname);
                    url = Url.AbsoluteContent($"~/fileservice/ckuploads/{filepath}");
                    message = "Upload Success!";
                }

                if(string.IsNullOrEmpty(CKEditorFuncNum)){
                    return Content(@$"{{
                            ""uploaded"": 1,
                            ""fileName"": ""{filepath}"",
                            ""url"": ""{url}"",
                            ""error"": {{
                                ""message"": """"
                            }}
                        }}", "text/html");
                }
                
                output = string.Format("<script>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}');</script>", 
                    CKEditorFuncNum, 
                    url, 
                    message);
                return Content(output,"text/html");
                                
            }
            catch (System.Exception err)
            {
                if(string.IsNullOrEmpty(CKEditorFuncNum)){
                    return Content(@$"{{
                            ""uploaded"": 0,
                            ""fileName"": """",
                            ""url"": """",
                            ""error"": {{
                                ""message"": ""{err.Message}""
                            }}
                        }}", "text/html");
                }                
                output = string.Format("<script>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}');</script>", 
                    CKEditorFuncNum, 
                    null, 
                    err.Message);
                return Content(output,"text/html");
                throw;
            }
        }
    }
}
