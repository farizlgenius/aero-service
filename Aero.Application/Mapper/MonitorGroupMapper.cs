using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class MonitorGroupMapper
{
      public static MonitorGroup ToDomain(MonitorGroupDto dto)
      {
            var res = new MonitorGroup();
            // Base 
            MacBaseMapper.ToDomain(dto,res);
            res.Name = dto.Name;
            res.nMpCount = dto.nMpCount;
            res.nMpList = dto.nMpList.Select(x => new MonitorGroupList
            {
                  PointNumber = x.PointNumber,
                  PointType = x.PointType,
                  PointTypeDesc = x.PointTypeDesc
            }).ToList();

            return res;

      }

}
