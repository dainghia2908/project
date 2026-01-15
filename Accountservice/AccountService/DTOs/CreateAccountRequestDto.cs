namespace AccountService.DTOs;

public class CreateAccountRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}
