using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий CommentRepository
/// </summary>
public interface ICommentRepository : IBaseRepository<Comment>, IReadRepository<Comment>, IBulkWriteRepository<Comment>
{
    /// <summary>
    /// Получение всех Comment пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public Task<List<Comment>> GetAllUserCommentsAsync(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех Comment главы
    /// </summary>
    /// <param name="chapterId">Идентификатор главы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public Task<List<Comment>> GetAllChapterCommentsAsync(Guid chapterId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех Comment родительского комментария
    /// </summary>
    /// <param name="parentCommentId">Идентификатор родительского комментария.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public Task<List<Comment>> GetAllParentCommentsAsync(Guid parentCommentId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаление Comment
    /// </summary>
    /// <param name="comment">Comment на удаление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Comment comment, CancellationToken cancellationToken);
}