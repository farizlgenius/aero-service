using System;
using System.IO.Compression;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class AccessLevelMapper
{
      public static CreateUpdateAccessLevel ToCreateDomain(CreateUpdateAccessLevelDto dto)
      {
            var res = new CreateUpdateAccessLevel();
            // Base 
            NoMacBaseMapper.ToDomain(dto,res);
            res.Name = dto.Name;
            res.CreateUpdateAccessLevelDoorTimeZone = dto.CreateUpdateAccessLevelDoorTimeZoneDto.Select(x => new CreateUpdateAccessLevelDoorTimeZone
            {
                  DoorId = x.DoorId,
                  AcrId = x.AcrId,
                  DoorMac = x.DoorMac,
                  TimezoneId = x.TimezoneId
            }).ToList();

            return res;

      }

}
