using AutoMapper;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Utility;
using HIDAeroService.Helpers;
using HIDAeroService.Constant;
using HIDAeroService.DTO;
using HIDAeroService.DTO.CardFormat;
using HIDAeroService.Mapper;

namespace HIDAeroService.Service.Impl
{

    public class CardFormatService(AppDbContext context, IMapper mapper, ILogger<CardFormatService> logger, AeroCommand command, IHelperService<CardFormat> helperService) : ICardFormatService
    {

        public virtual async Task<ResponseDto<IEnumerable<CardFormatDto>>> GetAsync()
        {
            var dtos = await context.CardFormats
                .AsNoTracking()
                .Select(x => MapperHelper.CardFormatToDto(x))
                .ToArrayAsync();
            
            return ResponseHelper.SuccessBuilder<IEnumerable<CardFormatDto>>(dtos);
        }

        public async Task<ResponseDto<CardFormatDto>> GetByComponentIdAsync(short ComponentId)
        {
            var dto = await context.CardFormats
                .Where(x => x.ComponentId == ComponentId)
                .Select(x => MapperHelper.CardFormatToDto(x))
                .FirstOrDefaultAsync();
            return ResponseHelper.SuccessBuilder(dto);

        }

        public async Task<ResponseDto<bool>> CreateAsync(CardFormatDto dto)
        {
            List<string> errors = new List<string>();
            var componentNo = await helperService.GetLowestUnassignedNumberNoLimitAsync<CardFormat>(context);
            if (componentNo == -1) return ResponseHelper.ExceedLimit<bool>();
            List<short> ScpIds = await context.Hardwares.Select(x => x.ComponentId).ToListAsync();
            //foreach (var id in ScpIds)
            //{
            //    string mac = await helperService.GetMacFromIdAsync(id);
            //    if (!await command.CardFormatterConfigurationAsync(id, dto, 1))
            //    {
            //        errors.Add(MessageBuilder.Unsuccess(mac,Command.C1102));
            //    }
            //}
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var entity = MapperHelper.DtoToCardFormat(dto,componentNo,DateTime.Now); 
            await context.CardFormats.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            List<string> errors = new List<string>();
            var entity = await context.CardFormats.Where(x => x.ComponentId == component).FirstOrDefaultAsync();
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            var dto = MapperHelper.CardFormatToDto(entity);
            List<short> scpIds = await context.Hardwares.Select(x => x.ComponentId).ToListAsync();
            //foreach (var id in scpIds)
            //{
            //    string mac = await helperService.GetMacFromIdAsync(id);
            //    if (!await command.CardFormatterConfigurationAsync(id, dto, 0))
            //    {
            //        errors.Add(MessageBuilder.Unsuccess(mac,Command.C1102));
            //    }

            //}
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>( ResponseMessage.COMMAND_UNSUCCESS, errors);
            context.CardFormats.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<CardFormatDto>> UpdateAsync(CardFormatDto dto)
        {
            List<string> errors = new List<string>();
            var entity = await context.CardFormats.Where(x => x.ComponentId == dto.ComponentId).FirstOrDefaultAsync();
            if (entity is null) return ResponseHelper.NotFoundBuilder<CardFormatDto>();
            List<short> ids = await context.Hardwares.Select(x => x.ComponentId).ToListAsync();
            foreach (var id in ids)
            {
                string mac = await helperService.GetMacFromIdAsync(id);
                if (!await command.CardFormatterConfigurationAsync(id, dto, 1))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac,Command.C1102));
                }

            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardFormatDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            MapperHelper.UpdateCardFormat(entity,dto);
            context.CardFormats.Update(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }

    }
}
