using System;
using System.IO.Compression;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class AccessLevelMapper
{
      public static AccessLevel ToDomain(AccessLevelDto dto)
      {
            var res = new AccessLevel();
            // Base 
            NoMacBaseMapper.ToDomain(dto,res);
            res.Name = dto.Name;
            res.Components = dto.Components.Select(x => new AccessLevelComponent
            {
                AlvlId = x.AlvlId,
                  Mac = x.Mac,
                DoorId = x.DoorId,
                AcrId = x.AcrId,
                TimezoneId = x.TimezoneId
            }).ToList();

            return res;

      }

}
