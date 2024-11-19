using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.BaseRepositories;

/// <summary>
/// Базовый репозиторий массовой записи
/// </summary>
public interface IBulkWriteRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Массовое создание сущностей
    /// </summary>
    /// <param name="entities">Список сущностей.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task BulkAddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    
    /// <summary>
    /// Массовое обновление сущностей
    /// </summary>
    /// <param name="entities">Список сущностей.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task BulkUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    
    /// <summary>
    /// Массовое удаление сущностей
    /// </summary>
    /// <param name="entities">Список сущностей.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task BulkDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
}