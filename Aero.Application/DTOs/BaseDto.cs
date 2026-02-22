using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public record BaseDto(
        int LocationId,
        bool IsActive
        );
    

}
