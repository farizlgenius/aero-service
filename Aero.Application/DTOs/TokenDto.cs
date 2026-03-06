namespace Aero.Application.DTOs;

public sealed record TokenDto(DateTime TimeStamp, string AccessToken, int ExpireInMinute);
