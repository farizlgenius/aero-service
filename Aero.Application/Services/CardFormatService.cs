using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{

    public class CardFormatService(IHwRepository hw,ICfmtCommand cfmt,ICfmtRepository repo,ISettingRepository setting) : ICardFormatService
    {

        public async Task<ResponseDto<IEnumerable<CardFormatDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();    
            return ResponseHelper.SuccessBuilder<IEnumerable<CardFormatDto>>(dtos);
        }

        public async Task<ResponseDto<CardFormatDto>> GetByIdAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);
            return ResponseHelper.SuccessBuilder(dto);

        }

        public async Task<ResponseDto<CardFormatDto>> CreateAsync(CardFormatDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<CardFormatDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            List<string> errors = new List<string>();
            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nCfmt);
            if (DriverId == -1) return ResponseHelper.ExceedLimit<CardFormatDto>();

            var domain = new CardFormat(DriverId, dto.Name,dto.Fac,dto.Offset,dto.FuncId,dto.Flag,dto.Bits,dto.PeLn,dto.PeLoc,dto.PoLn,dto.PoLoc,dto.FcLn,dto.FcLoc,dto.ChLn,dto.ChLoc,dto.IcLn,dto.IcLoc);

            var macs = await hw.GetMacsAsync();
            foreach (var mac in macs)
            {
                var id = await hw.GetComponentIdFromMacAsync(mac);
                if (! await cfmt.CardFormatterConfiguration(id, domain, 1))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CARDFORMAT_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<CardFormatDto>> DeleteAsync(int id)
        {
            List<string> errors = new List<string>();

            var en = await repo.GetByIdAsync(id);

            if(en is null) return ResponseHelper.NotFoundBuilder<CardFormatDto>();

            var domain = new CardFormat(en.DriverId,en.Name,en.Fac,en.Offset,en.FuncId,en.Flag,en.Bits,en.PeLn,en.PeLoc,en.PoLn,en.PoLoc,en.FcLn,en.FcLoc,en.ChLn,en.ChLoc,en.IcLn,en.IcLoc);

            var macs = await hw.GetMacsAsync();
            foreach (var mac in macs)
            {
                var ScpId = await hw.GetComponentIdFromMacAsync(mac);
                 if (!await cfmt.CardFormatterConfiguration(ScpId, domain, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CARDFORMAT_CONFIG));
                }

            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>( ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(en);
        }

        public async Task<ResponseDto<CardFormatDto>> UpdateAsync(CardFormatDto dto)
        {
            List<string> errors = new List<string>();
            if (!await repo.IsAnyById(dto.Id)) return ResponseHelper.NotFoundBuilder<CardFormatDto>();
            var macs = await hw.GetMacsAsync();
            var domain = CardFormatMapper.ToDomain(dto);
            foreach (var mac in macs)
            {
                var id = await hw.GetComponentIdFromMacAsync(mac);
                if (!await  cfmt.CardFormatterConfiguration(id, domain, 1))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac,Command.CARDFORMAT_CONFIG));
                }

            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            
            var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]); 

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<CardFormatDto>>> GetByLocationIdAsync(int location)
        {
            var res = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<Pagination<CardFormatDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
