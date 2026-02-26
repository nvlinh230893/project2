using Elect.Data.EF.Models;

namespace WebApp.Data.Models;

public class UserEntity : Entity
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
