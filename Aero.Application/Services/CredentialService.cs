
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Application.Services
{
    public class CredentialService(ICredRepository repo,IUserCommand holder) : ICredentialService
    {
       

        public async Task<bool> ScanCardTrigger(ScanCardDto dto)
        {

            //var ScpId = await helperService.GetIdFromMacAsync(dto.Mac);
            //read.isWaitingCardScan = true;
            //read.ScanScpId = ScpId;
            //read.ScanAcrNo = dto.DoorId;
            await repo.ToggleScanCardAsync(dto);
            return true;
        }

    

        public async Task<ResponseDto<IEnumerable<CredentialDto>>> GetAsync()
        {

            var dtos = await repo.GetAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<CredentialDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<CredentialDto>>> GetByUserId(string UserId)
        {

            var dtos = await repo.GetByUserIdAsync(UserId);

            return ResponseHelper.SuccessBuilder<IEnumerable<CredentialDto>>(dtos);
        }




        // public async Task<int> GetLowestUnassignedIssueCodeAsync(string userId, int max = 1000)
        // {

        //     var query = await context.credential
        //         .AsNoTracking()
        //         .Include(x => x.cardholder)
        //         .Where(x => x.cardholder.user_id == userId)
        //         .Select(x => x.issue_code).ToArrayAsync();

        //     // Handle empty table case quickly
        //     if (query.Length == 0)
        //         return 1; // start at 1 if table is empty

        //     // Load all numbers into memory (only the column, so it's lightweight)
        //     var numbers = query.Distinct().OrderBy(x => x).ToList();

        //     short expected = 1;
        //     foreach (var num in numbers)
        //     {
        //         if (num != expected)
        //             return expected; // found the lowest missing number
        //         expected++;
        //     }

        //     // If none missing in sequence, return next number
        //     if(expected > max) return -1;
        //     return expected;
        // }

        public async Task<ResponseDto<bool>> DeleteCardAsync(DeleteCardDto dto)
        {
            if(!holder.CardDelete((short)dto.DeviceId,dto.CardNo))
            {
                ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, Command.DELETE_CARD);
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCredentialFlagAsync()
        {
            var dtos = await repo.GetCredentialFlagAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }
    }
}
