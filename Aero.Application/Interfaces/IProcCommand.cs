using System;

namespace Aero.Domain.Interface;

public interface IProcCommand
{
      bool ActionSpecificationAsyncForAllHW(short ComponentId, Aero.Domain.Entities.Action action, List<short> ScpIds);
      bool ActionSpecificationDelayAsync(short ComponentId, Aero.Domain.Entities.Action action);
      bool ActionSpecificationAsync(short ComponentId, List<Aero.Domain.Entities.Action> en);
}
