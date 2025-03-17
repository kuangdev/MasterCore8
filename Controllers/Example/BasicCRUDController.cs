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
    public class BasicCRUDController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileservice;

        public BasicCRUDController(ApplicationDbContext context, IFileService fileservice)
        {
            _context = context;
            _fileservice = fileservice;
        }
        // ============= Page View =================================================================  
        [HttpGet]
        public async Task<IActionResult> Index()
        {   
            ViewData["lookup_category_list"] = await _context.tblookup.Where(l=>l.lookup_type == LookupTypeList.BasicCRUDCategory).ToListAsync();
            return View("Index");
        }
        
        [HttpGet]
        public async Task<ActionResult> TableBasicCRUD(string search_key="",string monthyear="",string category="", int? page=1)
        {
            int pageSize = 10;
            int maxPages = 5;
            search_key = search_key?.ToLower().Trim();

            var userid = User.FindFirst("id").Value;
            var myDept = User.FindFirst("dept").Value;

            var data = _context.tbbasic_crud
                .Include(a=>a.create)
                .Include(i=>i.lookup_category)
                .Where(a=>a.title.Contains(search_key ?? "")
                    || a.category.Contains(search_key ?? "")
                    || a.movie_code.Contains(search_key ?? "")
                    || a.create_by.Contains(search_key ?? "")
                )
                .Where(a=> a.category.Contains(category ?? ""))
                .Where(a=> string.IsNullOrEmpty(monthyear) || EF.Functions.DateDiffMonth(a.create_date, Convert.ToDateTime(monthyear)) == 0)
                .OrderBy(a=>a.create_date);

            return View(await PaginatedList<tbbasic_crud>.CreateAsync(data, page ?? 1, pageSize, maxPages));
        }

        [HttpGet]
        public async Task<ActionResult> ExportData(string search_key="",string monthyear="",string category="", string export="")
        {
            search_key = search_key?.ToLower().Trim();
            var userid = User.FindFirst("id").Value;
            var myDept = User.FindFirst("dept").Value;

            var data = await _context.tbbasic_crud
                .Include(a=>a.create)
                .Where(a=>a.title.Contains(search_key ?? "")
                    || a.category.Contains(search_key ?? "")
                    || a.movie_code.Contains(search_key ?? "")
                    || a.create_by.Contains(search_key ?? "")
                )
                .Where(a=> a.category.Contains(category ?? ""))
                .Where(a=> string.IsNullOrEmpty(monthyear) || EF.Functions.DateDiffMonth(a.create_date, Convert.ToDateTime(monthyear)) == 0)
                .OrderBy(a=>a.create_date)
                .ToListAsync();

            if(export == "PDF"){

                string footer = "--footer-center \"Printed on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                return new ViewAsPdf("ExportData", data){
                    FileName = $"BasicCRUD_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
                    ContentDisposition =ContentDisposition.Inline,
                    CustomSwitches = footer
                };

            }else if(export == "EXCEL"){

                var time = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $"BasicCRUD_{time}.xls";
                Response.Headers.Add("content-disposition", $"attachment;filename={fileName}");
                Response.Headers.Add("Content-Type", "application/vnd.ms-excel");
                return View("ExportData", data);  

            }
            return View(data);
        }
        
        // =========================================================================================
        // ============= Form =====================================================================

        [HttpGet]
        public async Task<IActionResult> FormCreate()
        {
            ViewData["lookup_category_list"] = await _context.tblookup.Where(l=>l.lookup_type == LookupTypeList.BasicCRUDCategory).ToListAsync();
            return View("FormData", new tbbasic_crud());
        }

        [HttpGet]
        public async Task<IActionResult> FormEdit(string id="")
        {
            ViewData["lookup_category_list"] = await _context.tblookup.Where(l=>l.lookup_type == LookupTypeList.BasicCRUDCategory).ToListAsync();
            var data = await _context.tbbasic_crud.FirstOrDefaultAsync(d=>d.id == id);
            return View("FormData", data);
        }
        
        [HttpGet]
        public async Task<IActionResult> FormShow(string id)
        {
            ViewData["lookup_category_list"] = await _context.tblookup.Where(l=>l.lookup_type == LookupTypeList.BasicCRUDCategory).ToListAsync();            
            var data = await _context.tbbasic_crud
                .Include(d=>d.create)
                .Include(d=>d.update)
                .FirstOrDefaultAsync(d=>d.id == id);
            return View("FormShow",data);
        }

        // =========================================================================================

        // ============= Action ====================================================================
        [HttpPost]
        public async Task<IActionResult> Store(tbbasic_crud input, IFormFile fileupload=null)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var add  = new tbbasic_crud();
                add.movie_code = input.movie_code;
                add.title = input.title;
                add.category = input.category;
                add.detail = input.detail;
                if(fileupload != null){
                    string newname = "Movie_"+ DateTime.Now.ToString("yyyyMMddhhmmss");
                    add.img = _fileservice.Upload(fileupload, "fileUploads/", newname);
                }
                add.publish_start_date = input.publish_start_date;
                add.publish_end_date = input.publish_end_date;
                add.create_by = User.FindFirst("id").Value;
                add.create_date = DateTime.Now;
                await _context.tbbasic_crud.AddAsync(add);
                await _context.SaveChangesAsync();

                // gen code run no.
                await tbbasic_crud.GenDocCode(_context, add.id, true);

                transaction.Commit(); 

                TempData["success"] = "Add Data Success";
                return Redirect(HttpContext.Request.Headers["Referer"]);;
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                TempData["error"] = "Add Data Failed";
                return Redirect(HttpContext.Request.Headers["Referer"]);;
                throw;
            }
            
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(tbbasic_crud input, IFormFile fileupload = null)
        {
            try
            {
                var data = await _context.tbbasic_crud.FirstOrDefaultAsync(b=>b.id == input.id);
                if(data  != null){
                    data.movie_code = input.movie_code;
                    data.title = input.title;
                    data.category = input.category;
                    data.detail = input.detail;
                    if(fileupload != null) {
                        string newname = "Movie_"+data.id+"_"+ DateTime.Now.ToString("yyyyMMddhhmmss");
                        data.img = _fileservice.Upload(fileupload, "fileUploads/", newname);
                    }
                    data.publish_start_date = input.publish_start_date;
                    data.publish_end_date = input.publish_end_date;
                    data.update_by = User.FindFirst("id").Value;
                    data.update_date = DateTime.Now;
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Update Data Success";
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }
                TempData["error"] = "Not Found Data!";
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }
            catch (System.Exception)
            {
                TempData["error"] = "Update Data Failed";
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
                var data = await _context.tbbasic_crud.FirstOrDefaultAsync(e=>e.id == id && e.create_by == userid);
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