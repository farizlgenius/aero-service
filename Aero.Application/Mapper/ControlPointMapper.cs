using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class ControlPointMapper
{
      public static ControlPoint ToDomain(ControlPointDto dto)
      {
            var res = new ControlPoint();
            // Base 
            MacBaseMapper.ToDomain(dto,res);
            res.DriverId = dto.CpId;
            res.Name = dto.Name;
            res.ModuleId = dto.ModuleId;
            res.OutputNo = dto.OutputNo;
            res.RelayMode = dto.RelayMode;
            res.RelayModeDescription  =dto.RelayModeDescription;
            res.OfflineMode = dto.OfflineMode;
            res.OfflineModeDescription = dto.OfflineModeDescription;
            res.DefaultPulse = dto.DefaultPulse;

            return res;

      }

}
