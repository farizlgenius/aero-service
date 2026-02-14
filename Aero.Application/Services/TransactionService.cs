using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{
    public sealed class TransactionService(IQTransactionRepository qTran,ITransactionRepository rTran, IScpCommand scp, IQHwRepository qHw) : ITransactionService
    {
        public async Task<ResponseDto<Pagination<TransactionDto>>> GetPageTransactionWithCountAsync(PaginationParams param)
        {
            var dto = await qTran.GetPageTransactionWithCountAsync(param);
            return ResponseHelper.SuccessBuilder<Pagination<TransactionDto>>(dto);
        }
        public async Task<ResponseDto<Pagination<TransactionDto>>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param,short location)
        {
            var dto = await qTran.GetPageTransactionWithCountAndDateAndSearchAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<TransactionDto>>(dto);
        }



        public async Task<ResponseDto<bool>> SetTranIndexAsync(string mac)
        {
            var id = await qHw.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.SetTransactionLogIndex(id, true))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.TRAN_INDEX));
            }

            return ResponseHelper.SuccessBuilder(true);
        }

         public async Task SaveToDatabaseAsync(IScpReply message)
        {
            try
            {
                if (!await qHw.IsAnyByComponentId((short)message.ScpId)) return;
                var tran = await rTran.HandleTransactionAsync(message);
                

                await rTran.AddAsync(tran);
            

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetSourceAsync()
        {
            var res = qTran.GetSourceAsync();
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetDeviceAsync(int source)
        {
            var res = await qTran.GetDeviceAsync(source);
            return ResponseHelper.SuccessBuilder(res);
        }
    }

   

}

