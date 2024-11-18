using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий WorkTagRepository
/// </summary>
public interface IWorkTagRepository : IBaseRepository<WorkTag>, IBulkWriteRepository<WorkTag>
{
    /// <summary>
    /// Получение всех тэгов работы
    /// </summary>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список WorkTag.</returns>
    public Task<List<WorkTag>> GetAllWorkTagsAsync(Guid workId, CancellationToken cancellationToken);

    /// <summary>
    /// Получение всех WorkTag по идентификатору тэга
    /// </summary>
    /// <param name="tagId">Идентификатор тэга.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список WorkTag.</returns>
    public Task<List<WorkTag>> GetAllWorkTagsByTagIdAsync(Guid tagId, CancellationToken cancellationToken);
}