using Application.Interfaces;
using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий WorkLike
/// </summary>
public class WorkLikeRepository : IWorkLikeRepository
{
    private readonly FanficSiteDbContext _dbContext;

    public WorkLikeRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Массовое добавление WorkLike
    /// </summary>
    /// <param name="workLikes">Список WorkLIke.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkAddAsync(IEnumerable<WorkLike> workLikes, CancellationToken cancellationToken)
    {
        await _dbContext.BulkInsertAsync(workLikes, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое обновление WorkLike
    /// </summary>
    /// <param name="workLikes">Список WorkLike.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkUpdateAsync(IEnumerable<WorkLike> workLikes, CancellationToken cancellationToken)
    {
        await _dbContext.BulkUpdateAsync(workLikes, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Массовое удаление WorkLike
    /// </summary>
    /// <param name="workLikes">Cписок WorkLike.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkDeleteAsync(IEnumerable<WorkLike> workLikes, CancellationToken cancellationToken)
    {
        await _dbContext.BulkDeleteAsync(workLikes, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Проверка наличия лайка от пользователя на работе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<bool> HasUserLikedWorkAsync(Guid userId, Guid workId, CancellationToken cancellationToken)
    {
        return _dbContext.WorkLikes.AnyAsync(p => p.UserId == userId && p.WorkId == workId, cancellationToken);
    }

    /// <summary>
    /// Получение всех лайков пользователей на работе
    /// </summary>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список WorkLike.</returns>
    public async Task<List<WorkLike>> GetAllWorkLikesAsync(Guid workId, CancellationToken cancellationToken)
    {
        return await _dbContext.WorkLikes
            .Include(wl => wl.User)
            .Include(wl => wl.WorkId)
            .Where(p => p.WorkId == workId)
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