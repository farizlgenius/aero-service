using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class RoleMapper
{
      public static Aero.Domain.Entities.Role ToDomain(RoleDto dto)
      {
            var res = new Aero.Domain.Entities.Role();
            res.ComponentId = dto.ComponentId;
            res.Name = dto.Name;
            res.Features = dto.Features.Select(f => new Aero.Domain.Entities.Feature
            {
                  ComponentId = f.ComponentId,
                  Name = f.Name,
                  Path = f.Path,
                  SubItems = f.SubItems.Select(s => new SubFeature
                  {
                       Name = s.Name,
                       Path=s.Path 
                  }).ToList(),
                  IsAllow = f.IsAllow,
                  IsCreate = f.IsCreate,
                  IsModify = f.IsModify,
                  IsDelete = f.IsDelete,
                  IsAction = f.IsAction
            }).ToList();

            return res;
      }

}
