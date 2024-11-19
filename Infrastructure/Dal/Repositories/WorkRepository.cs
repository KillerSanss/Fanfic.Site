using Application.Interfaces;
using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий Work
/// </summary>
public class WorkRepository : IWorkRepository
{
    private readonly FanficSiteDbContext _dbContext;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public WorkRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Массовое добавление Work
    /// </summary>
    /// <param name="works">Список Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkAddAsync(IEnumerable<Work> works, CancellationToken cancellationToken)
    {
        await _dbContext.BulkInsertAsync(works, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое обновление Work
    /// </summary>
    /// <param name="works">Список Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkUpdateAsync(IEnumerable<Work> works, CancellationToken cancellationToken)
    {
        await _dbContext.BulkUpdateAsync(works, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Массовое удаление Work
    /// </summary>
    /// <param name="works">Cписок Works.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkDeleteAsync(IEnumerable<Work> works, CancellationToken cancellationToken)
    {
        await _dbContext.BulkDeleteAsync(works, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Удаление Work
    /// </summary>
    /// <param name="work">Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Work work, CancellationToken cancellationToken)
    {
        _dbContext.Works.Remove(work);
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Получение Work по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Work.</returns>
    public async Task<Work> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Works
            .Include(w => w.User)
            .Include(w => w.Chapters)
            .ThenInclude(wl => wl.Comments)
            .Include(w => w.WorkLikes)
            .ThenInclude(wl => wl.User)
            .Include(w => w.WorkTags)
            .ThenInclude(wl => wl.Tag)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Получение всех Work
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Work.</returns>
    public async Task<List<Work>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Works
            .Include(w => w.User)
            .Include(w => w.Chapters)
            .ThenInclude(wl => wl.Comments)
            .Include(w => w.WorkLikes)
            .ThenInclude(wl => wl.User)
            .Include(w => w.WorkTags)
            .ThenInclude(wl => wl.Tag)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получение всех Work пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Work.</returns>
    public async Task<List<Work>> GetAllUserWorkAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Works
            .Include(w => w.User)
            .Include(w => w.Chapters)
            .ThenInclude(wl => wl.Comments)
            .Include(w => w.WorkLikes)
            .ThenInclude(wl => wl.User)
            .Include(w => w.WorkTags)
            .ThenInclude(wl => wl.Tag)
            .Where(p => p.UserId == userId)
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