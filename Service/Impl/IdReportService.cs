using HIDAeroService.Aero.CommandService;
using HIDAeroService.Aero.CommandService.Impl;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.IdReport;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HIDAeroService.Service.Impl
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
