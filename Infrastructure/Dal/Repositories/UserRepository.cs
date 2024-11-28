using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Dal.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

/// <summary>
/// Репозиторий User
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly FanficSiteDbContext _dbContext;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public UserRepository(FanficSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Проверка существования User
    /// </summary>
    /// <param name="email">Электронная почта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns> 
    public async Task<User> IsUserExistAsync(string email, CancellationToken cancellationToken)
    {
        var existUser = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Email == email, cancellationToken);

        if (existUser == null)
            return null;

        return existUser;
    }

    /// <summary>
    /// Проверка уникальности User
    /// </summary>
    /// <param name="email">Электронная почта.</param>
    public bool IsUniqueUser(string email)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
            return false;

        return true;
    }

    /// <summary>
    /// Добавление User
    /// </summary>
    /// <param name="user">User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        return user;
    }

    /// <summary>
    /// Обновление User
    /// </summary>
    /// <param name="user">User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        return Task.FromResult(user);
    }

    /// <summary>
    /// Удаление User
    /// </summary>
    /// <param name="user">User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Получение User по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(w => w.UserTags)
                .ThenInclude(wt => wt.Tag)
            .Include(w => w.WorkLikes)
                .ThenInclude(wt => wt.Work)
            .Include(w => w.Works)
            .Include(w => w.Comments)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
    
    /// <summary>
    /// Получение User по идентификатору телеграма
    /// </summary>
    /// <param name="telegramId">Идентификатор телеграма.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public async Task<User> GetByTelegramIdAsync(string telegramId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(w => w.UserTags)
                .ThenInclude(wt => wt.Tag)
            .Include(w => w.WorkLikes)
                .ThenInclude(wt => wt.Work)
            .Include(w => w.Works)
            .Include(w => w.Comments)
            .FirstOrDefaultAsync(p => p.TelegramId == telegramId, cancellationToken);
    }

    /// <summary>
    /// Получение User по никнейму
    /// </summary>
    /// <param name="nickName">Никнейм.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public async Task<User> GetByNickNameAsync(string nickName, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(w => w.UserTags)
                .ThenInclude(wt => wt.Tag)
            .Include(w => w.WorkLikes)
                .ThenInclude(wt => wt.Work)
            .Include(w => w.Works)
            .Include(w => w.Comments)
            .FirstOrDefaultAsync(p => p.NickName == nickName, cancellationToken);
    }

    /// <summary>
    /// Получение всех User
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список User.</returns>
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(w => w.UserTags)
                .ThenInclude(wt => wt.Tag)
            .Include(w => w.WorkLikes)
                .ThenInclude(wt => wt.Work)
            .Include(w => w.Works)
            .Include(w => w.Comments)
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