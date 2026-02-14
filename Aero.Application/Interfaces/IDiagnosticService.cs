using Aero.Application.DTOs;
using Aero.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface IDiagnosticService
    {
        Task<ResponseDto<bool>> CommandAsync(CommandDto command);
        Task<ResponseDto<Pagination<CommandAudit>>> GetCommandStatusAsync(PaginationParamsWithFilter pagination,short location);
    }
}
