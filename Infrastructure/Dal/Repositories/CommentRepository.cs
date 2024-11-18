using Application.Interfaces;
using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий Comment
/// </summary>
public class CommentRepository : ICommentRepository
{
    private readonly FanficSiteDbContext _dbContext;

    public CommentRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Массовое добавление Comment
    /// </summary>
    /// <param name="comments">Список Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkAddAsync(IEnumerable<Comment> comments, CancellationToken cancellationToken)
    {
        await _dbContext.BulkInsertAsync(comments, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое обновление Comment
    /// </summary>
    /// <param name="comments">Список Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkUpdateAsync(IEnumerable<Comment> comments, CancellationToken cancellationToken)
    {
        await _dbContext.BulkUpdateAsync(comments, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое удаление Comment
    /// </summary>
    /// <param name="comments">Cписок Comments.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkDeleteAsync(IEnumerable<Comment> comments, CancellationToken cancellationToken)
    {
        await _dbContext.BulkDeleteAsync(comments, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Удаление Comment
    /// </summary>
    /// <param name="comment">Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Comment comment, CancellationToken cancellationToken)
    {
        _dbContext.Comments.Remove(comment);
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Получение Comment по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Comment.</returns>
    public async Task<Comment> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Chapter)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Получение всех Comment
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public async Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Chapter)
            .ToListAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получение всех Comment пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public async Task<List<Comment>> GetAllUserCommentsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Chapter)
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получение всех Comment главы
    /// </summary>
    /// <param name="chapterId">Идентификатор главы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public async Task<List<Comment>> GetAllChapterCommentsAsync(Guid chapterId, CancellationToken cancellationToken)
    {
        return await _dbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Chapter)
            .Where(p => p.ChapterId == chapterId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получение всех ответов на Comment
    /// </summary>
    /// <param name="parentCommentId">Идентификатор родительского комментария.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public async Task<List<Comment>> GetAllParentCommentsAsync(Guid parentCommentId, CancellationToken cancellationToken)
    {
        return await _dbContext.Comments
            .Include(c => c.User)
            .Include(c => c.Chapter)
            .Where(p => p.ParentCommentId == parentCommentId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Сохранение изменений в базе
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        _dbContext.SaveChangesAsync(cancellationToken);
        return Task.CompletedTask;
    }
}