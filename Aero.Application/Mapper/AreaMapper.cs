using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class AreaMapper
{
      public static AccessArea ToDomain(AccessAreaDto dto)
      {
            var res = new AccessArea();
            // Base 
            NoMacBaseMapper.ToDomain(dto,res);
            res.Name = dto.Name;
            res.MultiOccupancy = dto.MultiOccupancy;
            res.AccessControl = dto.AccessControl;
            res.OccControl = dto.OccControl;
            res.OccSet = dto.OccSet;
            res.OccMax = dto.OccMax;
            res.OccUp = dto.OccUp;
            res.OccDown = dto.OccDown;
            res.AreaFlag = dto.AreaFlag;

            return res;
      }
}
