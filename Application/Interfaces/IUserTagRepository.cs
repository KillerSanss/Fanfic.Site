using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий UserTagRepository
/// </summary>
public interface IUserTagRepository : IBaseRepository<UserTag>, IBulkWriteRepository<UserTag>
{
    /// <summary>
    /// Проверка наличия лайка у пользователя на тэге
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="tagId">Идентификатор тэга.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<bool> HasUserLikedTagAsync(Guid userId, Guid tagId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех UserTag пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список UserTag.</returns>
    public Task<List<UserTag>> GetAllUserTags(Guid userId, CancellationToken cancellationToken);
}