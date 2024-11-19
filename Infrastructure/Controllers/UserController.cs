using System.Security.Claims;
using Application.Dto.UserDto;
using Application.Services;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

/// <summary>
/// Контроллер User
/// </summary>
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="userService">UserService.</param>
    public UserController(
        UserService userService)
    {
        _userService = Guard.Against.Null(userService);
    }
    
    /// <summary>
    /// Регистрация User
    /// </summary>
    /// <param name="registrationRequest">Данные для регистрации.</param>
    /// <param name="avatarFile">Файл аватара.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый User.</returns>
    [HttpPost("register")]
    public async Task<ActionResult> Register(
        [FromForm] RegistrationRequest registrationRequest,
        IFormFile avatarFile, 
        CancellationToken cancellationToken)
    {
        registrationRequest.Password = BCrypt.Net.BCrypt.HashPassword(registrationRequest.Password);

        var stream = avatarFile.OpenReadStream();
        {
            var registrationResponse = await _userService.RegisterAsync(
                registrationRequest,
                stream,
                avatarFile.FileName,
                avatarFile.ContentType,
                cancellationToken);

            return Ok(registrationResponse);
        }
    }
    
    /// <summary>
    /// Обновление User
    /// </summary>
    /// <param name="userRequest">Данные для обновления.</param>
    /// <param name="avatarFile">Аватар.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный User.</returns>
    [Authorize]
    [HttpPut("update")]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUser(
        [FromForm] UpdateUserRequest userRequest,
        IFormFile avatarFile,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userRequest.Id != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете обновлять данные другого пользователя.");
        
        var stream = avatarFile.OpenReadStream();
        {
            var updateResponse = await _userService.UpdateAsync(
                userRequest,
                stream,
                avatarFile.FileName,
                avatarFile.ContentType,
                cancellationToken);

            return Ok(updateResponse);
        }
    }

    /// <summary>
    /// Удаление User
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpDelete("delete/{userId:guid}")]
    public async Task<ActionResult> DeleteUser(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    { 
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете удалить другого пользователя.");
        
        await _userService.DeleteAsync(Guid.Parse(userIdClaim), cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Вход в аккаунт
    /// </summary>
    /// <param name="loginRequest">Данные для входа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Токен.</returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromForm] LoginRequest loginRequest,
        CancellationToken cancellationToken)
    {
        var loginResponse = await _userService.LoginAsync(loginRequest, cancellationToken);
        return Ok(loginResponse);
    }

    /// <summary>
    /// Привязка телеграмма
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="telegramId">Идентификатор телеграмма.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPut("set/telegram/{userId:guid}/{telegramId}")]
    [Authorize]
    public async Task<ActionResult> SetTelegram(
        [FromRoute] Guid userId,
        [FromRoute] string telegramId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете привязать телеграм к другому пользователю.");
        
        
        await _userService.SetTelegramId(telegramId, Guid.Parse(userIdClaim), cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Отвязка телеграмма
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPut("unlink/telegram/{userId:guid}")]
    [Authorize]
    public async Task<ActionResult> UnlinkTelegram(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете отвязать телеграм от другого пользователя.");
        
        await _userService.UnlinkTelegram(Guid.Parse(userIdClaim), cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Обновление настроек User
    /// </summary>
    /// <param name="userRequest">Данные для обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновление User.</returns>
    [HttpPut("update/settings")]
    [Authorize]
    public async Task<ActionResult<UpdateSettingsUserResponse>> UpdateSettingsUserSettings(
        [FromForm] UpdateSettingsUserRequest userRequest,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userRequest.Id != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете изменить настройки другого пользователя.");
        
        var updateResponse = await _userService.UpdateSettings(userRequest, cancellationToken);
        return Ok(updateResponse);
    }
    
    /// <summary>
    /// Получение User по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>User.</returns>
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<GetByIdUserResponse>> GetByIdUser(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(userId, cancellationToken);
        return Ok(user);
    }
}