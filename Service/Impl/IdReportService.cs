using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.IdReport;
using HIDAeroService.Helpers;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HIDAeroService.Service.Impl
{
    public class IdReportService(AeroCommand command, AeroMessage read, AppDbContext context, ILogger<IdReportService> logger)
    {

        public async Task<ResponseDto<IEnumerable<IDReportDto>>> GetAsync()
        {

            List<IDReportDto> dtos = new List<IDReportDto>();
            var datas = read.iDReports;
            foreach (var data in datas)
            {
                if (!await context.Hardwares.AnyAsync(x => x.MacAddress == data.MacAddress))
                {
                    dtos.Add(
                        new IDReportDto 
                        {
                            DeviceId = data.DeviceId,
                            SerialNumber = data.SerialNumber,
                            ScpId = data.ScpId,
                            ConfigFlag = data.ConfigFlag,
                            MacAddress = data.MacAddress,
                            Ip = data.Ip,
                            Port = (short)data.Port,
                            Model = data.Model
                        }
                    );
                }

            }
            return ResponseHelper.SuccessBuilder<IEnumerable<IDReportDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(short id)
        {

            List<IDReportDto> dtos = new List<IDReportDto>();
            if (!await command.GetIdReportAsync(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C401));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<int>> GetCount()
        {
            List<string> errors = new List<string>();
            int count = 0;
            if (read.iDReports.Count != 0)
            {
                count = read.iDReports.Count;
            }
            return ResponseHelper.SuccessBuilder(count);

        }


    }
}
