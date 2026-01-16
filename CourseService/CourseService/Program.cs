using CourseService.Data;
using CourseService.Services;
using Microsoft.EntityFrameworkCore;
using CourseService.Middlewares;
using System.Text.Json.Serialization; 
using CourseService.Services.Sync;


// 1. Cấu hình EPPlus License Context
Environment.SetEnvironmentVariable("EPPlusLicenseContext", "NonCommercial");
// -----------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// 2. Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 3. Kết nối Database
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Đăng ký Services
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IClassService, ClassService>();

// // --- ĐĂNG KÝ HTTP CLIENT ---
builder.Services.AddHttpClient<IAccountServiceClient, AccountServiceClient>();

// 5. --- FIX LỖI MẤT TÊN MÔN HỌC (JSON LOOP) ---
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// ---------------------------------------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký SyllabusService
builder.Services.AddScoped<ISyllabusService, SyllabusService>();

// 1. Lấy URL từ appsettings
var accountServiceUrl = builder.Configuration["ServiceUrls:AccountService"];

// 2. Đăng ký HttpClient
builder.Services.AddHttpClient<IAccountServiceClient, AccountServiceClient>(client =>
{
    client.BaseAddress = new Uri(accountServiceUrl);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // Kích hoạt CORS

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>(); // Middleware bắt lỗi

app.MapControllers();

app.Run();