using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dto.UserDto;
using Application.Interfaces;
using Application.Settings;
using Ardalis.GuardClauses;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User = Domain.Entities.User;

namespace Application.Services;

/// <summary>
/// Сервис User
/// </summary>
public class UserService
{
    private readonly IUserRepository _userRepository;

    private readonly GoogleCloudService _googleCloudService;
    private readonly IMapper _mapper;
    private readonly string _secretKey;
    
    private readonly EmailService _emailService;
    private readonly EmailMessages _emailMessages;
    private readonly BotMessage _botMessage;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="emailService">Сервис Email.</param>
    /// <param name="emailMessages">EmailMessages.</param>
    /// <param name="userRepository">Репозиторий User.</param>
    /// <param name="googleCloudService">Сервис GoogleCloud.</param>
    /// <param name="mapper">Автомаппер.</param>
    /// <param name="botMessage">BotMessages.</param>
    /// <param name="apiSettings">Настройки Api.</param>
    public UserService(
        EmailService emailService,
        EmailMessages emailMessages,
        IUserRepository userRepository,
        GoogleCloudService googleCloudService,
        IMapper mapper,
        BotMessage botMessage,
        IOptions<ApiSettings> apiSettings)
    {
        _userRepository = Guard.Against.Null(userRepository);

        _googleCloudService = Guard.Against.Null(googleCloudService);
        _mapper = Guard.Against.Null(mapper);
        _secretKey = Guard.Against.Null(apiSettings.Value.Secret);
        
        _emailService = Guard.Against.Null(emailService);
        _emailMessages = Guard.Against.Null(emailMessages);
        _botMessage = Guard.Against.Null(botMessage);
    }

    /// <summary>
    /// Регистрация User
    /// </summary>
    /// <param name="registrationRequest">Данные для User.</param>
    /// <param name="avatarFile">Аватар.</param>
    /// <param name="fileName">Название файла.</param>
    /// <param name="contentType">Тип файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый User.</returns>
    public async Task<RegistrationResponse> RegisterAsync(
        RegistrationRequest registrationRequest,
        Stream avatarFile,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(registrationRequest);
        var user = _mapper.Map<User>(registrationRequest);

        if (_userRepository.IsUniqueUser(user.Email))
            return null;
        
        user.AvatarUrl = await _googleCloudService.UploadFileAsync(avatarFile, fileName + $"-{Guid.NewGuid().ToString()}", contentType);

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        
        await _emailService.SendEmailAsync(user.Email, _emailMessages.GetWelcomeTheme(), _emailMessages.GetWelcomeMessage(user));

        return _mapper.Map<RegistrationResponse>(user);
    }
    
    /// <summary>
    /// Обновление User
    /// </summary>
    /// <param name="userRequest">Данные для обновления User.</param>
    /// <param name="fileStream">Аватар.</param>
    /// <param name="fileName">Название файла</param>
    /// <param name="contentType">Тип файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный User.</returns>
    public async Task<UpdateUserResponse> UpdateAsync(
        UpdateUserRequest userRequest,
        Stream fileStream,
        string fileName,
        string contentType, 
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(userRequest);
        var user = await GetByIdUser(userRequest.Id, cancellationToken);
        
        user.Update(
            userRequest.NickName,
            userRequest.BirthDate,
            userRequest.Gender,
            userRequest.Email,
            userRequest.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password));
        
        await _googleCloudService.DeleteFileAsync(user.AvatarUrl.Split("/").Last());
        user.AvatarUrl = await _googleCloudService.UploadFileAsync(fileStream, fileName + $"-{user.Id.ToString()}", contentType);

        await _userRepository.SaveChangesAsync(cancellationToken);

        if (user.IsEmail)
            await _emailService.SendEmailAsync(user.Email, _emailMessages.GetProfileUpdateTheme(), _emailMessages.GetProfileUpdatedMessage(user));
        if (user.IsTelegram)
            await _botMessage.SendMessage(user, user, "Вы обновили данные своего профиля", true, cancellationToken);
        
        return _mapper.Map<UpdateUserResponse>(user);
    }
    
    /// <summary>
    /// Изменить настройки
    /// </summary>
    /// <param name="userRequest">Настройки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Настройки.</returns>
    public async Task<UpdateSettingsUserResponse> UpdateSettings(
        UpdateSettingsUserRequest userRequest,
        CancellationToken cancellationToken)
    {
        var user = await GetByIdUser(userRequest.Id, cancellationToken);

        user.ChangeSettings(
            userRequest.IsEmail,
            userRequest.IsShowEmail,
            userRequest.IsTelegram,
            userRequest.IsShowTelegram);
        
        await _userRepository.SaveChangesAsync(cancellationToken);
        
        if (user.IsTelegram)
            await _botMessage.SendMessage(user, user, "Вы обновили настройки своего профиля", true, cancellationToken);

        return _mapper.Map<UpdateSettingsUserResponse>(userRequest);
    }
    
    /// <summary>
    /// Удаление User
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetByIdUser(userId, cancellationToken);
        await _userRepository.DeleteAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        
        await _googleCloudService.DeleteFileAsync(user.AvatarUrl.Split("/").Last());
        
        if (user.IsEmail)
            await _emailService.SendEmailAsync(user.Email, _emailMessages.GetProfileDeleteTheme(), _emailMessages.GetProfileDeletedMessage(user));
        if (user.IsTelegram) 
            await _botMessage.SendMessage(user, user, "Ваш профиль удален!!! Нам очень жаль что вы решили удалить ваш профиль на нашем сайте(((", false, cancellationToken);
    }
    
    /// <summary>
    /// Привязка телеграмма
    /// </summary>
    /// <param name="telegramId">Идентификатор телеграмма.</param>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task SetTelegramId(string telegramId, Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetByIdUser(userId, cancellationToken);
        user.SetTelegram(telegramId);
        
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        
        await _botMessage.SendMessage(user, user, "Телеграм привязан!!!", false, cancellationToken);
        await _botMessage.SendMessage(user, user,
            "С помощью телеграмма вы сможете получать уведомления о происходящем с вашими работами или с ваши аккаунтом \n" +
            "Так же вы будите в курсе всех обновлений для ваших любимых работ, будь то новая глава или изменение в содержании!!!", false, cancellationToken);
    }
    
    /// <summary>
    /// Отвязка телеграмма
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task UnlinkTelegram(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetByIdUser(userId, cancellationToken);
        
        user.UnlinkTelegram();
        
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        
        await _botMessage.SendMessage(user, user, "Телеграм отвязан(((", false, cancellationToken);
    }
    
    /// <summary>
    /// Логин
    /// </summary>
    /// <param name="loginRequest">Данные для входа в аккаунт.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Токен.</returns>
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        Guard.Against.Null(loginRequest);
        var user = await _userRepository.IsUserExistAsync(loginRequest.Email, cancellationToken);
        
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            return null;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Role, user.Role.ToString()) }),
            Expires = DateTime.Now.AddHours(12),
            SigningCredentials = new(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)), SecurityAlgorithms.HmacSha256Signature)
        };
        
        await _botMessage.SendMessage(user, user, "Вы успешно вошли в аккаунт", true, cancellationToken);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new LoginResponse { Token = tokenHandler.WriteToken(token) };
    }
    
    /// <summary>
    /// Получение User по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public async Task<GetUserResponse> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetByIdUser(userId, cancellationToken);
        return _mapper.Map<GetUserResponse>(user);
    }

    /// <summary>
    /// Получение User по идентификатору телеграма
    /// </summary>
    /// <param name="telegramId">Идентификатор телеграма.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public async Task<GetUserResponse> GetByTelegramIdAsync(string telegramId, CancellationToken cancellationToken)
    {
        var telegramUser = await _userRepository.GetByTelegramIdAsync(telegramId, cancellationToken);
        return _mapper.Map<GetUserResponse>(telegramUser);
    }
    
    /// <summary>
    /// Получение User по никнейму
    /// </summary>
    /// <param name="nickName">Никнейм.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    public async Task<GetUserResponse> GetByNickNameAsync(string nickName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByNickNameAsync(nickName, cancellationToken);
        return _mapper.Map<GetUserResponse>(user);
    }
    
    private async Task<User> GetByIdUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
            return null;

        return user;
    }
}