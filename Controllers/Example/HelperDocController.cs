using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MasterCore8.Controllers.Example
{
    public class HelperDocController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {            
            return View();
        }

        [HttpGet]
        public IActionResult ApiAutocomplete(){
            var data = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                data.Add("dataTest"+i);
            }
            return Json(data);
        }
    }
}