namespace TaskManager.WebApi.Dtos;

public class UserRegistrationDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}
