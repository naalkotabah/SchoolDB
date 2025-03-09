using CREDAJAX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CREDAJAX.Controllers
{
    public class StudintController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public StudintController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var persons = _context.Studints.AsNoTracking().Select(S => new{


                    S.Id,
                    S.FullName,
                    S.NameFather,
                    S.NameMother,
                    S.Age,
                    S.Class,

                }); // جلب البيانات دون تتبع لتحسين الأداء


                return Ok(persons); // إرجاع البيانات بصيغة JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "حدث خطأ في الخادم", Error = ex.Message }); // تحسين رسالة الخطأ
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                var student = _context.Studints.AsNoTracking().FirstOrDefault(s => s.Id == id);

                if (student == null)
                {
                    return NotFound($"لم يتم العثور على طالب بالمعرف {id}.");
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "حدث خطأ في الخادم", Error = ex.Message });
            }
        }

        [Authorize]
        public IActionResult Savestudint([FromBody] studint student)
        {
            try
            {
                if (student == null)
                    return BadRequest("البيانات غير صحيحة!");
                var student3 = new studint
                {
                    FullName = student.FullName,
                    NameFather = student.NameFather,
                    NameMother = student.NameMother,
                    Age = student.Age, // تأكد من أن العمر صحيح
                    Gender = student.Gender,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    PhoneNumberFather = student.PhoneNumberFather,
                    Address = student.Address,
                    Grade = student.Grade,
                    Class =   student.Class,
                    SchoolName = student.SchoolName,
                    StudentNumber = student.StudentNumber,
                    Skills = student.Skills,
                    Hobbies = student.Hobbies,
                    HealthStatus = student.HealthStatus,
                    Notes = student.Notes,
                    Behavior = student.Behavior,
                    JobStudin = student.JobStudin,
                    educationallevel = student.educationallevel
                };

                _context.Studints.Add(student3);
                _context.SaveChanges();

                return Ok(new { success = true, message = "تمت إضافة الطالب بنجاح!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء إضافة الطالب", error = ex.Message });
            }
        }




        [Authorize]
        public IActionResult Edit([FromBody] studint student)
        {
            try
            {
                if (student == null || student.Id == 0)
                    return BadRequest("البيانات غير صحيحة!");

                var existingStudent = _context.Studints.Find(student.Id);
                if (existingStudent == null)
                    return NotFound("لم يتم العثور على الطالب!");

                // تحديث جميع الحقول بنفس طريقة الإضافة
                existingStudent.FullName = student.FullName;
                existingStudent.NameFather = student.NameFather;
                existingStudent.NameMother = student.NameMother;
                existingStudent.Age = student.Age;
                existingStudent.Gender = student.Gender;
                existingStudent.Email = student.Email;
                existingStudent.PhoneNumber = student.PhoneNumber;
                existingStudent.PhoneNumberFather = student.PhoneNumberFather;
                existingStudent.Address = student.Address;
                existingStudent.Grade = student.Grade;
                existingStudent.Class = student.Class;
                existingStudent.SchoolName = student.SchoolName;
                existingStudent.StudentNumber = student.StudentNumber;
                existingStudent.Skills = student.Skills;
                existingStudent.Hobbies = student.Hobbies;
                existingStudent.Behavior = student.Behavior;
                existingStudent.educationallevel = student.educationallevel;
                existingStudent.JobStudin = student.JobStudin;
                existingStudent.HealthStatus = student.HealthStatus;

                existingStudent.Notes = student.Notes;

                _context.SaveChanges();

                return Ok(new { success = true, message = "تم تعديل بيانات الطالب بنجاح!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء تعديل البيانات", error = ex.Message });
            }
        }

        // حذف موظف
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var student = _context.Studints.Find(id);
                if (student == null)
                    return NotFound("لم يتم العثور على الطالب!");

                _context.Studints.Remove(student);
                _context.SaveChanges();

                return Ok(new { success = true, message = "تم حذف الطالب بنجاح!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء حذف الطالب", error = ex.Message });
            }
        }



    }
}
