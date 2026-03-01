using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Services
{
    public sealed class DiagnosticService(IDiagRepository repository,ICmndRepository cmnd) : IDiagnosticService
    {
        public async Task<ResponseDto<bool>> CommandAsync(CommandDto command)
        {
            var res = repository.CommandAsync(command.Command);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<Pagination<CommandAudit>>> GetCommandStatusAsync(PaginationParamsWithFilter pagination, short location)
        {
            var res = await cmnd.GetCommandStatusAsync(pagination, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
