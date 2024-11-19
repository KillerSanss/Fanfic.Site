using Application.Dto.UserDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для User
/// </summary>
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, GetByIdUserResponse>();
        CreateMap<User, GetByTelegramIdUserResponse>();
        CreateMap<User, GetByNickNameUserResponse>();
        CreateMap<User, RegistrationResponse>();
        CreateMap<User, UpdateUserResponse>();
        CreateMap<UpdateSettingsUserRequest, UpdateSettingsUserResponse>();
        
        CreateMap<RegistrationRequest, User>()
            .ConstructUsing(dto => new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                Password = dto.Password,
                BirthDate = dto.BirthDate,
                NickName = dto.NickName,
                Gender = dto.Gender
            });
        
        CreateMap<UpdateUserRequest, User>()
            .ConstructUsing(dto => new User
            {
                Id = dto.Id,
                Email = dto.Email,
                Password = dto.Password,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                NickName = dto.NickName
            });
        
        CreateMap<LoginRequest, User>()
            .ConstructUsing(dto => new User
            {
                Email = dto.Email,
                Password = dto.Password
            });
    }
}