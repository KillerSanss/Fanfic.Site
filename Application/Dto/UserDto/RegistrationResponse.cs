using Domain.Primitives;

namespace Application.Dto.UserDto;

/// <summary>
/// Дто ответа на регистрацию User
/// </summary>
public class RegistrationResponse
{
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
    
    /// <summary>
    /// Роль
    /// </summary>
    public Role Role { get; init; }
}