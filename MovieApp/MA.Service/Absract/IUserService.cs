using MA.DTOs;

namespace MA.Service.Absract;

public interface IUserService
{
    Task<bool> RegisterAsync(UserRegisterDto userDto);
    Task<string?> LoginAsync(UserLoginDto userLoginDto);
    Task<List<UserDto>> GetAllUsersAsync();

}