using System.Text;
using Application.Settings;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;

/// <summary>
/// Сервис бота
/// </summary>
public class BotService
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    private readonly TelegramBotClient _telegramBotClient;
    private readonly Dictionary<long, bool> _awaitingNicknames = new();

    public BotService(
        IServiceScopeFactory scopeFactory,
        IOptions<ApiSettings> apiSettings)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _telegramBotClient = Guard.Against.Null(new TelegramBotClient(apiSettings.Value.BotKey));
    }
    
    /// <summary>
    /// Запуск бота
    /// </summary>
    public async void StartBot()
    {
        await Task.Run(StartCommandHandling);
    }
    
    /// <summary>
    /// Начало обработки команд
    /// </summary>
    private void StartCommandHandling()
    {
        var receiverOptions = new ReceiverOptions { AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery] };

        _telegramBotClient.StartReceiving(
            async (_, update, ct) =>
            {
                try
                {
                    await HandleUpdateAsync(update, ct);
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(ex);
                }
            },
            async (_, exception, _) => await HandleErrorAsync(exception),
            receiverOptions);

        Console.WriteLine("Бот начал ожидать команды.");
    }
    
    /// <summary>
    /// Начало реагирования на команды
    /// </summary>
    /// <param name="update">Команда.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task HandleUpdateAsync(
        Update update,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Запрос отмены получен.");
            return;
        }
        
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Получено сообщение от {chatId}: {messageText}");

            if (!messageText.StartsWith("/"))
            {
                await SendUserAsync(messageText, chatId, cancellationToken);
            }
            else
            {
                switch (messageText)
                {
                    case "/help":
                        await SendHelpMessageAsync(chatId, cancellationToken);
                        break;
                    case "/me":
                        await SendMeAsync(chatId, cancellationToken);
                        break;
                    case "/user":
                        _awaitingNicknames[chatId] = true; 
                        await _telegramBotClient.SendMessage(chatId, "Введите никнейм пользователя:", cancellationToken: cancellationToken);
                        break;
                    case "/myWorks":
                        await SendMyWorksAsync(chatId, cancellationToken);
                        break;
                    default:
                        await SendUnknownCommandMessage(chatId, cancellationToken);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Отправка сообщения о неизвестной команде
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task SendUnknownCommandMessage(long chatId, CancellationToken cancellationToken)
    {
        const string unknownCommandMessage = "Неизвестная команда. Используйте /help для получения списка команд.";
        await _telegramBotClient.SendMessage(chatId, unknownCommandMessage, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Отправка списка всех команд
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task SendHelpMessageAsync(long chatId, CancellationToken cancellationToken)
    {
        const string helpMessage = "Список доступных команд:\n" +
                                   "/me - Данные вашего профиля \n" +
                                   "/user - Данные пользователя \n" +
                                   "/myWorks - Посмотреть мои работы\n";
        await _telegramBotClient.SendMessage(chatId, helpMessage, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Отправка данных о текущем пользователе сайта
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task SendMeAsync(long chatId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();

        Chat chat = await _telegramBotClient.GetChat(chatId.ToString(), cancellationToken);
        var user = await userService.GetByTelegramIdAsync(chatId.ToString(), cancellationToken);
        
        await _telegramBotClient.SendMessage(
            chatId,
            $"Nickname: {user.NickName} \n" +
            $"Email: {user.Email} \n" +
            $"Gender: {user.Gender} \n" +
            $"Registration Date: {user.RegistrationDate} \n" +
            $"Birth Date: {user.BirthDate} \n" + 
            $"Role: {user.Role} \n" +
            $"Telegram: [@{chat.Username}](https://t.me/{chat.Username})",
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Отправка данных о пользователе на сайте по никнейму
    /// </summary>
    /// <param name="nickName">Никнейм.</param>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task SendUserAsync(string nickName, long chatId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        
        var user = await userService.GetByNickNameAsync(nickName, cancellationToken);

        if (user == null)
        {
            await _telegramBotClient.SendMessage(chatId, "Пользователь с таким никнеймом не найден.", cancellationToken: cancellationToken);
            return;
        }
        
        var messageBuilder = new StringBuilder()
            .AppendLine($"Nickname: {user.NickName}")
            .AppendLine($"Email: {user.Email}")
            .AppendLine($"Gender: {user.Gender}")
            .AppendLine($"Registration Date: {user.RegistrationDate}")
            .AppendLine($"Birth Date: {user.BirthDate}")
            .AppendLine($"Role: {user.Role}");

        if (user.TelegramId != null)
        {
            Chat chat = await _telegramBotClient.GetChat(user.TelegramId, cancellationToken);
            messageBuilder.AppendLine($"Telegram: [@{chat.Username}](https://t.me/{chat.Username})");
        }
        
        await _telegramBotClient.SendMessage(chatId, messageBuilder.ToString(), parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Отправка всех работ текущего пользователя сайта
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task SendMyWorksAsync(long chatId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        
        var user = await userService.GetByTelegramIdAsync(chatId.ToString(), cancellationToken);
        if (user.Works == null || !user.Works.Any())
        {
            await _telegramBotClient.SendMessage(chatId, "У вас нет работ.", cancellationToken: cancellationToken);
            return;
        }
        
        var inlineKeyboardButtons = user.Works
            .Select(work => InlineKeyboardButton.WithUrl(work.Title, $"http://192.168.6.117:8087/works/{work.Id}"))
            .Select(button => new[] { button })
            .ToList();
        
        await _telegramBotClient.SendMessage(chatId, "Ваши работы:", replyMarkup: new InlineKeyboardMarkup(inlineKeyboardButtons), cancellationToken: cancellationToken);
    }
    
    private static async Task HandleErrorAsync(Exception exception)
    {
        Console.WriteLine($"Произошла ошибка: {exception.Message}");
        if (exception.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {exception.InnerException.Message}");
        }
        
        if (exception is TaskCanceledException)
        {
            Console.WriteLine("Произошел TaskCanceledException, возможно, токен отмены был вызван.");
        }

        await Task.Delay(1000);
    }
}