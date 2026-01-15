using System.ComponentModel.DataAnnotations;

namespace CourseService.DTOs
{
    public class AddStudentDto
    {
        [Required(ErrorMessage = "Vui lòng nhập mã sinh viên")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Mã sinh viên không hợp lệ")]
        public string StudentCode { get; set; } = string.Empty;
    }
}