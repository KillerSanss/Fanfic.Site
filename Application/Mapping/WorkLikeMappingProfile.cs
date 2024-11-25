using Application.Dto.WorkLikeDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для WorkLike
/// </summary>
public class WorkLikeMappingProfile : Profile
{
    public WorkLikeMappingProfile()
    {
        CreateMap<WorkLike, GetWorkLikesResponse>();
        CreateMap<WorkLike, GetUserLikesResponse>();
        
        CreateMap<WorkLikeRequest, WorkLike>()
            .ConstructUsing(dto => new WorkLike
            {
                UserId = dto.UserId,
                WorkId = dto.WorkId
            });
    }
}