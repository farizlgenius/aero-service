using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.AeroLibrary;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.IdReport;
using AeroService.Helpers;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AeroService.Service.Impl
{
    public class IdReportService(AeroCommandService command, AeroMessage read, AppDbContext context, ILogger<IdReportService> logger)
    {

        public async Task<ResponseDto<IEnumerable<IdReportDto>>> GetAsync()
        {

            var dtos = await context.id_report
                .AsNoTracking()
                .Select(data => MapperHelper.IdReportToDto(data))
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<IdReportDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(short id)
        {

            List<IdReportDto> dtos = new List<IdReportDto>();
            if (!command.GetIdReport(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C401));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<int>> GetCount()
        {
            List<string> errors = new List<string>();
            int count = 0;
            count = await context.id_report.CountAsync();
            return ResponseHelper.SuccessBuilder(count);

        }


    }
}
