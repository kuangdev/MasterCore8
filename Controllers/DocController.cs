using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterCore8.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterCore8.Controllers
{
    public class DocController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {            
            return View(new DocumentModel(){ TestString="MANANA",TestString2="babaraga" });
        }
    }
}