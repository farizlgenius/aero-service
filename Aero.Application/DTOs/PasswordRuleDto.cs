namespace Aero.Application.DTOs;

public sealed record PasswordRuleDto(int Len, bool IsLower, bool IsUpper, bool IsDigit, bool IsSymbol, List<string> Weaks);
