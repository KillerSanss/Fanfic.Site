using Domain.Entities;
using Domain.Primitives;

namespace Application.Dto.UserDto;

/// <summary>
/// Дто ответа на получение User по идентификатору телеграма
/// </summary>
public class GetByTelegramIdUserResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Никнейм
    /// </summary>
    public string NickName { get; init; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime BirthDate { get; init; }
    
    /// <summary>
    /// Гендер
    /// </summary>
    public Gender Gender { get; init; }
    
    /// <summary>
    /// Дата регистрации
    /// </summary>
    public DateTime RegistrationDate { get; init; }
    
    /// <summary>
    /// Роль
    /// </summary>
    public Role Role { get; init; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; init; }
    
    /// <summary>
    /// Идентификатор телеграмма
    /// </summary>
    public string TelegramId { get; init; }
    
    /// <summary>
    /// Ссылка на аватар пользователя
    /// </summary>
    public string AvatarUrl { get; init; }
    
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
    
    /// <summary>
    /// Список Work
    /// </summary>
    public List<Work> Works { get; init; }
    
    /// <summary>
    /// Список WorkLike
    /// </summary>
    public List<WorkLike> WorksLikes { get; init; }
    
    /// <summary>
    /// Список UserTag
    /// </summary>
    public List<UserTag> UserTags { get; init; }
    
    /// <summary>
    /// Список Comment
    /// </summary>
    public List<Comment> Comments { get; init; }
}