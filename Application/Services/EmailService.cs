using Application.Dto.EmailDto;
using Application.Dto.UserDto;
using Application.Settings;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Services;

/// <summary>
/// Сервис Email
/// </summary>
public class EmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly AddToCache _addToCache;

    /// <summary>
    /// Настройки
    /// </summary>
    /// <param name="smtpSettings">Email настройки.</param>
    /// <param name="addToCache">AddToCache.</param>
    public EmailService(
        IOptions<SmtpSettings> smtpSettings,
        AddToCache addToCache)
    {
        _smtpSettings = smtpSettings.Value;
        _addToCache = Guard.Against.Null(addToCache);
    }

    /// <summary>
    /// Отправка сообщения
    /// </summary>
    /// <param name="userEmail">Почта получателя.</param>
    /// <param name="subject">Тема.</param>
    /// <param name="message">Сообщение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task CreateEmailAsync(
        string userEmail,
        string subject,
        string message,
        CancellationToken cancellationToken)
    {
        var email = new MimeMessage();
        
        email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(userEmail));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = message };

        var emailMessage = new CreateEmailRequest { SenderName = _smtpSettings.SenderName, SenderEmail = _smtpSettings.SenderEmail, ToEmail = userEmail, Subject = subject, Message = message };

        await _addToCache.StoreInCache(emailMessage, "EMAILS-TO-SEND", cancellationToken);
    }
}