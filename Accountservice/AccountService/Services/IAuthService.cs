using AccountService.DTOs;
using Microsoft.AspNetCore.Http;

namespace AccountService.Services
{
    public interface IAuthService
    {
        string Ping();
        LoginResponseDto Login(LoginRequestDto request);
        UserProfileDto GetProfile(Guid userId);
        void CreateAccount(string creatorRole, CreateAccountRequestDto request);
        IEnumerable<UserProfileDto> GetAccounts(string roleFilter);
        void DeactivateAccount(Guid accountId);
        void ReactivateAccount(Guid accountId);
        ImportResultDto ImportAccounts(string creatorRole, IFormFile file);
        string ForgotPassword(ForgotPasswordRequestDto request);
        void ResetPassword(ResetPasswordRequestDto request);
    }
}