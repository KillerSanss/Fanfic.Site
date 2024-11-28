using Application.Interfaces.BaseRepositories;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс описывающий TagRepository
/// </summary>
public interface ITagRepository : IBaseRepository<Tag>, IReadRepository<Tag>, IBulkWriteRepository<Tag>
{
    /// <summary>
    /// Удаление Tag
    /// </summary>
    /// <param name="tag">Tag на удаление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task DeleteAsync(Tag tag, CancellationToken cancellationToken);
}