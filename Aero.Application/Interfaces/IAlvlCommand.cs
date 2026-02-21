using System;
using Aero.Domain.Entities;


namespace Aero.Application.Interfaces;

public interface IAlvlCommand
{
    Task<bool> AccessLevelConfigurationExtended(short ScpId, short component, short tzAcr);
    Task<bool> AccessLevelConfigurationExtended(short ScpId,short number, AccessLevel data);
    Task<bool> AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<AccessLevelComponent> component);
}
