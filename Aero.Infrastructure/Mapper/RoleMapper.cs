using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class RoleMapper
{
      public static Aero.Infrastructure.Data.Entities.Role ToEf(Aero.Domain.Entities.Role data)
      {
            var res = new Aero.Infrastructure.Data.Entities.Role();
            res.component_id = data.ComponentId;
            res.name = data.Name;
            res.feature_roles = data.Features.Select(x => new FeatureRole
            {
                  feature_id = x.ComponentId,
                  role_id = data.ComponentId,
                  is_allow = x.IsAllow,
                  is_create = x.IsCreate,
                  is_modify = x.IsModify,
                  is_delete = x.IsDelete,
                  is_action = x.IsAction
            }).ToList();

            return res;
      }
      public static void Update(Aero.Domain.Entities.Role data,Aero.Infrastructure.Data.Entities.Role res)
      {
            res.component_id = data.ComponentId;
            res.name = data.Name;
            res.feature_roles = data.Features.Select(x => new FeatureRole
            {
                  feature_id = x.ComponentId,
                  role_id = data.ComponentId,
                  is_allow = x.IsAllow,
                  is_create = x.IsCreate,
                  is_modify = x.IsModify,
                  is_delete = x.IsDelete,
                  is_action = x.IsAction
            }).ToList();
      }

}
