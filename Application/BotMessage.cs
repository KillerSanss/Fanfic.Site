using Application.Settings;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application;

/// <summary>
/// Класс отправки сообщений ботом
/// </summary>
public class BotMessage
{
    private readonly TelegramBotClient _telegramBotClient;

    public BotMessage(IOptions<ApiSettings> apiSettings)
    {
        var botToken = apiSettings.Value.BotKey;
        _telegramBotClient = new TelegramBotClient(botToken);
    }
    
    /// <summary>
    /// Отправка сообщения.
    /// </summary>
    /// <param name="user">User.</param>
    /// <param name="entity">Сущность связанная с сообщением.</param>
    /// <param name="message">Сообщение.</param>
    /// <param name="isAddKeyboard">Добавление кнопок управления к сообщению.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task SendMessage<T>(User user, T entity, string message, bool isAddKeyboard, CancellationToken cancellationToken) where T : BaseEntity
    {
        if (user.TelegramId != null)
        {
            var entityType = typeof(T);
            string entityProperty = string.Empty;
            
            var titleProperty = entityType.GetProperty("Title");
            if (titleProperty != null)
            {
                entityProperty = titleProperty.GetValue(entity)?.ToString();
            }
            var nickNameProperty = entityType.GetProperty("NickName");
            if (nickNameProperty != null)
            {
                entityProperty = nickNameProperty.GetValue(entity)?.ToString();
            }
            else
            {
                var nameProperty = entityType.GetProperty("Name");
                if (nameProperty != null)
                {
                    entityProperty = nameProperty.GetValue(entity)?.ToString();
                }
            }

            if (isAddKeyboard)
            {
                var inlineKeyboard = CreateInlineKeyboard(entity, entityProperty);
                await _telegramBotClient.SendMessage(user.TelegramId, message, replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
            }
            else
                await _telegramBotClient.SendMessage(user.TelegramId, message, cancellationToken: cancellationToken);
        }
    }
    
    /// <summary>
    /// Создание кнопок управления
    /// </summary>
    /// <param name="entity">Сущность связанная с сообщением.</param>
    /// <param name="entityProperty">Параметр сущности.</param>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <returns>Кнопки управления.</returns>
    private InlineKeyboardMarkup CreateInlineKeyboard<T>(T entity, string entityProperty) where T : BaseEntity
    {
        return new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithUrl($"{entityProperty}",
                $"http://192.168.6.117:8087/{typeof(T).Name.ToLowerInvariant()}s/{entity.Id}")
        });
    }
}