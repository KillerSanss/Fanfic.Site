using Application.Interfaces;
using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий WorkTag
/// </summary>
public class WorkTagRepository : IWorkTagRepository
{
    private readonly FanficSiteDbContext _dbContext;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public WorkTagRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Массовое добавления WorkTag
    /// </summary>
    /// <param name="workTags">Список WorkTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkAddAsync(IEnumerable<WorkTag> workTags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkInsertAsync(workTags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое обновление WorkTag
    /// </summary>
    /// <param name="workTags">Список WorkTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkUpdateAsync(IEnumerable<WorkTag> workTags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkUpdateAsync(workTags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое удаление WorkTag
    /// </summary>
    /// <param name="workTags">Список WorkTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkDeleteAsync(IEnumerable<WorkTag> workTags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkDeleteAsync(workTags, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Получение по WorkTag по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор WorkTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>WorkTag.</returns>
    public async Task<WorkTag> GetByIdAsync(Guid tagId, CancellationToken cancellationToken)
    {
        return await _dbContext.WorkTags.FirstOrDefaultAsync(p => p.TagId == tagId, cancellationToken);
    }

    /// <summary>
    /// Получение всех WorkTag
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список WorkTag.</returns>
    public async Task<List<WorkTag>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.WorkTags.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получение всех тэгов работы
    /// </summary>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список WorkTag.</returns>
    public async Task<List<WorkTag>> GetAllWorkTagsAsync(Guid workId, CancellationToken cancellationToken)
    {
        return await _dbContext.WorkTags.Where(p => p.WorkId == workId).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получение всех WorkTag по идентификатору тэга
    /// </summary>
    /// <param name="tagId">Идентификатор тэга.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список WorkTag.</returns>
    public async Task<List<WorkTag>> GetAllWorkTagsByTagIdAsync(Guid tagId, CancellationToken cancellationToken)
    {
        return await _dbContext.WorkTags.Where(p => p.TagId == tagId).ToListAsync(cancellationToken);
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