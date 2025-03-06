using System.Security.Cryptography;
using System.Text;
using MA.Data;
using MA.Data.Entities;
using MA.DTOs;
using MA.Service.Absract;
using Microsoft.EntityFrameworkCore;

namespace MA.Service.MovieService;

public class UserService : IUserService
{
    private readonly MADBContext _context;

    public UserService(MADBContext context)
    {
        _context = context;
    }
    
    
    public async Task<bool> RegisterAsync(UserRegisterDto userDto)
    {
        // Kullanıcı adı kontrolü
        if (await _context.users.AnyAsync(u => u.Username == userDto.Username))
        {
            return false; // Kullanıcı adı zaten alınmış
        }
        

        // Yeni kullanıcı oluştur
        var user = new User
        {
            First_name = userDto.First_name,
            Last_name = userDto.Last_name,
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = HashPassword(userDto.Password),
        };

        _context.users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> LoginAsync(UserLoginDto userDto)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
        if (user == null || !VerifyPassword(userDto.Password, user.PasswordHash))
        {
            return null; // Geçersiz giriş
        }

        // Başarılı giriş, token oluşturabilirsin (JWT vb.)
        return "Giris Basarılı"; // JWT Token döndürebilirsin
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.users.ToListAsync();
    
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            First_name = u.First_name,
            Last_name = u.Last_name,
            Email = u.Email,
            Username = u.Username,
            Password = u.PasswordHash,
            
        }).ToList();    }
    
}

