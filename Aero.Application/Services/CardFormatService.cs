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

    public class CardFormatService(IQCfmtRepository qCfmt,IQHwRepository qHw,ICfmtCommand cfmt,ICfmtRepository rCfmt) : ICardFormatService
    {

        public async Task<ResponseDto<IEnumerable<CardFormatDto>>> GetAsync()
        {
            var dtos = await qCfmt.GetAsync();    
            return ResponseHelper.SuccessBuilder<IEnumerable<CardFormatDto>>(dtos);
        }

        public async Task<ResponseDto<CardFormatDto>> GetByComponentIdAsync(short ComponentId)
        {
            var dto = await qCfmt.GetByComponentIdAsync(ComponentId);
            return ResponseHelper.SuccessBuilder(dto);

        }

        public async Task<ResponseDto<bool>> CreateAsync(CardFormatDto dto)
        {
            List<string> errors = new List<string>();
            var ComponentId = await qCfmt.GetLowestUnassignedNumberAsync(10,"");
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var domain = CardFormatMapper.ToDomain(dto); 

            var macs = await qHw.GetMacsAsync();
            foreach (var mac in macs)
            {
                var id = await qHw.GetComponentIdFromMacAsync(mac);
                if (!cfmt.CardFormatterConfiguration(id, domain, 1))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CARDFORMAT_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            var status = await rCfmt.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            List<string> errors = new List<string>();
            if (!await qCfmt.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();

            var data = await qCfmt.GetByComponentIdAsync(component);

            var domain = CardFormatMapper.ToDomain(data);
            var macs = await qHw.GetMacsAsync();
            foreach (var mac in macs)
            {
                var ScpId = await qHw.GetComponentIdFromMacAsync(mac);
                 if (!cfmt.CardFormatterConfiguration(ScpId, domain, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CARDFORMAT_CONFIG));
                }

            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>( ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rCfmt.DeleteByComponentIdAsync(component);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<CardFormatDto>> UpdateAsync(CardFormatDto dto)
        {
            List<string> errors = new List<string>();
            if (!await qCfmt.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<CardFormatDto>();
            var macs = await qHw.GetMacsAsync();
            var domain = CardFormatMapper.ToDomain(dto);
            foreach (var mac in macs)
            {
                var id = await qHw.GetComponentIdFromMacAsync(mac);
                if (!cfmt.CardFormatterConfiguration(id, domain, 1))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac,Command.CARDFORMAT_CONFIG));
                }

            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            
            var status = await rCfmt.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]); 

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<CardFormatDto>>> GetByLocationIdAsync(short location)
        {
            var res = await qCfmt.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<Pagination<CardFormatDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await qCfmt.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
