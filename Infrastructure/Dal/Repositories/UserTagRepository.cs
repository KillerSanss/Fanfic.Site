using Application.Interfaces;
using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий UserTag
/// </summary>
public class UserTagRepository : IUserTagRepository
{
    private readonly FanficSiteDbContext _dbContext;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public UserTagRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Массовое добавление UserTag
    /// </summary>
    /// <param name="userTags">Список UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkAddAsync(IEnumerable<UserTag> userTags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkInsertAsync(userTags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое обновление UserTag
    /// </summary>
    /// <param name="userTags">Список UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkUpdateAsync(IEnumerable<UserTag> userTags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkUpdateAsync(userTags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Массовое удаление UserTag
    /// </summary>
    /// <param name="userTags">Список UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task BulkDeleteAsync(IEnumerable<UserTag> userTags, CancellationToken cancellationToken)
    {
        await _dbContext.BulkDeleteAsync(userTags, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Проверка, что пользователь добавил тэг в понравившиеся
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="tagId">Идентификатор тэга.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task<bool> HasUserLikedTagAsync(Guid userId, Guid tagId, CancellationToken cancellationToken)
    {
        return await _dbContext.UserTags.AnyAsync(p => p.UserId == userId && p.TagId == tagId, cancellationToken);
    }

    /// <summary>
    /// Получение всех любимых тэгов пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список UserTag.</returns>
    public async Task<List<UserTag>> GetAllUserTags(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.UserTags.Where(p => p.UserId == userId).ToListAsync(cancellationToken);
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