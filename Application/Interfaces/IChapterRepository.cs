using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий ChapterRepository
/// </summary>
public interface IChapterRepository : IBaseRepository<Chapter>, IReadRepository<Chapter>, IBulkWriteRepository<Chapter>
{
    /// <summary>
    /// Получение всех Chapter работы
    /// </summary>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Chapter.</returns>
    public Task<List<Chapter>> GetAllWorkChaptersAsync(Guid workId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаление Chapter
    /// </summary>
    /// <param name="chapter">Chapter на удаление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Chapter chapter, CancellationToken cancellationToken);
}