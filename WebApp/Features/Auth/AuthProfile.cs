using AutoMapper;
using WebApp.Data.Models;

namespace WebApp.Features.Auth;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterDto, UserEntity>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
