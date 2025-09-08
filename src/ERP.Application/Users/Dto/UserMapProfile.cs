using ERP.Authorization.Users;
using AutoMapper;
using ERP.Sessions.Dto;

namespace ERP.Users.Dto;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<UserDto, User>()
            .ForMember(x => x.Roles, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore());

        CreateMap<CreateUserDto, User>();
        CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

        // Add mapping for UserDto to UserLoginInfoDto
        CreateMap<UserDto, UserLoginInfoDto>();
    }
}
