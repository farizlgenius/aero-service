using System;

namespace Aero.Application.Interfaces;

public interface IJwtSettings
{
      string Secret {get;}
      string Issuer {get;}
      string Audience {get;}
      short AccessTokenMinute {get;}
}
