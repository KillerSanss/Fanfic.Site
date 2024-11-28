namespace Domain.Primitives;

/// <summary>
/// Перечисление возрастных ограничений
/// </summary>
public enum AgeRestriction
{
    None = 0,
    GeneralAudience = 1,
    Teens = 2,
    Explicit = 3,
    Nc17 = 4,
    Nc21 = 5
}