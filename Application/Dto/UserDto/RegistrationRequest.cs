using Domain.Primitives;

namespace Application.Dto.UserDto;

/// <summary>
/// Дто запроса на регистрацию User
/// </summary>
public class RegistrationRequest
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
    public string Password { get; set; }
}