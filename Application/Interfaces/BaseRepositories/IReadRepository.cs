using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.BaseRepositories;

/// <summary>
/// Базовый репозиторий чтения
/// </summary>
public interface IReadRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Получение по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Сущность.</returns>
    public Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список всех сущностей.</returns>
    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
}