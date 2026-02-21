using System;
using Aero.Domain.Entities;

namespace Aero.Application.Interface;

public interface ITrigCommand 
{
      bool TriggerSpecification(short ScpId, Trigger data, short ComponentId);
}
