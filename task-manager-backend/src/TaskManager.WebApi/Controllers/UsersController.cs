using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;
using TaskManager.Infrastructure.Services;
using TaskManager.WebApi.Dtos;

namespace TaskManager.WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly AuthService _authService;

    public UsersController(IUserRepository userRepo, AuthService authService)
    {
        _userRepo = userRepo;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistrationDto dto)
    {
        if (await _userRepo.GetByUsernameAsync(dto.Username) != null)
            return BadRequest("Username already exists");

        if (await _userRepo.GetByEmailAsync(dto.Email) != null)
            return BadRequest("Email already exists");

        _authService.CreatePasswordHash(dto.Password, out var hash, out var salt);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepo.AddAsync(user);
        return Ok(new { token = _authService.GenerateJWT(user) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var user = await _userRepo.GetByUsernameAsync(dto.Username);
        if (user == null) return Unauthorized("Invalid credentials");

        if (!_authService.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized("Invalid credentials");

        return Ok(new { token = _authService.GenerateJWT(user) });
    }
}
