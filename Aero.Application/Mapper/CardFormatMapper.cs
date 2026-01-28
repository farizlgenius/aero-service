using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class CardFormatMapper
{
      public static CardFormat ToDomain(CardFormatDto dto)
      {
            var res = new CardFormat();
            // Base 
            NoMacBaseMapper.ToDomain(dto,res);
            res.Name = dto.Name;
            res.Facility = dto.Facility;
            res.Offset = dto.Offset;
            res.FunctionId = dto.FunctionId;
            res.Flags = dto.Flags;
            res.Bits = dto.Bits;
            res.PeLn = dto.PeLn;
            res.PeLoc = dto.PeLoc;
            res.PoLn = dto.PoLn;
            res.PoLoc = dto.PoLoc;
            res.FcLn = dto.FcLn;
            res.FcLoc = dto.FcLoc;
            res.ChLn = dto.ChLn;
            res.ChLoc = dto.ChLoc;
            res.IcLn = dto.IcLn;
            res.IcLoc = dto.IcLoc;

            return res;

      }
}
