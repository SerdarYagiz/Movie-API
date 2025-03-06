using MA.DTOs;
using MA.Service.Absract;
using MA.Service.MovieService;
using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Controller;

[Route("api/login")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
    {
        var result = await _userService.RegisterAsync(request);
        if (!result)
        {
            return BadRequest("Kullanıcı adı veya e-posta zaten mevcut.");
        }

        return Ok("Kullanıcı başarıyla oluşturuldu.");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto request)
    {
        var token = await _userService.LoginAsync(request);
        if (token == null)
        {
            return Unauthorized("Kullanıcı adı veya şifre hatalı.");
        }

        return Ok(token);
    }
    
    
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    
}