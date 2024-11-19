using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий WorkLikeRepository
/// </summary>
public interface IWorkLikeRepository : IBaseRepository<WorkLike>, IBulkWriteRepository<WorkLike>
{
    /// <summary>
    /// Проверка наличия лайка у пользователя на работе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<bool> HasUserLikedWorkAsync(Guid userId, Guid workId, CancellationToken cancellationToken);

    /// <summary>
    /// Получение всех лайков пользователей у работы
    /// </summary>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список WorkLike.</returns>
    public Task<List<WorkLike>> GetAllWorkLikesAsync(Guid workId, CancellationToken cancellationToken);
}