using Domain.Primitives;
using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Сущность пользователя
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Никнейм
    /// </summary>
    public string NickName { get; set; }
    
    /// <summary>
    /// Дата регистрации
    /// </summary>
    public DateTime RegistrationDate { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime BirthDate { get; set; }
    
    /// <summary>
    /// Гендер
    /// </summary>
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Роль
    /// </summary>
    public Role Role { get; set; } = Role.User;
    
    /// <summary>
    /// Идентификатор телеграмма
    /// </summary>
    public string? TelegramId { get; set; }
    
    /// <summary>
    /// Аватар
    /// </summary>
    public string AvatarUrl { get; set; }
    
    /// <summary>
    /// Отправлять сообщения на почту
    /// </summary>
    public bool IsEmail { get; set; }
    
    /// <summary>
    /// Показывать почту
    /// </summary>
    public bool IsShowEmail { get; set; }
    
    /// <summary>
    /// Отправлять сообщения в телеграм
    /// </summary>
    public bool IsTelegram { get; set; }
    
    /// <summary>
    /// Показывать телеграм
    /// </summary>
    public bool IsShowTelegram { get; set; }
    
    /// <summary>
    /// Список работ
    /// </summary>
    public ICollection<Work> Works = new List<Work>();
    
    /// <summary>
    /// Список лайков
    /// </summary>
    public ICollection<WorkLike> WorkLikes = new List<WorkLike>();
    
    /// <summary>
    /// Список любимых тэгов
    /// </summary>
    public ICollection<UserTag> UserTags = new List<UserTag>();
    
    /// <summary>
    /// Список комментариев
    /// </summary>
    public ICollection<Comment> Comments = new List<Comment>();

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="nickName">Никнейм.</param>
    /// <param name="birthDate">Дата рождения.</param>
    /// <param name="gender">Гендер.</param>
    /// <param name="email">Электронная почта.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="avatarUrl">Аватар.</param>
    public User(
        Guid id,
        string nickName,
        DateTime birthDate,
        Gender gender,
        string email,
        string password,
        string avatarUrl)
    {
        SetId(id);
        NickName = nickName;
        RegistrationDate = DateTime.Now;
        BirthDate = birthDate;
        Gender = gender;
        Email = email;
        Password = password;
        AvatarUrl = avatarUrl;
        IsEmail = true;
        IsShowEmail = false;
        IsTelegram = false;
        IsShowTelegram = false;

        Validate();
    }
    
    /// <summary>
    /// Обновление пользователя
    /// </summary>
    /// <param name="nickName">Никнейм.</param>
    /// <param name="birthDate">Дата рождения.</param>
    /// <param name="gender">Гендер.</param>
    /// <param name="email">Электронная почта.</param>
    /// <param name="password">Пароль</param>
    /// <param name="avatarUrl"></param>
    /// <returns>Обновленный пользователь.</returns>
    public User Update(
        string nickName,
        DateTime birthDate,
        Gender gender,
        string email,
        string password,
        string avatarUrl)
    {
        NickName = nickName;
        BirthDate = birthDate;
        Gender = gender;
        Email = email;
        Password = password;
        AvatarUrl = avatarUrl;
        
        Validate();
        
        return this;
    }
    
    /// <summary>
    /// Привязка телеграмма
    /// </summary>
    /// <param name="telegramId">Идентификатор телеграмма.</param>
    /// <returns>Пользователь.</returns>
    public User SetTelegram(string telegramId)
    {
        TelegramId = telegramId;
        
        return this;
    }
    
    /// <summary>
    /// Отвязка телеграмма
    /// </summary>
    /// <returns>Пользователь.</returns>
    public void UnlinkTelegram()
    {
        TelegramId = null;
    }
    
    /// <summary>
    /// Изменение настроек
    /// </summary>
    /// <param name="isEmail">Отправка сообщений на почту.</param>
    /// <param name="isShowEmail">Отображение электронной почты в профиле.</param>
    /// <param name="isTelegram">Отправка сообщений в телеграм.</param>
    /// <param name="isShowTelegram">Отображение телеграмма в профиле.</param>
    /// <returns>Пользователь с обновленными настройками.</returns>
    public User ChangeSettings(
        bool isEmail,
        bool isShowEmail,
        bool isTelegram,
        bool isShowTelegram)
    {
        IsEmail = isEmail;
        IsShowEmail = isShowEmail;
        IsTelegram = isTelegram;
        IsShowTelegram = isShowTelegram;
        
        Validate();

        return this;
    }
    
    private void Validate()
    {
        var validator = new UserValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }

    public User()
    {
    }
}