using Domain.Primitives;

namespace Application.Dto.UserDto;

/// <summary>
/// Дто ответа на обновление User
/// </summary>
public class UpdateUserResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Никнейм
    /// </summary>
    public string NickName { get; init; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime BirthDate { get; init; }
    
    /// <summary>
    /// Гендер
    /// </summary>
    public Gender Gender { get; init; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; init; }
    
    /// <summary>
    /// Аватар
    /// </summary>
    public string AvatarUrl { get; init; }
}