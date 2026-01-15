using CourseService.Data;
using CourseService.DTOs;
using CourseService.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;


namespace CourseService.Services
{
    public class SyllabusService : ISyllabusService
    {
        private readonly CourseDbContext _context;

        public SyllabusService(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<Syllabus> CreateSyllabusAsync(CreateSyllabusDto dto)
        {
            // Kiểm tra môn học có tồn tại không
            var subjectExists = await _context.Subjects.AnyAsync(s => s.Id == dto.SubjectId);
            if (!subjectExists) throw new Exception("Môn học không tồn tại");

            var syllabus = new Syllabus
            {
                Name = dto.Name,
                Description = dto.Description,
                SubjectId = dto.SubjectId,
                AssignmentWeight = dto.AssignmentWeight,
                ExamWeight = dto.ExamWeight,
                PassGrade = dto.PassGrade,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Syllabi.Add(syllabus);
            await _context.SaveChangesAsync();
            return syllabus;
        }

        public async Task<List<Syllabus>> GetSyllabusesBySubjectAsync(int subjectId)
        {
            return await _context.Syllabi
                .Include(s => s.Contents)
                .Where(s => s.SubjectId == subjectId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteSyllabusAsync(int id)
        {
            var item = await _context.Syllabi.FindAsync(id);
            if (item == null) return false;

            _context.Syllabi.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ImportSyllabusFromExcelAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
           
            int count = 0;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    // Duyệt từ dòng 2
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            // 1. ĐỌC DỮ LIỆU (Cột 1 bây giờ là MÃ MÔN thay vì ID)
                            var subjectCode = worksheet.Cells[row, 1].Value?.ToString()?.Trim(); // Ví dụ: "IT001"
                            var name = worksheet.Cells[row, 2].Value?.ToString();
                            var description = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                            var assignWeightStr = worksheet.Cells[row, 4].Value?.ToString() ?? "30";
                            var examWeightStr = worksheet.Cells[row, 5].Value?.ToString() ?? "70";
                            var passGradeStr = worksheet.Cells[row, 6].Value?.ToString() ?? "5.0";

                            if (string.IsNullOrEmpty(subjectCode) || string.IsNullOrEmpty(name)) continue;

                            // 2. TÌM MÔN HỌC DỰA TRÊN MÃ (Subject Code)
                            // (Lưu ý: Phải dùng .FirstOrDefaultAsync để lấy ra đối tượng môn học)
                            var subject = await _context.Subjects
                                .FirstOrDefaultAsync(s => s.Code == subjectCode);

                            // Nếu không tìm thấy mã môn này trong DB thì bỏ qua
                            if (subject == null) continue; 

                            // 3. TẠO SYLLABUS (Lấy ID thật từ môn học vừa tìm được)
                            var newSyllabus = new Syllabus
                            {
                                SubjectId = subject.Id, // <-- Quan trọng: Gán ID lấy từ DB
                                Name = name,
                                Description = description,
                                AssignmentWeight = int.Parse(assignWeightStr),
                                ExamWeight = int.Parse(examWeightStr),
                                PassGrade = double.Parse(passGradeStr),
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow
                            };

                            _context.Syllabi.Add(newSyllabus);
                            count++;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return count;
        }
    }
}