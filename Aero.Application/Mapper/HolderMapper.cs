using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class HolderMapper
{
      public static CardHolder ToDomain(CardHolderDto dto)
      {
            var res = new CardHolder();
            // Base 
            NoMacBaseMapper.ToDomain(dto,res);
            res.UserId  = dto.UserId;
            res.Title = dto.Title;
            res.FirstName = dto.FirstName;
            res.MiddleName = dto.MiddleName;
            res.LastName = dto.LastName;
            res.Sex = dto.Sex;
            res.Email = dto.Email;
            res.Phone = dto.Phone;
            res.Company = dto.Company;
            res.Position = dto.Position;
            res.Department = dto.Department;
            res.ImagePath = dto.ImagePath;
            res.Flag = dto.Flag;
            res.Additionals = dto.Additionals;
            res.Credentials = dto.Credentials is null || dto.Credentials.Count <= 0 ? new List<Credential>() : dto.Credentials.Select(x => new Aero.Domain.Entities.Credential
            {
                  // Base 
                  ComponentId = x.ComponentId,
                  HardwareName = x.HardwareName,
                  LocationId = x.LocationId,
                  IsActive = true,
                  Bits = x.Bits,
                  IssueCode = x.IssueCode,
                  FacilityCode = x.FacilityCode,
                  CardNo = x.CardNo,
                  Pin = x.Pin,
                  ActiveDate = x.ActiveDate,
                  DeactiveDate = x.DeactiveDate
            }).ToList();
            res.AccessLevels = dto.AccessLevels is null || dto.AccessLevels.Count <= 0 ? new List<AccessLevel>() : dto.AccessLevels.Select(x => new AccessLevel
            {
                  Name = x.Name,
                  ComponentId = x.ComponentId,
                  HardwareName = x.HardwareName,
                  LocationId = x.LocationId,
                  IsActive = x.IsActive,
                  Components = x.Components.Select(x => new CreateUpdateAccessLevelComponent
                  {
                        Mac = x.Mac,
                        DoorComponents = x.DoorComponent.Select(x => new CreateUpdateAccessLevelDoorComponent
                        {
                            DoorId = x.DoorId,
                              AcrId = x.AcrId,
                              TimezoneId = x.TimezoneId
                        }).ToList()
                  }).ToList()
            }).ToList();

            return res;

      } 

}
