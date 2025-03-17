using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterCore8.Data;
using MasterCore8.Models;
using Newtonsoft.Json;

namespace MasterCore8.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Denied()
        {
            if(Request.Headers["X-Requested-With"] == "XMLHttpRequest"){
                return Json(Forbid());
            }
            ViewData["ReturnUrl"] = HttpContext.Request.Headers["Referer"];
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            ReturnUrl ??= Url.Content("~/");
            if(User.Identity.IsAuthenticated){
                return Redirect(ReturnUrl);
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(new LoginInput());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInput Input, string ReturnUrl)
        {
            ReturnUrl ??= Url.Content("~/");
            AppUser data = await _context.AppUser.FirstOrDefaultAsync(u=>u.id == Input.username && u.status == "active");
            if(data != null){
                // มี user ใน AppUser

                if(data.password == Input.password){

                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, data.id));
                    claims.Add(new Claim(ClaimTypes.Name, data.name));
                    claims.Add(new Claim("id", data.id));
                    claims.Add(new Claim("empno", data.empno));
                    claims.Add(new Claim("name", data.name));
                    claims.Add(new Claim("email", data.email));
                    claims.Add(new Claim("ext", data.ext ?? ""));
                    claims.Add(new Claim("dept", data.dept));
                    claims.Add(new Claim("div", data.div));
                    claims.Add(new Claim("position", data.position));
                    claims.Add(new Claim("status", data.status));
                    claims.Add(new Claim("roles", data.roles));

                    foreach (var item in data.rolelist)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item));
                    }

                    var claimsIdentity = new ClaimsIdentity(
                        claims, 
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties{
                            IsPersistent = true
                        });                    
                    
                    TempData["success"] = "เข้าสู่ระบบสำเร็จ!";
                    return Redirect(ReturnUrl);
                }

                TempData["error"] = "Username หรือ Password ไม่ถูกต้อง!";
                return View(Input);
            }
            TempData["error"] = "Username ไม่มีในระบบ!";
            return View(Input);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Profile(AppUser Input)
        {
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordInput input)
        {
            string userid = User.FindFirst("id").Value;
            var data = await _context.AppUser.FirstOrDefaultAsync(a=>a.id == userid && a.password == input.password);

            if(data != null){
                if(input.newPassword != input.newPasswordConfirm){
                    TempData["error"] = "New Password NotMatch!";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                data.password = input.newPassword;
                await _context.SaveChangesAsync();
                TempData["success"] = "Change Password Success!";

            }else{
                TempData["error"] = "Change Password Fail!";
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
               
    }
}

public class LoginInput
{
    [Required]
    [MaxLength(50)]
    public string username { get; set; }
    [MaxLength(50)]
    public string password { get; set; }
}

public class ChangePasswordInput
{
    [Required]
    [MaxLength(50)]
    public string password { get; set; }
    [MaxLength(50)]
    public string newPassword { get; set; }
    [MaxLength(50)]
    public string newPasswordConfirm { get; set; }
}

public class UserHrModel
{
    public string Success { get; set;}
    public Collection<UserHrData> data { get; set; }
}
public class UserHrData {        
    public string OLD_EMP_NO { get; set;}
    public string EMP_NO { get; set;}
    public string SNAME_ENG { get; set;}
    public string GNAME_ENG { get; set;}
    public string FNAME_ENG { get; set;}
    public string SNAME_THA { get; set;}
    public string GNAME_THA { get; set;}
    public string FNAME_THA { get; set;}
    public string DIV_CLS { get; set;}
    public string DIV_NAME { get; set;}
    public string DIV_ABB_NAME { get; set;}
    public string DEPT_CODE { get; set;}
    public string DEPT_ABB_NAME { get; set;}
    public string DEPT_NAME { get; set;}
    public string WC_CODE { get; set;}
    public string WC_ABB_NAME { get; set;}
    public string WC_NAME { get; set;}
    public string RESN_DATE { get; set;}
    public string BAND { get; set;}
    public string POSN_CODE { get; set;}
    public string POSN_ENAME { get; set;}
    public string PROB_DATE { get; set;}
    public string EMAIL { get; set;}
    public string EMAIL_ACTIVE_DATE { get; set;}
}