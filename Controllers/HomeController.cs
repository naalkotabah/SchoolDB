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

          
            //await _backupService.CreateBackupAndSendEmailAsync();

            
            return RedirectToAction("Index");
        }



        public IActionResult Login()
        {
          
            return View();
        }
        [HttpGet]
        public IActionResult Search(string searchValue)
        {
            try
            {
                var query = _context.Studints.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    string searchLower = searchValue.ToLower().Trim();

                    query = query.Where(s =>
                        (s.FullName != null && s.FullName.ToLower().Contains(searchLower)) || // البحث في الاسم
                        (s.Address != null && s.Address.ToLower().Contains(searchLower)) ||  // البحث في العنوان
                        (s.Class != null && s.Class.ToLower().Contains(searchLower))      // البحث في الفصل
                    );
                }

                var students = query.AsNoTracking()
                                    .Select(S => new
                                    {
                                        S.Id,
                                        S.FullName,
                                        S.NameFather,
                                        S.NameMother,
                                        S.Age,
                                        S.Class,
                                    })
                                    .ToList();

                if (students.Count == 0)
                {
                    return NotFound(new { Message = "لم يتم العثور على طالب مطابق للبحث" });
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "حدث خطأ في الخادم", Error = ex.Message });
            }
        }





    }
}
