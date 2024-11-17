namespace Domain.Validations;

/// <summary>
/// Класс сообщений для валидаций
/// </summary>
public static class ValidationMessages
{
    public const string NullError = "{PropertyName} не может быть null";
    public const string EmptyError = "{PropertyName} не может быть пустым";
    public const string MinimumLengthError = "{PropertyName} слишком короткий";
    public const string MaximumLengthError = "{PropertyName} слишком длинный";
    public const string NegativeNumberError = "{PropertyName} не может быть отрицательным";
    public const string EmailError = "Неверный формат электронной почты. Пример: example@gmail.com";
    public const string DateError = "Дата не может быть в будущем";
    public const string EnumError = "{PropertyName} не является одним из возможных элементов перечисления";
}