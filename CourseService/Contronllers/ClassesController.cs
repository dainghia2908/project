using CourseService.DTOs;
using CourseService.Services;
using Microsoft.AspNetCore.Mvc;
using CourseService.Wrappers; // Giả sử bạn có class wrapper response này
using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CourseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        // ==========================================
        // SECTION 1: QUẢN LÝ LỚP HỌC (CRUD)
        // ==========================================

        // 1. Lấy danh sách lớp học
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _classService.GetAllClassesAsync();
            // Trả về danh sách DTO
            return Ok(new ApiResponse<List<ClassDto>>(data, "Lấy danh sách thành công"));
        }

        // 2. Lấy chi tiết 1 lớp học
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _classService.GetClassByIdAsync(id);
            if (data == null) 
                return NotFound(new ApiResponse<string>(false, "Không tìm thấy lớp học"));
            
            return Ok(new ApiResponse<Class>(data, "Thành công"));
        }

        // 3. Tạo lớp học mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClassDto dto)
        {
            try
            {
                var newClass = await _classService.CreateClassAsync(dto);
                return Ok(new ApiResponse<Class>(newClass, "Tạo lớp thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        // 4. Cập nhật thông tin lớp học (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] CreateClassDto request)
        {
            try
            {
                var result = await _classService.UpdateClassAsync(id, request);
                return Ok(new ApiResponse<ClassDto>(result, "Cập nhật thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        // 5. Xóa lớp học (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                await _classService.DeleteClassAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa lớp học thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        // 6. Import danh sách lớp từ Excel
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0) 
                return BadRequest(new ApiResponse<string>(false, "File không hợp lệ"));

            try
            {
                int count = await _classService.ImportClassesFromExcelAsync(file);
                return Ok(new ApiResponse<int>(count, $"Import thành công {count} lớp."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        // ==========================================
        // SECTION 2: QUẢN LÝ TÀI LIỆU (RESOURCES)
        // ==========================================

        [HttpGet("{id}/resources")]
        public async Task<IActionResult> GetResources(int id)
        {
            var files = await _classService.GetClassResourcesAsync(id);
            return Ok(new ApiResponse<List<ClassResource>>(files, "Thành công"));
        }

        [HttpPost("{id}/resources")]
        public async Task<IActionResult> UploadResource(int id, IFormFile file)
        {
            if (file == null) return BadRequest("Vui lòng chọn file");
            try
            {
                // Giả lập ID người upload (nếu chưa có Auth thật)
                var fakeUserId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");
                
                await _classService.UploadResourceAsync(id, file, fakeUserId);
                return Ok(new ApiResponse<string>(true, "Upload thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        [HttpDelete("resources/{resourceId}")]
        public async Task<IActionResult> DeleteResource(int resourceId)
        {
            var result = await _classService.DeleteResourceAsync(resourceId);
            if (result) return Ok(new ApiResponse<string>(true, "Đã xóa file"));
            return BadRequest(new ApiResponse<string>(false, "Lỗi xóa file hoặc file không tồn tại"));
        }

        [HttpGet("resources/{resourceId}/download")]
        public async Task<IActionResult> DownloadResource(int resourceId)
        {
            var (stream, fileName, contentType) = await _classService.DownloadResourceAsync(resourceId);
            if (stream == null) return NotFound("File không tồn tại");
            
            return File(stream, contentType, fileName);
        }

        // ==========================================
        // SECTION 3: QUẢN LÝ SINH VIÊN (MEMBERS)
        // ==========================================

        [HttpGet("{id}/members")]
        public async Task<IActionResult> GetMembers(int id)
        {
            var members = await _classService.GetClassMembersAsync(id);
            return Ok(new ApiResponse<List<ClassMember>>(members, "Thành công"));
        }

        [HttpPost("{id}/members")]
        public async Task<IActionResult> AddMember(int id, [FromBody] AddStudentDto dto)
        {
            try
            {
                var result = await _classService.AddStudentToClassAsync(id, dto.StudentCode);
                return Ok(new ApiResponse<bool>(true, "Thêm sinh viên thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        [HttpDelete("{id}/members/{memberId}")]
        public async Task<IActionResult> RemoveMember(int id, int memberId)
        {
            var result = await _classService.RemoveStudentFromClassAsync(id, memberId);
            if (result) return Ok(new ApiResponse<bool>(true, "Đã xóa sinh viên khỏi lớp"));
            return NotFound(new ApiResponse<string>(false, "Không tìm thấy sinh viên"));
        }

        [HttpPost("{id}/members/import")]
        public async Task<IActionResult> ImportMembers(int id, IFormFile file)
        {
            if (file == null) return BadRequest("File không hợp lệ");
            
            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new ApiResponse<string>(false, "Chỉ hỗ trợ file Excel (.xlsx)"));

            try
            {
                int count = await _classService.ImportMembersFromExcelAsync(id, file);
                return Ok(new ApiResponse<int>(count, $"Đã thêm thành công {count} sinh viên vào lớp."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        // ==========================================
        // SECTION 4: QUẢN LÝ GIẢNG VIÊN
        // ==========================================

        [HttpPost("{id}/assign-lecturer")]
        public async Task<IActionResult> AssignLecturer(int id, [FromBody] AssignLecturerDto dto)
        {
            try
            {
                await _classService.AssignLecturerAsync(id, dto.Email);
                return Ok(new ApiResponse<bool>(true, "Đã cập nhật giảng viên cho lớp"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }
    }
}