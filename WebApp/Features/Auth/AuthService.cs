using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Elect.Data.EF.Interfaces.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using WebApp.Common;
using WebApp.Data.Interfaces;
using WebApp.Data.Models;

namespace WebApp.Features.Auth;

public class AuthService : IAuthService
{
    private readonly IRepository<UserEntity> _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(
        IRepository<UserEntity> userRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var existingUser = _userRepo.GetSingle(x => x.Email == dto.Email);
        if (existingUser != null)
            return Result.Failure<AuthResponseDto>(ErrorCodes.Auth.EmailExists);

        var existingUsername = _userRepo.GetSingle(x => x.Username == dto.Username);
        if (existingUsername != null)
            return Result.Failure<AuthResponseDto>(ErrorCodes.Auth.UsernameExists);

        var user = _mapper.Map<UserEntity>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        _userRepo.Add(user);
        await _unitOfWork.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email
        };
    }

    public Result<AuthResponseDto> Login(LoginDto dto)
    {
        var user = _userRepo.GetSingle(x => x.Email == dto.Email);
        if (user == null)
            return Result.Failure<AuthResponseDto>(ErrorCodes.Auth.InvalidCredentials);

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Result.Failure<AuthResponseDto>(ErrorCodes.Auth.InvalidCredentials);

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email
        };
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationInMinutes"]!)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
