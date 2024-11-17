using Ardalis.GuardClauses;

namespace Domain.Entities;

/// <summary>
/// Базовая сущность
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Установка идентификатора
    /// </summary>
    protected void SetId(Guid id)
    {
        Id = Guard.Against.NullOrEmpty(id);
    }
}