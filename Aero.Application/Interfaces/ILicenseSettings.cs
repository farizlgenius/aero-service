using System;

namespace Aero.Application.Interfaces;

public interface ILicenseSettings
{
      string Secret {get;}      
      string LicenseServerUrl {get;}
}
