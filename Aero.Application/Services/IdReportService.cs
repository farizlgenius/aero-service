using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;

namespace Aero.Application.Services
{
    public class IdReportService(IIdReportRepository repo,IScpCommand scp) 
    {

        public async Task<ResponseDto<IEnumerable<IdReportDto>>> GetAsync(short location)
        {

            var dtos = await repo.GetAsync(location);
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
            int count = await repo.GetCountAsync(location);
            return ResponseHelper.SuccessBuilder(count);

        }


    }
}
