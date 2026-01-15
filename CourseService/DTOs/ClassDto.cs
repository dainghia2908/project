using System;

namespace CourseService.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        
        // Thông tin môn học (để hiển thị tên thay vì số ID)
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;

        // Thông tin giảng viên (có thể null nếu chưa gán)
        public Guid? LecturerId { get; set; }
        public string? LecturerName { get; set; }
        public string? LecturerEmail { get; set; }
    }
}