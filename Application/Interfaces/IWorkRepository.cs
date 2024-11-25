using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий WorkRepository
/// </summary>
public interface IWorkRepository : IBaseRepository<Work>, IReadRepository<Work>, IBulkWriteRepository<Work>
{
    /// <summary>
    /// Получение всех Work пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Work.</returns>
    public Task<List<Work>> GetAllUserWorkAsync(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаление Work
    /// </summary>
    /// <param name="work">Work на удаление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Work work, CancellationToken cancellationToken);
}