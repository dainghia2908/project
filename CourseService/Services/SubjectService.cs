using CourseService.Data;
using CourseService.DTOs;
using CourseService.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml; // Thư viện đọc Excel

namespace CourseService.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly CourseDbContext _context;

        public SubjectService(CourseDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách môn học
        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        // 2. Tạo môn học mới
        public async Task<Subject> CreateSubjectAsync(CreateSubjectDto dto)
        {
            var newSubject = new Subject
            {
                Code = dto.Code,
                Name = dto.Name,
                Credits = dto.Credits, 
                Description = dto.Description,
                IsActive = true
            };

            _context.Subjects.Add(newSubject);
            await _context.SaveChangesAsync();
            return newSubject;
        }

        // 3. Import Môn học từ Excel 
        public async Task<int> ImportSubjectsFromExcelAsync(IFormFile file)
        {
            int count = 0;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    if (worksheet.Dimension == null) return 0;
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var code = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        var name = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                        var creditsText = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                        var description = worksheet.Cells[row, 4].Value?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name)) continue;

                        // Check trùng mã môn
                        bool exists = await _context.Subjects.AnyAsync(s => s.Code == code);
                        if (exists) continue;

                        int.TryParse(creditsText, out int credits);

                        var subject = new Subject
                        {
                            Code = code,
                            Name = name,
                            Credits = credits,
                            Description = description,
                            IsActive = true
                        };

                        _context.Subjects.Add(subject);
                        count++;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return count;
        }

        // 4. Lấy chi tiết môn học
        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        // 5. Cập nhật môn học 
        public async Task<bool> UpdateSubjectAsync(int id, CreateSubjectDto dto)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return false;

            subject.Code = dto.Code;
            subject.Name = dto.Name;
            subject.Credits = dto.Credits;
            subject.Description = dto.Description;
            
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            return true;
        }

        // 6. Xóa môn học
        public async Task<bool> DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return false;

            // Kiểm tra ràng buộc: Nếu môn học đã có Lớp thì không được xóa
            bool hasClasses = await _context.Classes.AnyAsync(c => c.SubjectId == id);
            if (hasClasses)
            {
                throw new InvalidOperationException("Không thể xóa môn học này vì đã có lớp học đang hoạt động.");
            }

            // Xóa Syllabus đi kèm (nếu có)
            var syllabi = await _context.Syllabi.Where(s => s.SubjectId == id).ToListAsync();
            if (syllabi.Any())
            {
                _context.Syllabi.RemoveRange(syllabi);
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}