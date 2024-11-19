namespace Application.Settings;

/// <summary>
/// Класс настроек для апи
/// </summary>
public class ApiSettings
{
    /// <summary>
    /// Секрет пользователя
    /// </summary>
    public string Secret { get; init; }
    
    /// <summary>
    /// Ключ бота
    /// </summary>
    public string BotKey { get; init; }
}