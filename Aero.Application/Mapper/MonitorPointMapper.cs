using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class MonitorPointMapper
{
      public static MonitorPoint ToDomain(MonitorPointDto dto)
      {
            var res = new MonitorPoint();
            MacBaseMapper.ToDomain(dto,res);
            res.DriverId = dto.MpId;
            res.Name = dto.Name;
            res.ModuleId = dto.ModuleId;
            res.ModuleDescription = dto.ModuleDescription;
            res.InputNo = dto.InputNo;
            res.InputMode = dto.InputMode;
            res.InputModeDetail = dto.InputModeDescription;
            res.Debounce = dto.Debounce;
            res.HoldTime = dto.HoldTime;
            res.LogFunction = dto.LogFunction;
            res.LogFunctionDetail = dto.LogFunctionDescription;
            res.MonitorPointMode = dto.MonitorPointMode;
            res.MonitorPointModeDetail = dto.MonitorPointModeDescription;
            res.DelayEntry = dto.DelayEntry;
            res.DelayExit = dto.DelayExit;
            res.IsMask = dto.IsMask;

            return res;
      }

}
