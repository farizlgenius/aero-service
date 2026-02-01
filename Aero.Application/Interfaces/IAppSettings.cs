using System;

namespace Aero.Application.Interfaces;

public interface IAppSettings
{
      string Secret {get;}
      short MaxCardFormat {get;}
      short AeroPort {get;}
      short ConnectionType {get;}
      short nChannelId {get;}
      short nPort {get;}
      string LicenseServerUrl {get;}
      IApiEndpoints ApiEndpoints {get;}
}
