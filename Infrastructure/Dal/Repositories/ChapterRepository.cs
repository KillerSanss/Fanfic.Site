using Application.Interfaces;
using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий Chapter
/// </summary>
public class ChapterRepository : IChapterRepository
{
    private readonly FanficSiteDbContext _dbContext;

    public ChapterRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Массовое добавление Chapter
    /// </summary>
    /// <param name="chapters">Список Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkAddAsync(IEnumerable<Chapter> chapters, CancellationToken cancellationToken)
    {
        await _dbContext.BulkInsertAsync(chapters, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое обновление Chapter
    /// </summary>
    /// <param name="chapters">Список Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkUpdateAsync(IEnumerable<Chapter> chapters, CancellationToken cancellationToken)
    {
        await _dbContext.BulkUpdateAsync(chapters, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Массовое удаление Chapter
    /// </summary>
    /// <param name="chapters">Cписок Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkDeleteAsync(IEnumerable<Chapter> chapters, CancellationToken cancellationToken)
    {
        await _dbContext.BulkDeleteAsync(chapters, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Удаление Chapter
    /// </summary>
    /// <param name="chapter">Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Chapter chapter, CancellationToken cancellationToken)
    {
        _dbContext.Chapters.Remove(chapter);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Получение Chapter по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Chapter.</returns>
    public async Task<Chapter> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Chapters
            .Include(c => c.Work)
            .Include(c => c.User)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Получение всех Chapter
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Chapter.</returns>
    public async Task<List<Chapter>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Chapters
            .Include(c => c.Work)
            .Include(c => c.User)
            .ToListAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получение всех Chapter работы
    /// </summary>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Chapter.</returns>
    public async Task<List<Chapter>> GetAllWorkChaptersAsync(Guid workId, CancellationToken cancellationToken)
    {
        return await _dbContext.Chapters
            .Include(c => c.Work)
            .Include(c => c.User)
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