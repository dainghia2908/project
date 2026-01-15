using CourseService.Data;
using CourseService.DTOs;
using CourseService.Models; 
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml; // Cần thiết cho Import Excel
using CourseService.Services.Sync;    
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CourseService.Services
{
    public class ClassService : IClassService
    {
        private readonly CourseDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IAccountServiceClient _accountClient;

        public ClassService(
            CourseDbContext context,
            IWebHostEnvironment env,
            IAccountServiceClient accountClient)
        {
            _context = context;
            _env = env;
            _accountClient = accountClient;
        }

        // ==========================================
        // PHẦN 1: QUẢN LÝ LỚP HỌC (CRUD)
        // ==========================================

        public async Task<List<ClassDto>> GetAllClassesAsync()
        {
            var classes = await _context.Classes
                .Include(c => c.Subject)
                // .Include(c => c.Lecturer) // Bỏ comment nếu model Class đã có quan hệ Lecturer
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            return classes.Select(c => new ClassDto 
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Semester = c.Semester,
                MaxStudents = c.MaxStudents,
                SubjectId = c.SubjectId,
                SubjectName = c.Subject != null ? c.Subject.Name : "Chưa có tên môn",
                SubjectCode = c.Subject != null ? c.Subject.Code : "",
                LecturerName = c.LecturerName, 
                LecturerEmail = c.LecturerEmail
            }).ToList();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.Subject)
                // .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Class> CreateClassAsync(CreateClassDto dto)
        {
            var subject = await _context.Subjects.FindAsync(dto.SubjectId);
            if (subject == null) throw new Exception("Môn học không tồn tại");

            var newClass = new Class
            {
                Code = dto.Code,
                Name = dto.Name,
                Semester = dto.Semester,
                SubjectId = dto.SubjectId,
                MaxStudents = dto.MaxStudents > 0 ? dto.MaxStudents : 60,
                Status = "Active"
            };
            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();
            return newClass;
        }

        public async Task<ClassDto> UpdateClassAsync(int id, CreateClassDto request) 
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) throw new Exception("Không tìm thấy lớp học");

            classEntity.Code = request.Code;
            classEntity.Name = request.Name;
            classEntity.Semester = request.Semester;
            classEntity.SubjectId = request.SubjectId;
            classEntity.MaxStudents = request.MaxStudents;

            await _context.SaveChangesAsync();
            
            return new ClassDto 
            { 
                Id = classEntity.Id, 
                Code = classEntity.Code, 
                Name = classEntity.Name,
                Semester = classEntity.Semester,
                SubjectId = classEntity.SubjectId,
                MaxStudents = classEntity.MaxStudents,
                SubjectName = classEntity.Subject?.Name ?? "", // Load lại nếu cần thiết
                LecturerName = classEntity.LecturerName
            };
        }

        public async Task DeleteClassAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) throw new Exception("Không tìm thấy lớp học");

            var hasStudents = await _context.ClassMembers.AnyAsync(m => m.ClassId == id);
            if (hasStudents) throw new Exception("Lớp đang có sinh viên, không thể xóa!");

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ImportClassesFromExcelAsync(IFormFile file)
        {
            int count = 0;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                // Cần cài NuGet: EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    // Giả sử file Excel có cột: Mã Lớp | Tên Lớp | Mã Môn | Học Kỳ | MaxSV
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try 
                        {
                            var classCode = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            var className = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                            var subjectCode = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                            var semester = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                            var maxStudentsStr = worksheet.Cells[row, 5].Value?.ToString();

                            if (string.IsNullOrEmpty(classCode) || string.IsNullOrEmpty(subjectCode)) continue;

                            // 1. Tìm Subject ID dựa vào Subject Code (Backend cần hỗ trợ hàm này)
                            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Code == subjectCode);
                            if (subject == null) continue; // Bỏ qua nếu môn không tồn tại

                            // 2. Tạo lớp
                            var newClass = new Class
                            {
                                Code = classCode,
                                Name = className ?? classCode, // Nếu không có tên thì lấy mã làm tên
                                SubjectId = subject.Id,
                                Semester = semester ?? "N/A",
                                MaxStudents = int.TryParse(maxStudentsStr, out int max) ? max : 60,
                                Status = "Active"
                            };

                            _context.Classes.Add(newClass);
                            count++;
                        }
                        catch
                        {
                            // Log lỗi nếu cần
                            continue;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return count;
        }

        // ==========================================
        // PHẦN 2: QUẢN LÝ TÀI LIỆU (RESOURCES)
        // ==========================================

        public async Task<List<ClassResource>> GetClassResourcesAsync(int classId)
        {
            return await _context.ClassResources.Where(r => r.ClassId == classId).ToListAsync();
        }

        public async Task UploadResourceAsync(int classId, IFormFile file, Guid uploaderId)
        {
            var classExists = await _context.Classes.AnyAsync(c => c.Id == classId);
            if (!classExists) throw new Exception("Lớp không tồn tại");

            string uploadsFolder = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var resource = new ClassResource
            {
                ClassId = classId,
                FileName = file.FileName,
                FilePath = uniqueFileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = uploaderId
            };

            _context.ClassResources.Add(resource);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteResourceAsync(int resourceId)
        {
            var res = await _context.ClassResources.FindAsync(resourceId);
            if (res == null) return false;

            string uploadsFolder = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "uploads");
            string filePath = Path.Combine(uploadsFolder, res.FilePath);
            
            if (File.Exists(filePath)) File.Delete(filePath);

            _context.ClassResources.Remove(res);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(Stream?, string, string)> DownloadResourceAsync(int resourceId)
        {
            var res = await _context.ClassResources.FindAsync(resourceId);
            if (res == null) return (null, "", "");

            string uploadsFolder = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "uploads");
            string filePath = Path.Combine(uploadsFolder, res.FilePath);

            if (!File.Exists(filePath)) return (null, "", "");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return (memory, res.FileName, res.ContentType);
        }

        // ==========================================
        // PHẦN 3: QUẢN LÝ SINH VIÊN (MEMBERS)
        // ==========================================

        public async Task<List<ClassMember>> GetClassMembersAsync(int classId)
        {
            return await _context.ClassMembers.Where(m => m.ClassId == classId).ToListAsync();
        }

        public async Task<bool> AddStudentToClassAsync(int classId, string studentCode)
        {
            // 1. Kiểm tra đã có trong lớp chưa
            bool exists = await _context.ClassMembers.AnyAsync(m => m.ClassId == classId && m.StudentCode == studentCode);
            if (exists) throw new Exception("Sinh viên này đã có trong lớp");

            // 2. GỌI SANG ACCOUNT SERVICE ĐỂ LẤY THÔNG TIN THẬT
            // Lưu ý: Nếu AccountService chưa chạy, đoạn này có thể gây lỗi. 
            // Nếu muốn test nhanh có thể comment đoạn này và dùng data giả.
            var studentInfo = await _accountClient.GetStudentByCodeAsync(studentCode);

            if (studentInfo == null) 
            {
                // Fallback nếu không có Account Service: Tạo data giả để test
                // throw new Exception($"Không tìm thấy sinh viên {studentCode}");
                studentInfo = new UserInfoDto { StudentCode = studentCode, FullName = "Sinh viên " + studentCode, Email = studentCode + "@test.com" };
            }

            // 3. Lưu thông tin vào Database
            var member = new ClassMember
            {
                ClassId = classId,
                StudentCode = studentCode,
                FullName = studentInfo.FullName, 
                Email = studentInfo.Email,       
                JoinedAt = DateTime.UtcNow
            };

            _context.ClassMembers.Add(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStudentFromClassAsync(int classId, int memberId)
        {
            var mem = await _context.ClassMembers.FindAsync(memberId);
            if (mem == null || mem.ClassId != classId) return false;

            _context.ClassMembers.Remove(mem);
            await _context.SaveChangesAsync();
            return true;
        }
    
        public async Task<int> ImportMembersFromExcelAsync(int classId, IFormFile file)
        {
            int count = 0;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var studentCode = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(studentCode)) continue;

                        try 
                        {
                            await AddStudentToClassAsync(classId, studentCode);
                            count++;
                        }
                        catch 
                        {
                            continue;
                        }
                    }
                }
            }
            return count;
        }

        // ==========================================
        // PHẦN 4: GIẢNG VIÊN
        // ==========================================

        public async Task<bool> AssignLecturerAsync(int classId, string lecturerEmail)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null) throw new Exception("Lớp không tồn tại");

            var lecturerInfo = await _accountClient.GetUserByEmailAsync(lecturerEmail);
            
            if (lecturerInfo == null) throw new Exception("Không tìm thấy giảng viên với email này");

            classEntity.LecturerId = lecturerInfo.Id;
            classEntity.LecturerName = lecturerInfo.FullName;
            classEntity.LecturerEmail = lecturerInfo.Email;

            _context.Classes.Update(classEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}