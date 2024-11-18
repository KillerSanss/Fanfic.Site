using Domain.Entities;

namespace Application.Interfaces.BaseRepositories;

/// <summary>
/// Базовый репозиторий
/// </summary>
public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Сохранение в базе
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}