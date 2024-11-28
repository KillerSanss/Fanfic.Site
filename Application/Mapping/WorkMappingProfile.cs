using Application.Dto.WorkDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для Work
/// </summary>
public class WorkMappingProfile : Profile
{
    public WorkMappingProfile()
    {
        CreateMap<Work, GetWorkResponse>();
        CreateMap<Work, GetUserWorkResponse>();
        CreateMap<Work, CreateWorkResponse>();
        CreateMap<Work, UpdateWorkResponse>();

        CreateMap<CreateWorkRequest, Work>()
            .ConstructUsing(dto => new Work
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category
            });
        
        CreateMap<UpdateWorkRequest, Work>()
            .ConstructUsing(dto => new Work
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category
            });
    }
}