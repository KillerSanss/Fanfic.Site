using Application.Dto.ChapterDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для Chapter
/// </summary>
public class ChapterMappingProfile : Profile
{
    public ChapterMappingProfile()
    {
        CreateMap<Chapter, CreateChapterResponse>();
        CreateMap<Chapter, UpdateChapterResponse>();
        CreateMap<Chapter, GetChapterResponse>();
        CreateMap<Chapter, GetWorkChapterResponse>();

        CreateMap<CreateChapterRequest, Chapter>()
            .ConstructUsing(dto => new Chapter
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                Content = dto.Content
            });
        
        CreateMap<UpdateChapterRequest, Chapter>()
            .ConstructUsing(dto => new Chapter
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Content = dto.Content
            });
    }
}