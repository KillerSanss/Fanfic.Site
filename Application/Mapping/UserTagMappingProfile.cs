using Application.Dto.UserTagDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для UserTag
/// </summary>
public class UserTagMappingProfile : Profile
{
    public UserTagMappingProfile()
    {
        CreateMap<UserTag, GetUserTagsResponse>();
        
        CreateMap<UserTagRequest, UserTag>()
            .ConstructUsing(dto => new UserTag
            {
                UserId = dto.UserId,
                TagId = dto.TagId
            });
    }
}