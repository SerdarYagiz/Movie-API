namespace MA.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string First_name { get; set; }
    public string Last_name { get; set; }
    public string Email { get; set; }
}