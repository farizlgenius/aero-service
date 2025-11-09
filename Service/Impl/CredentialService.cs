
using AutoMapper;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Credential;
using HIDAeroService.DTO.AccessLevel;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using HIDAeroService.Entity.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MiNET.Entities;
using MiNET.Entities.Passive;

namespace HIDAeroService.Service.Impl
{
    public class CredentialService(AeroMessage read, AeroCommand command, IHelperService<Credential> helperService, IHubContext<AeroHub> hub, AppDbContext context) : ICredentialService
    {
       

        public async Task<bool> ScanCardTrigger(ScanCardDto dto)
        {
            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
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
            if (await context.Credentials.AnyAsync(x => x.CardNo == dto.CardNo)) return ResponseHelper.Duplicate<bool>();
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Credential>(context, 1000);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            //cred.IssueCode = await GetLowestUnassignedIssueCodeAsync(userId: dto.UserId);

            //foreach (var id in ScpIds)
            //{
            //    if (!await command.AccessDatabaseCardRecordAsync(id.ComponentId, cred.Flag, cred.CardNo, cred.IssueCode, cred.Pin, cred.AccessLevels, (int)helperService.DateTimeToElapeSecond(cred.ActiveDate), (int)helperService.DateTimeToElapeSecond(cred.DeactiveDate)))
            //    {
            //        errors.Add(MessageBuilder.Unsuccess(id.MacAddress, Command.C8304));
            //    }
            //}
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<IEnumerable<CredentialDto>>> GetAsync()
        {
            var dtos = await context.Credentials
                .Include(x => x.CardHolder)
                .Select(x => MapperHelper.CredentialToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<CredentialDto>>(dtos);
        }

        public async Task<ResponseDto<CredentialDto>> GetByUserId(string UserId)
        {
            var dto = await context.Credentials
                .Include(x => x.CardHolder)
                .Where(x => x.CardHolder.UserId == UserId)
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
            var entity = await context.Credentials.FirstOrDefaultAsync(x => x.CardNo == dto.CardNo);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            List<string> errors = new List<string>();
            var Macs = entity.HardwareCredentials.Select(x => x.MacAddress);

            foreach (var mac in Macs)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                if (!await command.CardDeleteAsync(ScpId, dto.CardNo))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C3305));
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.Credentials.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<int> GetLowestUnassignedIssueCodeAsync(string userId, int max = 1000)
        {

            var query = await context.Credentials
                .AsNoTracking()
                .Include(x => x.CardHolder)
                .Where(x => x.CardHolder.UserId == userId)
                .Select(x => x.IssueCode).ToArrayAsync();

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
            if(!await command.CardDeleteAsync(ScpId,dto.CardNo))
            {
                ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, "Delete Card Fail");
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

    }
}
