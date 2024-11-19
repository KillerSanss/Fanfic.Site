namespace Application.Settings;

/// <summary>
/// Настройки гугл облака
/// </summary>
public class GoogleSettings
{
    /// <summary>
    /// Путь к файлу json облака
    /// </summary>
    public string CredentialsPath { get; init; }
    
    /// <summary>
    /// Названия бакета
    /// </summary>
    public string BucketName { get; init; }
}