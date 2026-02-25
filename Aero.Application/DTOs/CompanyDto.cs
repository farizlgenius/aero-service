using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed record CompanyDto(
        string Name,
        string Description,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
    
}
