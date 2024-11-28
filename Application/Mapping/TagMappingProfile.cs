using Application.Dto.TagDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для Tag
/// </summary>
public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<Tag, GetTagResponse>()
            .ForMember(dest => dest.TagWorks, opt => opt.MapFrom(src => src.WorkTags));
        
        CreateMap<Tag, CreateTagResponse>();
        CreateMap<Tag, UpdateTagResponse>();

        CreateMap<CreateTagRequest, Tag>()
            .ConstructUsing(dto => new Tag
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                AgeRestriction = dto.AgeRestriction,
                Description = dto.Description
            });
        
        CreateMap<UpdateTagRequest, Tag>()
            .ConstructUsing(dto => new Tag
            {
                Id = dto.Id,
                Name = dto.Name,
                AgeRestriction = dto.AgeRestriction,
                Description = dto.Description
            });
    }
}