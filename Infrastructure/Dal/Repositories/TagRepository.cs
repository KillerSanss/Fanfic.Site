using Application.Interfaces;
using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий Tag
/// </summary>
public class TagRepository : ITagRepository
{
    private readonly FanficSiteDbContext _dbContext;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public TagRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Массовое добавление Tag
    /// </summary>
    /// <param name="tags">Список Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkAddAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkInsertAsync(tags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое обновление Tag
    /// </summary>
    /// <param name="tags">Список Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkUpdateAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkUpdateAsync(tags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое удаление Tag
    /// </summary>
    /// <param name="tags">Cписок Tags.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkDeleteAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkDeleteAsync(tags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Удаление Tag
    /// </summary>
    /// <param name="tag">Тag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Tag tag, CancellationToken cancellationToken)
    {
        _dbContext.Tags.Remove(tag);
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Получение Tag по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Tag.</returns>
    public async Task<Tag> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tags
            .Include(t => t.WorkTags)
            .Include(t => t.UserTags)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
    
    /// <summary>
    /// Получение всех Tag
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Tag.</returns>
    public async Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Tags
            .Include(t => t.WorkTags)
            .Include(t => t.UserTags)
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