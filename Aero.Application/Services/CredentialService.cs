using HID.Aero.ScpdNet.Wrapper;
using AeroService.AeroLibrary;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.Credential;
using AeroService.DTO.AccessLevel;
using AeroService.Entity;
using AeroService.Helpers;
using AeroService.Hubs;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using AeroService.Entity.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MiNET.Entities;
using MiNET.Entities.Passive;
using AeroService.Aero.CommandService.Impl;
using AeroService.Aero.CommandService;

namespace AeroService.Service.Impl
{
    public class CredentialService(AeroMessage read, AeroCommandService command, IHelperService<Credential> helperService, IHubContext<AeroHub> hub, AppDbContext context) : ICredentialService
    {
       

        public async Task<bool> ScanCardTrigger(ScanCardDto dto)
        {
            var ScpId = await helperService.GetIdFromMacAsync(dto.Mac);
            read.isWaitingCardScan = true;
            read.ScanScpId = ScpId;
            read.ScanAcrNo = dto.DoorId;
            return true;
        }

        public void TriggerCardScan(int ScpId, int FormatNumber, int FacilityCode, double CardHolderId, int IssueCode, short FloorNumber)
        {
            string ScpMac = helperService.GetMacFromId((short)ScpId);
            hub.Clients.All.SendAsync("CardScanStatus", ScpMac, FormatNumber, FacilityCode, CardHolderId, IssueCode, FloorNumber);
            read.isWaitingCardScan = false;
        }



        public async Task<ResponseDto<bool>> CreateAsync(CredentialDto dto)
        {
            if (await context.credential.AnyAsync(x => x.card_no == dto.CardNo)) return ResponseHelper.Duplicate<bool>();
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Credential>(context, 1000);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            //cred.issue_code = await GetLowestUnassignedIssueCodeAsync(userId: dto.user_id);

            //foreach (var id in ScpIds)
            //{
            //    if (!await command.AccessDatabaseCardRecord(id.component_id, cred.flag, cred.card_no, cred.issue_code, cred.pin, cred.accesslevel, (int)helperService.DateTimeToElapeSecond(cred.active_date), (int)helperService.DateTimeToElapeSecond(cred.deactive_date)))
            //    {
            //        errors.Add(MessageBuilder.Unsuccess(id.mac, command.C8304));
            //    }
            //}
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<IEnumerable<CredentialDto>>> GetAsync()
        {
            var dtos = await context.credential
                .Include(x => x.cardholder)
                .Select(x => MapperHelper.CredentialToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<CredentialDto>>(dtos);
        }

        public async Task<ResponseDto<CredentialDto>> GetByUserId(string UserId)
        {
            var dto = await context.credential
                .Include(x => x.cardholder)
                .Where(x => x.cardholder.user_id == UserId)
                .Select(x => MapperHelper.CredentialToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<CredentialDto>(dto);
        }

        public Task<ResponseDto<bool>> UpdateAsync(CredentialDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<bool>> DeleteAsync(CredentialDto dto)
        {
            var entity = await context.credential.FirstOrDefaultAsync(x => x.card_no == dto.CardNo);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            List<string> errors = new List<string>();
            var Macs = entity.hardware_credentials.Select(x => x.hardware_mac);

            foreach (var mac in Macs)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                if (!command.CardDelete(ScpId, dto.CardNo))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C3305));
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.credential.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<int> GetLowestUnassignedIssueCodeAsync(string userId, int max = 1000)
        {

            var query = await context.credential
                .AsNoTracking()
                .Include(x => x.cardholder)
                .Where(x => x.cardholder.user_id == userId)
                .Select(x => x.issue_code).ToArrayAsync();

            // Handle empty table case quickly
            if (query.Length == 0)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = query.Distinct().OrderBy(x => x).ToList();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            if(expected > max) return -1;
            return expected;
        }

        public async Task<ResponseDto<bool>> DeleteCardAsync(DeleteCardDto dto)
        {
            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if(!command.CardDelete(ScpId,dto.CardNo))
            {
                ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, "DeleteAsync Card Fail");
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCredentialFlagAsync()
        {
            var dtos = await context.credential_flag
                .Select(x => MapperHelper.CredentialFlagToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }
    }
}
