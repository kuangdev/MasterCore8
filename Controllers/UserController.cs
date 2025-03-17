using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MasterCore8.Data;
using MasterCore8.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;

namespace MasterCore8.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Index()
        {
            try
            {
                var data = await _context.AppUser.Where(a=>a.status == "active")
                    .OrderBy(a=>a.create_date)
                    .AsNoTracking()
                    .ToListAsync();
                return View(data);
            }
            catch (System.Exception)
            {
                TempData["error"] = "Error Code 500!";
                return View();
            }
            
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            ViewData["formMode"] = "create";
            return View("Form", new AppUser());
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Store(AppUser user)
        {
            try
            {
                var checkuser = await _context.AppUser.AsNoTracking().FirstOrDefaultAsync(u=>u.id == user.empno);
                if(checkuser != null){
                    if(checkuser.status == "deleted"){
                        // duppicate user deleted changer id and remove olddata 
                        checkuser.id = checkuser.id+"_delete"+(checkuser.delete_date ?? DateTime.Now).ToString("yyyyMMddHHmmss"); 
                        var copydata = new AppUser();
                        copydata = checkuser;
                        await _context.AppUser.AddAsync(copydata);
                        var removedata = await _context.AppUser.FirstOrDefaultAsync(u=>u.id == user.empno && u.status == "deleted");
                        _context.Remove(removedata);
                        await _context.SaveChangesAsync();
                    }else{
                        return Json(NotFound("User นี้มีข้อมูลอยู่แล้ว!"));
                    }
                }

                var data = new AppUser();
                data.id = user.empno;
                data.empno = user.empno;
                data.password = user.password;
                data.name = user.name;
                data.email = user.email;
                data.ext = user.ext;
                data.dept = user.dept_full.Substring(0,3);
                data.dept_full = user.dept_full;
                data.div = user.div;
                data.position = user.position;
                data.status = "active";
                data.roles = user.roles;
                data.create_date = DateTime.Now;
                data.create_by = User.FindFirst("id").Value;

                _context.AppUser.Add(data);
                await _context.SaveChangesAsync();

                // return Json(Ok(new { message= "เพิ่มข้อมูลสำเร็จ!", modalclose = "modal-create"}));
                TempData["success"] = "เพิ่มข้อมูลสำเร็จ!";
                return RedirectToAction("Index", "User");
            }
            catch (System.Exception)
            {
                // return Json(BadRequest("เกิดข้อผิดพลาด"));
                TempData["error"] = "เกิดข้อผิดพลาด!";
                return RedirectToAction("Index", "User");
                //throw;
            }
            
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var data = await _context.AppUser.AsNoTracking().FirstOrDefaultAsync(e=>e.id == id);
                if(data != null){
                    ViewData["formMode"] = "edit";
                    return View("Form",data);
                }
                
                return NotFound();
            }
            catch (System.Exception)
            {
                
                return BadRequest();
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Update(AppUser user,string id)
        {
            try
            {
                var data = await _context.AppUser.FirstOrDefaultAsync(e=>e.id == id);
                if(data != null){
                    if(!string.IsNullOrEmpty(user.password)) data.password = user.password;
                    data.ext = user.ext;
                    data.roles = user.roles;
                    
                    await _context.SaveChangesAsync();

                    // return Json(Ok("Saved"));
                    TempData["success"] = "แก้ไขข้อมูลสำเร็จ!";
                    return RedirectToAction("Index", "User");
                }
                
                return NotFound();
            }
            catch (System.Exception)
            {
                
                // return BadRequest();
                TempData["error"] = "เกิดข้อผิดพลาด!";
                return RedirectToAction("Index", "User");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Delete(AppUser user)
        {
            try
            {
                var data = await _context.AppUser.FirstAsync(u=>u.id == user.id && u.id != "016623");
                if(data != null){
                    data.status = "deleted";
                    data.delete_by = User.FindFirst("id").Value;
                    data.delete_date = DateTime.Now;

                    //_context.Remove(data);
                    await _context.SaveChangesAsync();
                    // return Json(Ok("Deleted"));
                    TempData["success"] = "ลบข้อมูลสำเร็จ!";
                    return RedirectToAction("Index", "User");
                }
                // return Json(BadRequest(user));
                TempData["error"] = "ไม่พบข้อมูล!";
                return RedirectToAction("Index", "User");
            }
            catch (System.Exception)
            {
                // return Json(BadRequest("เกิดข้อผิดพลาด!"));
                TempData["error"] = "เกิดข้อผิดพลาด!";
                return RedirectToAction("Index", "User");
                throw;
            }
        }

        public async Task<IActionResult> ApiGetUser(string empno)
        {
            try
            {
                var data = await _context.AppUser.FirstOrDefaultAsync(a=>a.empno == empno && a.status == "active");
                if(data != null){
                    return Json(Ok(data));
                }
                return Json(NotFound("NoData"));
            }
            catch (System.Exception)
            {
                return Json(BadRequest("NoData"));
                throw;
            }
        }        
    }
}