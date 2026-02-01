using System;

namespace Aero.Application.DTOs;

public sealed class TokenDtoWithRefresh
{
              public DateTime TimeStamp { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpireInMinute {  get; set; }

}
