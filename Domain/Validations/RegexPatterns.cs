using System.Text.RegularExpressions;

namespace Domain.Validations;

/// <summary>
/// Класс условных выражений
/// </summary>
public static class RegexPatterns
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    public static readonly Regex EmailPattern = new (@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
}