using WebApp.Common;

namespace WebApp.Features.Auth;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Result<AuthResponseDto> Login(LoginDto dto);
}
