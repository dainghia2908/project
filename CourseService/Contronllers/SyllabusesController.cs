using CourseService.DTOs;
using CourseService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusesController : ControllerBase
    {
        private readonly ISyllabusService _syllabusService;

        public SyllabusesController(ISyllabusService syllabusService)
        {
            _syllabusService = syllabusService;
        }

        // 1. Tạo giáo trình mới
        [HttpPost]
        public async Task<IActionResult> CreateSyllabus([FromBody] CreateSyllabusDto dto)
        {
            try
            {
                var result = await _syllabusService.CreateSyllabusAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2. Lấy danh sách giáo trình theo môn học
        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetBySubject(int subjectId)
        {
            var list = await _syllabusService.GetSyllabusesBySubjectAsync(subjectId);
            return Ok(list);
        }

        // 3. Xóa giáo trình
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSyllabus(int id)
        {
            var result = await _syllabusService.DeleteSyllabusAsync(id);
            if (result) return Ok(new { message = "Xóa giáo trình thành công" });
            return NotFound(new { message = "Không tìm thấy giáo trình" });
        }

        // 4. Import Excel (Mới)
        [HttpPost("import")]
        public async Task<IActionResult> ImportSyllabus(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File không hợp lệ" });

            // Kiểm tra đuôi file
            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { message = "Chỉ hỗ trợ file Excel (.xlsx)" });

            try
            {
                int count = await _syllabusService.ImportSyllabusFromExcelAsync(file);
                return Ok(new { message = $"Đã import thành công {count} giáo trình." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}