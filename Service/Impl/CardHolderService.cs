using HIDAeroService.AeroLibrary;
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
    public sealed class CardHolderService(AppDbContext context,IHelperService<Credential> helperService,AeroCommand command,ICredentialService credentialService) : ICardHolderService
    {
        public async Task<ResponseDto<bool>> CreateAsync(CardHolderDto dto)
        {
            if (await context.CardHolders.AnyAsync(x => dto.UserId == x.UserId)) return ResponseHelper.Duplicate<bool>();
            List<short> CredentialComponentId = new List<short>();
            List<string> errors = new List<string>();
            // Send data 
            var ScpIds = await context.Hardwares.Select(x => new { x.ComponentId,x.MacAddress }).ToArrayAsync();

            var entity = MapperHelper.DtoToCardHolder(dto, CredentialComponentId, DateTime.Now);

            foreach (var cred in entity.Credentials)
            {
                CredentialComponentId.Add(await helperService.GetLowestUnassignedNumberNoLimitAsync<Credential>(context));
                cred.IssueCode = await credentialService.GetLowestUnassignedIssueCodeAsync(userId:dto.UserId);

                foreach (var id in ScpIds)
                {
                    if (!await command.AccessDatabaseCardRecordAsync(id.ComponentId, dto.Flag,cred.CardNo,cred.IssueCode,cred.Pin, entity.AccessLevels.Select(x => x.AccessLevel).ToList(), (int)helperService.DateTimeToElapeSecond(cred.ActiveDate), (int)helperService.DateTimeToElapeSecond(cred.DeactiveDate)))
                    {
                        errors.Add(MessageBuilder.Unsuccess(id.MacAddress, Command.C8304));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);


            await context.CardHolders.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string UserId)
        {
            List<string> errors = new List<string>();
            var entity = await context.CardHolders
                .AsNoTracking()
                .Include(x => x.Additional)
                .Include(x => x.Credentials)
                .ThenInclude(x => x.HardwareCredentials)
                .Where(x => x.UserId == UserId)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            var Macs = entity.Credentials
                .SelectMany(x => x.HardwareCredentials.Select(x => x.MacAddress))
                .Distinct()
                .ToArray();

            foreach(var mac in Macs)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                foreach(var cred in entity.Credentials)
                {
                    if (!await command.CardDeleteAsync(ScpId, cred.CardNo))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac,Command.C3305));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.CardHolders.Remove(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);


        }
        
        public async Task<ResponseDto<IEnumerable<CardHolderDto>>> GetAsync()
        {
            var dtos = await context.CardHolders
                .Include(x => x.Additional)
                .Include(x => x.Credentials)
                .Include(x => x.AccessLevels)
                .ThenInclude(x => x.AccessLevel)
                .Select(x => MapperHelper.CardHolderToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<CardHolderDto>>(dtos);

        }

        public async Task<ResponseDto<IEnumerable<CardHolderDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await context.CardHolders
                .Include(x => x.Additional)
                .Include(x => x.Credentials)
                .Include(x => x.AccessLevels)
                .ThenInclude(x => x.AccessLevel)
                .Where(x => x.LocationId == location)
                .Select(x => MapperHelper.CardHolderToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<CardHolderDto>>(dtos);

        }

        public async Task<ResponseDto<CardHolderDto>> GetByUserIdAsync(string UserId)
        {
            var dto = await context.CardHolders
                .Include(x => x.Additional)
                .Include(x => x.Credentials)
                .Include(x => x.AccessLevels)
                .ThenInclude(x => x.AccessLevel)
                .Select(x => MapperHelper.CardHolderToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<CardHolderDto>> UpdateAsync(CardHolderDto dto)
        {
            List<string> errors = new List<string>();
            var entity = await context.CardHolders
                .Include(x => x.AccessLevels)
                .ThenInclude(x => x.AccessLevel)
                .Include(x => x.Additional)
                .Include(x => x.Credentials)
                .Where(x => x.UserId == dto.UserId)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<CardHolderDto>();
            List<short> CredentialComponentId = new List<short>();

            context.CardHolderAdditionals.RemoveRange(entity.Additional);
            context.CardHolderAccessLevels.RemoveRange(entity.AccessLevels);

            for(int i = 0;i < dto.Credentials.Count(); i++)
            {
                CredentialComponentId.Add(await helperService.GetLowestUnassignedNumberNoLimitAsync<Credential>(context));
            }

            MapperHelper.UpdateCardHolder(entity, dto, CredentialComponentId);

            // DeleteAsync Old 
            var macs = entity.Credentials.SelectMany(x => x.HardwareCredentials.Select(x => x.MacAddress)).ToList();
            foreach(var mac in macs)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                foreach (var cred in entity.Credentials)
                {
                    if (!await command.CardDeleteAsync(ScpId, cred.CardNo))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C3305));
                    }
                }
            }


            // Send data 
            var ScpIds = await context.Hardwares.Select(x => new { x.ComponentId, x.MacAddress }).ToArrayAsync();
            foreach (var cred in entity.Credentials)
            {     
                foreach (var id in ScpIds)
                {
                    if (!await command.AccessDatabaseCardRecordAsync(id.ComponentId, dto.Flag, cred.CardNo, cred.IssueCode,string.IsNullOrEmpty(cred.Pin) ? "" : cred.Pin, entity.AccessLevels is null ? new List<AccessLevel>() : entity.AccessLevels.Select(x => x.AccessLevel).ToList(), (int)helperService.DateTimeToElapeSecond(cred.ActiveDate), (int)helperService.DateTimeToElapeSecond(cred.DeactiveDate)))
                    {
                        errors.Add(MessageBuilder.Unsuccess(id.MacAddress, Command.C8304));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);


            context.CardHolders.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<CardHolderDto>(dto);


        }
    }
}
