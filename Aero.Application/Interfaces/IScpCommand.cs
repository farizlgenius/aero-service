using System;

namespace Aero.Application.Interfaces;

public interface IScpCommand 
{
      bool DetachScp(short component);
      bool ResetScp(short component);
}
