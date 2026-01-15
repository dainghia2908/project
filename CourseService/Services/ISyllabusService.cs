using CourseService.DTOs;
using CourseService.Models;
using Microsoft.AspNetCore.Http;


namespace CourseService.Services
{
    public interface ISyllabusService
    {
        Task<Syllabus> CreateSyllabusAsync(CreateSyllabusDto dto);
        Task<List<Syllabus>> GetSyllabusesBySubjectAsync(int subjectId);
        Task<bool> DeleteSyllabusAsync(int id);
        Task<int> ImportSyllabusFromExcelAsync(IFormFile file);
    }
}