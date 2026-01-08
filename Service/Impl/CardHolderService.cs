using HIDAeroService.Aero.CommandService;
using HIDAeroService.Aero.CommandService.Impl;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessLevel;
using HIDAeroService.DTO.CardHolder;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;
using MiNET.Utils.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HIDAeroService.Service.Impl
{
    public sealed class CardHolderService(AppDbContext context,IHelperService<Credential> helperService,AeroCommandService command,ICredentialService credentialService) : ICardHolderService
    {
        public async Task<ResponseDto<bool>> CreateAsync(CardHolderDto dto)
        {
            if (await context.cardholder.AnyAsync(x => dto.UserId == x.user_id)) return ResponseHelper.Duplicate<bool>();
            List<short> CredentialComponentId = new List<short>();
            List<string> errors = new List<string>();
            // Send data 
            var ScpIds = await context.hardware.Select(x => new { x.component_id,x.mac }).ToArrayAsync();

            var entity = MapperHelper.DtoToCardHolder(dto, CredentialComponentId, DateTime.Now);

            foreach (var cred in entity.credentials)
            {
                CredentialComponentId.Add(await helperService.GetLowestUnassignedNumberNoLimitAsync<Credential>(context));
                cred.issue_code = await credentialService.GetLowestUnassignedIssueCodeAsync(userId:dto.UserId);

                foreach (var id in ScpIds)
                {
                    if (!command.AccessDatabaseCardRecord(id.component_id, dto.Flag,cred.card_no,cred.issue_code,cred.pin, entity.access_levels.Select(x => x.access_level).ToList(), (int)helperService.DateTimeToElapeSecond(cred.active_date), (int)helperService.DateTimeToElapeSecond(cred.deactive_date)))
                    {
                        errors.Add(MessageBuilder.Unsuccess(id.mac, Command.C8304));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);


            await context.cardholder.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string UserId)
        {
            List<string> errors = new List<string>();
            var entity = await context.cardholder
                .AsNoTracking()
                .Include(x => x.additional)
                .Include(x => x.credentials)
                .ThenInclude(x => x.hardware_credentials)
                .Where(x => x.user_id == UserId)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            var Macs = entity.credentials
                .SelectMany(x => x.hardware_credentials.Select(x => x.hardware_mac))
                .Distinct()
                .ToArray();

            foreach(var mac in Macs)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                foreach(var cred in entity.credentials)
                {
                    if (!command.CardDelete(ScpId, cred.card_no))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac,Command.C3305));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.cardholder.Remove(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);


        }
        
        public async Task<ResponseDto<IEnumerable<CardHolderDto>>> GetAsync()
        {
            var dtos = await context.cardholder
                .Include(x => x.additional)
                .Include(x => x.credentials)
                .Include(x => x.access_levels)
                .ThenInclude(x => x.access_level)
                .Select(x => MapperHelper.CardHolderToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<CardHolderDto>>(dtos);

        }

        public async Task<ResponseDto<IEnumerable<CardHolderDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await context.cardholder
                .Include(x => x.additional)
                .Include(x => x.credentials)
                .Include(x => x.access_levels)
                .ThenInclude(x => x.access_level)
                .Where(x => x.location_id == location)
                .Select(x => MapperHelper.CardHolderToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<CardHolderDto>>(dtos);

        }

        public async Task<ResponseDto<CardHolderDto>> GetByUserIdAsync(string UserId)
        {
            var dto = await context.cardholder
                .Include(x => x.additional)
                .Include(x => x.credentials)
                .Include(x => x.access_levels)
                .ThenInclude(x => x.access_level)
                .Select(x => MapperHelper.CardHolderToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<CardHolderDto>> UpdateAsync(CardHolderDto dto)
        {
            List<string> errors = new List<string>();
            var entity = await context.cardholder
                .Include(x => x.access_levels)
                .ThenInclude(x => x.access_level)
                .Include(x => x.additional)
                .Include(x => x.credentials)
                .Where(x => x.user_id == dto.UserId)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<CardHolderDto>();
            List<short> CredentialComponentId = new List<short>();

            context.cardholder_additional.RemoveRange(entity.additional);
            context.cardholder_accesslevel.RemoveRange(entity.access_levels);

            for(int i = 0;i < dto.Credentials.Count(); i++)
            {
                CredentialComponentId.Add(await helperService.GetLowestUnassignedNumberNoLimitAsync<Credential>(context));
            }

            MapperHelper.UpdateCardHolder(entity, dto, CredentialComponentId);

            // DeleteAsync Old 
            var macs = entity.credentials.SelectMany(x => x.hardware_credentials.Select(x => x.hardware_mac)).ToList();
            foreach(var mac in macs)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                foreach (var cred in entity.credentials)
                {
                    if (!command.CardDelete(ScpId, cred.card_no))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C3305));
                    }
                }
            }


            // Send data 
            var ScpIds = await context.hardware.Select(x => new { x.component_id, x.mac }).ToArrayAsync();
            foreach (var cred in entity.credentials)
            {     
                foreach (var id in ScpIds)
                {
                    if (!command.AccessDatabaseCardRecord(id.component_id, dto.Flag, cred.card_no, cred.issue_code,string.IsNullOrEmpty(cred.pin) ? "" : cred.pin, entity.access_levels is null ? new List<AccessLevel>() : entity.access_levels.Select(x => x.access_level).ToList(), (int)helperService.DateTimeToElapeSecond(cred.active_date), (int)helperService.DateTimeToElapeSecond(cred.deactive_date)))
                    {
                        errors.Add(MessageBuilder.Unsuccess(id.mac, Command.C8304));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);


            context.cardholder.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<CardHolderDto>(dto);


        }
    }
}
