using Elect.Data.EF.Models;

namespace WebApp.Data.Models;

public class UserFBEntity : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Pass { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string UserIp { get; set; } = string.Empty;
    public string Authen { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longtitude { get; set; }
}
