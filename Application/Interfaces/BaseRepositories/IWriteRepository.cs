using Domain.Entities;

namespace Application.Interfaces.BaseRepositories;

/// <summary>
/// Базовый репозиторий записи
/// </summary>
public interface IWriteRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Добавление сущности
    /// </summary>
    /// <param name="entity">Данные для добавления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новая сущность.</returns>
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновление сущности
    /// </summary>
    /// <param name="entity">Данные для обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленная сущность.</returns>
    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}