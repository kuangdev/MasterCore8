using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterCore8.Data;
using MasterCore8.Models;
using MasterCore8.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Microsoft.AspNetCore.Authorization;

namespace MasterCore8.Controllers.Example
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileservice;

        public StudentController(ApplicationDbContext context, IFileService fileservice)
        {
            _context = context;
            _fileservice = fileservice;
        }
        // ============= Page View =================================================================  
        [HttpGet]
        public async Task<IActionResult> Index(string search_key="",int? page = 1)
        {   
            int pageSize = 5;
            int maxPages = 5;
            search_key = search_key?.ToLower().Trim();

            var userid = User.FindFirst("id").Value;
            var myDept = User.FindFirst("dept").Value;

            var data = _context.tbstudent
                .Include(a=>a.create)
                .Where(a=>a.name.Contains(search_key ?? "")
                    || a.gender.Contains(search_key ?? "")
                    || a.address.Contains(search_key ?? "")
                    || a.create_by.Contains(search_key ?? "")
                )
                .OrderBy(a=>a.create_date);

            return View(await PaginatedList<tbstudent>.CreateAsync(data, page ?? 1, pageSize, maxPages));
        }

        [HttpGet]
        public async Task<ActionResult> ExportData(string search_key="" , string export="")
        {
            search_key = search_key?.ToLower().Trim();
            var userid = User.FindFirst("id").Value;
            var myDept = User.FindFirst("dept").Value;

            var data = await _context.tbstudent
                .Include(a=>a.create)
                .Where(a=>a.name.Contains(search_key ?? "")
                    || a.gender.Contains(search_key ?? "")
                    || a.address.Contains(search_key ?? "")
                    || a.create_by.Contains(search_key ?? "")
                )
                .OrderBy(a=>a.create_date)
                .ToListAsync();

            if(export == "PDF"){

                string footer = "--footer-center \"Printed on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                return new ViewAsPdf("ExportData", data){
                    FileName = $"Student_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
                    ContentDisposition =ContentDisposition.Inline,
                    CustomSwitches = footer
                };

            }else if(export == "EXCEL"){

                var time = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $"Student_{time}.xls";
                Response.Headers.Add("content-disposition", $"attachment;filename={fileName}");
                Response.Headers.Add("Content-Type", "application/vnd.ms-excel");
                return View("ExportData", data);  

            }
            return View(data);
        }
        
        // =========================================================================================
        // ============= Form =====================================================================

        [HttpGet]
        public IActionResult FormCreate()
        {
            return View(new tbstudent());
        }

        [HttpGet]
        public async Task<IActionResult> FormEdit(string id="")
        {
            var data = await _context.tbstudent.FirstOrDefaultAsync(d=>d.id == id);
            return View(data);
        }
        
        [HttpGet]
        public async Task<IActionResult> FormShow(string id)
        {
            var data = await _context.tbstudent
                .Include(d=>d.create)
                .Include(d=>d.update)
                .FirstOrDefaultAsync(d=>d.id == id);
            return View(data);
        }

        // =========================================================================================

        // ============= Action ====================================================================
        [HttpPost]
        public async Task<IActionResult> Store(tbstudent input, IFormFile fileupload=null,string signaturepad=null)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var add  = new tbstudent();
                add.name = input.name;
                add.address = input.address;
                add.gender = input.gender;
                add.class_room = input.class_room;
                if(fileupload != null){
                    string newname = "Student_"+ DateTime.Now.ToString("yyyyMMddhhmmss");
                    add.img = _fileservice.Upload(fileupload, "fileUploads/", newname);
                }                
                add.active_date = input.active_date;
                add.portfolio = input.portfolio;
                if(!string.IsNullOrEmpty(signaturepad)){                    
                    string newname = "Student_Signature_"+add.id+ DateTime.Now.ToString("yyyyMMddhhmmss");
                    add.signature = _fileservice.Base64SaveImage(signaturepad,"signatures/", newname);
                }
                
                add.create_by = User.FindFirst("id").Value;
                add.create_date = DateTime.Now;
                await _context.tbstudent.AddAsync(add);
                await _context.SaveChangesAsync();

                transaction.Commit(); 

                TempData["success"] = "Add Data Success";
                return Redirect(HttpContext.Request.Headers["Referer"]);;
            }
            catch (System.Exception err)
            {
                transaction.Rollback();
                TempData["error"] = $"Add Data Failed {err.Message}";
                return Redirect(HttpContext.Request.Headers["Referer"]);;
                throw;
            }
            
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(tbstudent input, IFormFile fileupload = null,string signaturepad=null)
        {
            try
            {
                var data = await _context.tbstudent.FirstOrDefaultAsync(b=>b.id == input.id);
                if(data  != null){
                    data.name = input.name;
                    data.address = input.address;
                    data.gender = input.gender;
                    data.class_room = input.class_room;
                    if(fileupload != null) {
                        string newname = "Student_"+data.id+"_"+ DateTime.Now.ToString("yyyyMMddhhmmss");
                        data.img = _fileservice.Upload(fileupload, "fileUploads/", newname);
                    }
                    data.active_date = input.active_date;
                    data.portfolio = input.portfolio;
                    if(!string.IsNullOrEmpty(signaturepad)){                    
                        string newname = "Student_Signature_"+data.id+ DateTime.Now.ToString("yyyyMMddhhmmss");
                        data.signature = _fileservice.Base64SaveImage(signaturepad,"signatures/", newname);
                    }
                    data.update_by = User.FindFirst("id").Value;
                    data.update_date = DateTime.Now;

                    await _context.SaveChangesAsync();

                    TempData["success"] = "Update Data Success";
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }
                TempData["error"] = "Not Found Data!";
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }
            catch (System.Exception err)
            {
                TempData["error"] = $"Update Data Failed {err.Message}";
                return Redirect(HttpContext.Request.Headers["Referer"]);
                throw;
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id="")
        {
            try
            {
                var userid = User.FindFirst("id").Value; 
                var data = await _context.tbstudent.FirstOrDefaultAsync(e=>e.id == id && e.create_by == userid);
                if(data != null){
                    _context.Remove(data);
                    await _context.SaveChangesAsync();
                    TempData["success"] = $"Delete Data Success";
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }
                TempData["error"] = $"Data Not Found Data or Status != WaitMgrup !";
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }
            catch (System.Exception)
            {
                TempData["error"] = $"Delete Data Error !";
                return Redirect(HttpContext.Request.Headers["Referer"]);
                throw;
            }
        }
        
        // =========================================================================================        
        // ============= API ====================================================================        
        // =========================================================================================
        // ============= Custom Handle Function =================================================
        
    }
}