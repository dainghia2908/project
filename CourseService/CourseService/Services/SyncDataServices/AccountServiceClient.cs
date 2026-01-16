using System.Text.Json;
using CourseService.DTOs;

namespace CourseService.Services.Sync
{
    public class AccountServiceClient : IAccountServiceClient
    {
        private readonly HttpClient _httpClient;

        public AccountServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Không cần IConfiguration ở đây nữa vì BaseAddress đã được set ở Program.cs
        }

        public async Task<UserInfoDto?> GetStudentByCodeAsync(string studentCode)
        {
            // Gọi API: /api/users/code/{studentCode}
            // (HttpClient sẽ tự nối với BaseAddress https://localhost:7001)
            var response = await _httpClient.GetAsync($"/api/users/code/{studentCode}");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            return JsonSerializer.Deserialize<UserInfoDto>(content, options);
        }

        public async Task<UserInfoDto?> GetUserByEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync($"/api/users/email/{email}");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            return JsonSerializer.Deserialize<UserInfoDto>(content, options);
        }
    }
}