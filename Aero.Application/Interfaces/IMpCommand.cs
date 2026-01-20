using System;

namespace Aero.Application.Interfaces;

public interface IMpCommand
{
  bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime);
}
