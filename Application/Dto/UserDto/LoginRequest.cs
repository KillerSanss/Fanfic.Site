namespace Application.Dto.UserDto;

/// <summary>
/// Дто запроса на логин
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; init; }
}