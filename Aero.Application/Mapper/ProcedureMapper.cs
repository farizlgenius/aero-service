using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class ProcedureMapper
{
      public static Procedure ToDomain(ProcedureDto dto)
      {
            var res = new Procedure();
            MacBaseMapper.ToDomain(dto,res);
            res.ProcId = dto.ProcId;
            res.Name = dto.Name;
            res.Actions = dto.Actions
            .Select(x => new Aero.Domain.Entities.Action
            {
                  // Base 
                  ComponentId = x.ComponentId,
                  HardwareName = x.HardwareName,
                  Mac = x.Mac,
                  LocationId = x.LocationId,
                  IsActive = true,

                  ScpId = x.ScpId,
                  ActionType = x.ActionType,
                  ActionTypeDesc = x.ActionTypeDesc,
                  Arg1 = x.Arg1,
                  Arg2 = x.Arg2,
                  Arg3 = x.Arg3,
                  Arg4 = x.Arg4,
                  Arg5 = x.Arg5,
                  Arg6 = x.Arg6,
                  Arg7 = x.Arg7,
                  StrArg = x.StrArg,
                  DelayTime = x.DelayTime
            })
            .ToList();

            return res;
      }

}
