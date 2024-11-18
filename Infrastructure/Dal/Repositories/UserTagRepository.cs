using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий UserTag
/// </summary>
public class UserTagRepository : IUserTagRepository
{
    private readonly FanficSiteDbContext _dbContext;

    public UserTagRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Добавление UserTag
    /// </summary>
    /// <param name="userTag">UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>UserTag.</returns>
    public async Task<UserTag> AddAsync(UserTag userTag, CancellationToken cancellationToken)
    {
        await _dbContext.UserTags.AddAsync(userTag, cancellationToken);
        return userTag;
    }

    /// <summary>
    /// Обновление UserTag
    /// </summary>
    /// <param name="userTag">UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>UserTag.</returns>
    public Task<UserTag> UpdateAsync(UserTag userTag, CancellationToken cancellationToken)
    {
        _dbContext.UserTags.Update(userTag);
        return Task.FromResult(userTag);
    }
    
    /// <summary>
    /// Удаление UserTag
    /// </summary>
    /// <param name="userTag">UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(UserTag userTag, CancellationToken cancellationToken)
    {
        _dbContext.UserTags.Remove(userTag);
        return Task.CompletedTask;
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
        return await _dbContext.UserTags
            .Include(ut => ut.User)
            .Include(ut => ut.Tag)
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