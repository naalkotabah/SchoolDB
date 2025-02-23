using CREDAJAX.Models;
using CREDAJAX.Models.dto;
using CREDAJAX.Serves;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.AccessControl;
using System.Security.Claims;

namespace CREDAJAX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly BackUp _backupService;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, BackUp backupService)
        {
            _logger = logger;
            _context = context;
            _backupService = backupService;
        }
        [Authorize]
        public IActionResult Index()
        {
           
            return View();
        }



        public async Task<IActionResult> Loginview([FromBody] formLogin user)
        {
            if (user == null || string.IsNullOrEmpty(user.name) || string.IsNullOrEmpty(user.password))
            {
                return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
            }

            var existingUser = _context.Users.FirstOrDefault(U => U.name == user.name && U.password == user.password);

            if (existingUser == null)
            {
                return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
            }

            // إنشاء القيم الخاصة بالمستخدم والدور
            var role = _context.Roles.FirstOrDefault(r => r.Id == existingUser.RoleId);
            if (role != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, existingUser.name),
            new Claim(ClaimTypes.Role, role.Name)
        };

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                // تعيين المستخدم في الجلسة أو ملفات تعريف الارتباط
                await HttpContext.SignInAsync(principal);
            }

            // إنشاء النسخة الاحتياطية وإرسال البريد الإلكتروني
            //await _backupService.CreateBackupAndSendEmailAsync();

            // إعادة التوجيه إلى الصفحة الرئيسية بعد تسجيل الدخول بنجاح
            return RedirectToAction("Index");
        }



        public IActionResult Login()
        {
          
            return View();
        }
    }
}
