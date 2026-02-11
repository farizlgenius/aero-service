using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;

namespace Aero.Application.Services
{
    public class IdReportService(IQIdReportRepository qReport,IScpCommand scp) 
    {

        public async Task<ResponseDto<IEnumerable<IdReportDto>>> GetAsync(short location)
        {

            var dtos = await qReport.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<IdReportDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(short id)
        {

            List<IdReportDto> dtos = new List<IdReportDto>();
            if (!scp.GetIdReport(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C401));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<int>> GetCount(short location)
        {
            int count = await qReport.GetCountAsync(location);
            return ResponseHelper.SuccessBuilder(count);

        }


    }
}
