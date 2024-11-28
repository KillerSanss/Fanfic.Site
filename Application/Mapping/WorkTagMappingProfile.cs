using Application.Dto.WorkTagDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для WorkTag
/// </summary>
public class WorkTagMappingProfile : Profile
{
    public WorkTagMappingProfile()
    {
        CreateMap<WorkTag, GetTagWorksResponse>();
        CreateMap<WorkTag, GetWorkTagsResponse>();
        CreateMap<WorkTag, WorkTagResponse>();

        CreateMap<WorkTagRequest, WorkTag>()
            .ConstructUsing(dto => new WorkTag
            {
                WorkId = dto.WorkId,
                TagId = dto.TagId
            });
    }
}