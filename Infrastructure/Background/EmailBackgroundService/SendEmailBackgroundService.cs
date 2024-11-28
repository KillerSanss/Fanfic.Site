using Application.Dto.EmailDto;
using Application.Settings;
using Ardalis.GuardClauses;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;

namespace Infrastructure.Background.EmailBackgroundService;

/// <summary>
/// Фоновый сервис массовой отправки Email
/// </summary>
public class SendEmailBackgroundService : BackgroundService
{
    private readonly IDistributedCache _cache;
    private readonly SmtpSettings _smtpSettings;

    /// <summary>
    /// Конструктор
    /// </summary>
    public SendEmailBackgroundService(
        IDistributedCache cache,
        IOptions<SmtpSettings> smtpSettings)
    {
        _cache = Guard.Against.Null(cache);
        _smtpSettings = smtpSettings.Value;
    }
    
    /// <summary>
    /// Массовая отправка Email
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cacheKey = "EMAILS-TO-SEND";
            var cachedEmails = await _cache.GetStringAsync(cacheKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedEmails))
            {
                var emailsList = JsonConvert.DeserializeObject<List<CreateEmailRequest>>(cachedEmails);
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls, stoppingToken);
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password, stoppingToken);

                foreach (var email in emailsList)
                {
                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                    mimeMessage.To.Add(MailboxAddress.Parse(email.ToEmail));
                    mimeMessage.Subject = email.Subject;
                    mimeMessage.Body = new TextPart("html") { Text = email.Message };

                    await smtp.SendAsync(mimeMessage, stoppingToken);
                }
                
                await smtp.DisconnectAsync(true, stoppingToken);
                await _cache.RemoveAsync(cacheKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}