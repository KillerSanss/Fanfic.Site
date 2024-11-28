namespace Application.Settings;

/// <summary>
/// Настройки для отправки почты
/// </summary>
public class SmtpSettings
{
    /// <summary>
    /// Сервер
    /// </summary>
    public string Server { get; set; }
    
    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }
    
    /// <summary>
    /// Имя отправителя
    /// </summary>
    public string SenderName { get; set; }
    
    /// <summary>
    /// Почта отправителя
    /// </summary>
    public string SenderEmail { get; set; }
    
    /// <summary>
    /// Никнейм
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Пароль приложения
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// ССЛ
    /// </summary>
    public bool UseSsl { get; set; }
}