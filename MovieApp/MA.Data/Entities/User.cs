using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace MA.Data.Entities;

public class User : IBaseEntity
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string First_name { get; set; }
    [Required]
    public string Last_name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PasswordHash {
        get
        {
            return PasswordHash;
        }
        set
        {
            PasswordHash = HashPassword(value);
        } }

    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
    
    
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}