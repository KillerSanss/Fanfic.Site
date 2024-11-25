using Application.Dto.CommentDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

/// <summary>
/// Маппинг для Comment
/// </summary>
public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CreateCommentResponse>();
        CreateMap<Comment, UpdateCommentResponse>();
        CreateMap<Comment, GetCommentResponse>();
        CreateMap<Comment, GetUserCommentResponse>();
        CreateMap<Comment, GetChapterCommentResponse>();
        
        CreateMap<CreateCommentRequest, Comment>()
            .ConstructUsing(dto => new Comment
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Content = dto.Content,
                ChapterId = dto.ChapterId,
                ParentCommentId = dto.ParentCommentId
            });
        
        CreateMap<UpdateCommentRequest, Comment>()
            .ConstructUsing(dto => new Comment
            {
                Id = dto.Id,
                Content = dto.Content
            });
    }
}