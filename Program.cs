using MasterCore8.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(option=>
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options=>{
        options.Cookie.Name = "_auth_"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddSingleton<MasterCore8.Helpers.IFileService,MasterCore8.Helpers.FileService>();

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


string FileServicePath = string.IsNullOrEmpty(builder.Configuration["FileServicePath"]) 
    ? Path.Combine(builder.Environment.WebRootPath,"fileservice/") 
    : Path.Combine(builder.Configuration["FileServicePath"]);
    
// using Microsoft.Extensions.FileProviders;
// using System.IO;
// ======= Seting File Upload in Temp Folder ===========
if (!Directory.Exists(FileServicePath)){
    Directory.CreateDirectory(FileServicePath);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        FileServicePath),
    RequestPath = "/fileservice"
});
// ======= Seting File Upload in Temp Folder ===========

RotativaConfiguration.Setup(builder.Environment.WebRootPath, "Rotativa");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
