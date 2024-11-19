namespace Application.Dto.UserDto;

/// <summary>
/// Дто ответа на обновление настроек User
/// </summary>
public class UpdateSettingsUserResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Отправка сообщений на почту
    /// </summary>
    public bool IsEmail { get; init; }
    
    /// <summary>
    /// Отображение почты в профиле
    /// </summary>
    public bool IsShowEmail { get; init; }
    
    /// <summary>
    /// Отправка сообщение в телегам
    /// </summary>
    public bool IsTelegram { get; init; }
    
    /// <summary>
    /// Отображение телеграма в профиле
    /// </summary>
    public bool IsShowTelegram { get; init; }
}