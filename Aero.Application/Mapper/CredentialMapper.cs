using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class CredentialMapper
{
      public static Credential ToDomain(CredentialDto dto)
      {
            var res = new Credential();
            // Base 
            NoMacBaseMapper.ToDomain(dto,res);
            res.Bits = dto.Bits;
            res.IssueCode = dto.IssueCode;
            res.FacilityCode = dto.FacilityCode;
            res.CardNo = dto.CardNo;
            res.Pin = dto.Pin;
            res.ActiveDate = dto.ActiveDate;
            res.DeactiveDate = dto.DeactiveDate;

            return res;
      }

}
