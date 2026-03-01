using System;

namespace Aero.Application.DTOs;

public sealed record TokenDtoWithRefresh(DateTime TimeStamp, string AccessToken, string RefreshToken, int ExpireInMinute);
