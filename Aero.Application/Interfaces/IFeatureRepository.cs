using Aero.Application.DTOs;
using Aero.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface IFeatureRepository : IBaseRepository<FeatureDto,Feature>
    {
        Task<IEnumerable<FeatureDto>> GetFeatureByRoleAsync(short RoleId);
        Task<FeatureDto> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId);
    }
}
