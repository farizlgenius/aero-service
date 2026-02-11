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
            res.Components = dto.Components.Select(x => new CreateUpdateAccessLevelComponent
            {
                  Mac = x.Mac,
                  DoorComponents = x.DoorComponent.Select(x => new CreateUpdateAccessLevelDoorComponent
                  {
                      DoorId = x.DoorId,
                        AcrId = x.AcrId,
                        TimezoneId = x.TimezoneId
                  }).ToList()
            }).ToList();

            return res;

      }

}
