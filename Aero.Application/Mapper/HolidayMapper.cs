using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class HolidayMapper
{
      public static Holiday ToDomain(HolidayDto dto)
      {
            var res = new Holiday();
            // Base
            NoMacBaseMapper.ToDomain(dto,res);
            res.Year = dto.Year;
            res.Month = dto.Month;
            res.Day = dto.Day;
            res.Extend = dto.Extend;
            res.TypeMask = dto.TypeMask;

            return res;
      }

}
