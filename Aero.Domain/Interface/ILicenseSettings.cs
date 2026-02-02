using System;

namespace Aero.Domain.Interfaces;

public interface ILicenseSettings
{
      string Secret {get;}      
      string LicenseServerUrl {get;}
}
