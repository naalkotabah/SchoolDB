using CREDAJAX.Models;
using CREDAJAX.Serves;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ إضافة الخدمات إلى الحاوية
builder.Services.AddControllersWithViews();

// ✅ إضافة DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Mycon")));

// ✅ إضافة المصادقة باستخدام ملفات تعريف الارتباط
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

// ✅ إضافة سياسات التصريح
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// ✅ تسجيل خدمة النسخ الاحتياطي
builder.Services.AddSingleton<BackUp>();



// ✅ بناء التطبيق
var app = builder.Build();

// ✅ إعداد التعامل مع الاستثناءات وملفات الاستاتيك
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// ✅ إعداد التوجيه
app.UseRouting();

// ✅ تفعيل المصادقة والتصريح
app.UseAuthentication();
app.UseAuthorization();

// ✅ تعريف مسار Controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

// ✅ تشغيل التطبيق
app.Run();
