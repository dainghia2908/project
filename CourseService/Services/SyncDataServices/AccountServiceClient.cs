using System.Text.Json;
using CourseService.DTOs;

namespace CourseService.Services.Sync
{
    public class AccountServiceClient : IAccountServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AccountServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<UserInfoDto?> GetStudentByCodeAsync(string studentCode)
        {
            // Lấy đường dẫn API của AccountService từ appsettings.json
            var accountServiceUrl = _configuration["AccountServiceUrl"]; 
            
            // Gọi API: GET http://localhost:5001/api/users/code/SV001
            var response = await _httpClient.GetAsync($"{accountServiceUrl}/api/users/code/{studentCode}");

            if (!response.IsSuccessStatusCode)
            {
                return null; // Không tìm thấy hoặc lỗi
            }

            var content = await response.Content.ReadAsStringAsync();
            
            // Cấu hình để parse JSON không phân biệt hoa thường
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            return JsonSerializer.Deserialize<UserInfoDto>(content, options);
        }

        public async Task<UserInfoDto?> GetUserByEmailAsync(string email)
        {
            var accountServiceUrl = _configuration["AccountServiceUrl"];
            // Giả sử Account Service có API tìm theo email: /api/users/email/{email}
            var response = await _httpClient.GetAsync($"{accountServiceUrl}/api/users/email/{email}");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<UserInfoDto>(content, options);
        }
    
    }
}