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
using OfficeOpenXml;

namespace MasterCore8.Controllers.Example
{
    [Authorize]
    public class AdvancedCRUDController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileservice;

        public AdvancedCRUDController(ApplicationDbContext context, IFileService fileservice)
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
        public async Task<ActionResult> TableAdvancedCRUD(string search_key="",string monthyear="",string category="", int? page=1)
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
                    FileName = $"AdvancedCRUD_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
                    ContentDisposition =ContentDisposition.Inline,
                    CustomSwitches = footer
                };

            }else if(export == "EXCEL"){

                var time = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $"AdvancedCRUD_{time}.xls";
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


        public async Task<ActionResult> TemplateImport()
        {
            var userid = User.FindFirst("id").Value;
            var myDept = User.FindFirst("dept").Value;

            var data = await _context.tbbasic_crud
                .Include(a=>a.create)                
                .OrderByDescending(a=>a.create_date)
                .Skip(0).Take(10)
                .ToListAsync(); 

            var time = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"TemplateImportAdvancedCRUD_{time}.xls";
            Response.Headers.Add("content-disposition", $"attachment;filename={fileName}");
            Response.Headers.Add("Content-Type", "application/vnd.ms-excel");
            return View(data);  
        }


        [HttpPost]
        public async Task<IActionResult> ImportData(IFormFile fileupload)
        { 
            var actionBy = User.FindFirst("id").Value;
            var actionDate = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();
            if(ModelState.IsValid)
            {
                if(fileupload?.Length > 0)
                {
                    // convert to a stream
                    var stream = fileupload.OpenReadStream();

                    try
                    {
                        using(var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;

                            // checkheadcells
                            if(!(worksheet.Cells[1, 1].Value?.ToString().Trim() ==  "movie_code"
                                && worksheet.Cells[1, 2].Value?.ToString().Trim() ==  "title"
                                && worksheet.Cells[1, 3].Value?.ToString().Trim() ==  "category"
                                && worksheet.Cells[1, 4].Value?.ToString().Trim() ==  "detail"
                                && worksheet.Cells[1, 5].Value?.ToString().Trim() ==  "publish_start_date"
                                && worksheet.Cells[1, 6].Value?.ToString().Trim() ==  "publish_end_date"
                                && worksheet.Cells[1, 7].Value?.ToString().Trim() ==  "create_by"
                                && worksheet.Cells[1, 8].Value?.ToString().Trim() ==  "create_date")){
                                await transaction.RollbackAsync();
                                TempData["error"] = $"Excel Template ไม่ถูกต้อง !";
                                return Redirect(HttpContext.Request.Headers["Referer"]); 
                            }

                            for(var row = 2; row <= rowCount; row++)
                            {
                                try
                                {
                                    var movie_code = worksheet.Cells[row, 1].Value?.ToString().Trim().NullIfWhiteSpace();
                                    var title = worksheet.Cells[row, 2].Value?.ToString().Trim().NullIfWhiteSpace();
                                    var category = worksheet.Cells[row, 3].Value?.ToString().Trim().NullIfWhiteSpace();
                                    var detail = worksheet.Cells[row, 4].Value?.ToString().Trim().NullIfWhiteSpace();
                                    var publish_start_date = worksheet.Cells[row, 5].Value?.ToString().Trim().NullIfWhiteSpace();
                                    var publish_end_date = worksheet.Cells[row, 6].Value?.ToString().Trim().NullIfWhiteSpace();
                                    var create_by = worksheet.Cells[row, 7].Value?.ToString().Trim().NullIfWhiteSpace();
                                    var create_date = worksheet.Cells[row, 8].Value?.ToString().Trim().NullIfWhiteSpace();

                                    var checkdup = _context.tbbasic_crud.Where(e => e.movie_code == movie_code).FirstOrDefault();

                                    if(!string.IsNullOrEmpty(title)){
                                        if (checkdup == null)
                                        {
                                            var add = new tbbasic_crud();
                                            add.movie_code = movie_code;
                                            add.title = title;
                                            add.category = category;
                                            add.detail = detail;
                                            add.publish_start_date = Convert.ToDateTime(publish_start_date);
                                            add.publish_end_date = Convert.ToDateTime(publish_end_date);
                                            add.create_by = create_by;
                                            add.create_date = Convert.ToDateTime(create_date);
                                            await _context.tbbasic_crud.AddAsync(add);
                                            await _context.SaveChangesAsync();
                                            if(string.IsNullOrEmpty(movie_code)){
                                                await tbbasic_crud.GenDocCode(_context, add.id, true);
                                            }
                                        }
                                        else
                                        {
                                            checkdup.title = title;
                                            checkdup.category = category;
                                            checkdup.detail = detail;
                                            checkdup.publish_start_date = Convert.ToDateTime(publish_start_date);
                                            checkdup.publish_end_date = Convert.ToDateTime(publish_end_date);
                                            checkdup.create_by = create_by;
                                            checkdup.create_date = Convert.ToDateTime(create_date);
                                            checkdup.update_by = actionBy;
                                            checkdup.update_date = actionDate;
                                            await _context.SaveChangesAsync();                                            
                                        }
                                    }
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    // throw;
                                    await transaction.RollbackAsync();
                                    TempData["error"] = $"Import Data ไม่สำเร็จ!";
                                    return Redirect(HttpContext.Request.Headers["Referer"]); 
                                }
                            }

                            // merge data table temp
                            try
                            {
                                // await _context.SaveChangesAsync();
                                await transaction.CommitAsync();
                                TempData["success"] = $"Import Data สำเร็จ!";
                                return Redirect(HttpContext.Request.Headers["Referer"]); 
                            }
                            catch (System.Exception)
                            {
                                await transaction.RollbackAsync();
                                throw;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await transaction.RollbackAsync();
                        // throw;
                        TempData["error"] = $"รูปแบบไฟล์ไม่ถูกต้อง 2!";
                        return Redirect(HttpContext.Request.Headers["Referer"]); 
                    }
                }
            }

            await transaction.RollbackAsync();
            TempData["error"] = $"เกิดข้อผิดพลาด ไม่สามารถอัพโหลดได้!";
            return Redirect(HttpContext.Request.Headers["Referer"]); 
        }
        
        // =========================================================================================        
        // ============= API ====================================================================        
        // =========================================================================================
        // ============= Custom Handle Function =================================================
        
    }
}