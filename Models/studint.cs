using System.ComponentModel.DataAnnotations;

namespace CREDAJAX.Models
{
    public class studint
    {

        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? NameFather { get; set; }
        public string? NameMother { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }

        // معلومات الاتصال
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        
        public string? PhoneNumberFather { get; set; }
        public string? Address { get; set; }

        // المعلومات الأكاديمية
        public string? Grade { get; set; }
        public string? Class { get; set; }
        public string? SchoolName { get; set; }
        public string? StudentNumber { get; set; }

        public string? JobStudin { get; set; }
        public string? educationallevel { get; set; } // معدل الدرجات

        // معلومات إضافية
        public string? HealthStatus { get; set; }
        public string? Skills { get; set; }
        public string? Hobbies { get; set; } //هوايات 
        public string? Behavior { get; set; } //سلوك
        public string? Notes { get; set; }

    }
}
