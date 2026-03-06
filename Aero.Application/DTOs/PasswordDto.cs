namespace Aero.Application.DTOs;

public sealed record PasswordDto(string Username, string Old, string New, string Con);
