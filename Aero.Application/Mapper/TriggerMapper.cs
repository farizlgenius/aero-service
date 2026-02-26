using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class TriggerMapper
{
      public static Trigger ToDomain(TriggerDto dto)
      {
            var res = new Trigger();
            MacBaseMapper.ToDomain(dto, res);
            res.DriverID = dto.TranType;
            res.Name = dto.Name;
            res.Command = dto.Command;
            res.ProcedureId = dto.ProcedureId;
            res.SourceType = dto.SourceType;
            res.SourceNumber = dto.SourceNumber;
            res.TranType = dto.TranType;
            res.CodeMap = dto.CodeMap.Select(x => new TransactionCode
            {
                  Name = x.Name,
                  Value = x.Value,
                  Description = x.Description,

            }).ToList();
            res.TimeZone = dto.TimeZone;

            return res;

      }

}
