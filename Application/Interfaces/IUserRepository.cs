using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий UserRepository
/// </summary>
public interface IUserRepository : IBaseRepository<User>, IReadRepository<User>, IWriteRepository<User>
{
    /// <summary>
    /// Проверка существования User
    /// </summary>
    /// <param name="email">Электронная почта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public Task<User> IsUserExistAsync(string email, CancellationToken cancellationToken);
    
    /// <summary>
    /// Проверка уникальности User
    /// </summary>
    /// <param name="email">Электронная почта.</param>
    public bool IsUniqueUser(string email);
    
    /// <summary>
    /// Получение User по идентификатору телеграмма
    /// </summary>
    /// <param name="telegramId">Идентификатор телеграмма.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public Task<User> GetByTelegramIdAsync(string telegramId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение User по никнейму
    /// </summary>
    /// <param name="nickName">Никнейм.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public Task<User> GetByNickNameAsync(string nickName, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаление User
    /// </summary>
    /// <param name="user">User на удаление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(User user, CancellationToken cancellationToken);
}