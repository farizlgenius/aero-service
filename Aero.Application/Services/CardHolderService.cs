using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using System.IO;

namespace Aero.Application.Services
{
    public sealed class CardHolderService(
        IQHolderRepository qHolder,
        IQHwRepository qHw,
        IHolderCommand holder,
        IQCredRepository qCred,
        IHolderRepository rHolder,
        IHolderCommand hol,
        ICredRepository rCred,
        IFileStorage file
        ) : ICardHolderService
    {


        public async Task<ResponseDto<bool>> CreateAsync(CardHolderDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserId)) return ResponseHelper.BadRequest<bool>();
            if (await qHolder.IsAnyByUserId(dto.UserId)) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await qHolder.GetLowestUnassignedNumberAsync(10,"");
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            List<short> CredentialComponentId = new List<short>();
            List<string> errors = new List<string>();

            var domain = HolderMapper.ToDomain(dto);
            domain.ComponentId = ComponentId;



            foreach (var alvl in domain.AccessLevels)
            {
                foreach (var cred in domain.Credentials)
                {
                    CredentialComponentId.Add(await qCred.GetLowestUnassignedNumberAsync(10,""));
                    cred.IssueCode = await qCred.GetLowestUnassignedIssueCodeByUserIdAsync(8, dto.UserId);

                    foreach (var component in alvl.Components)
                    {

                        var id = await qHw.GetComponentIdFromMacAsync(component.Mac);
                        if (!holder.AccessDatabaseCardRecord(id, domain.Flag, cred.CardNo, cred.IssueCode, cred.Pin,domain.AccessLevels.Where(x => x.Components.Any(x => x.Mac.Equals(component.Mac))).Select(x => x.ComponentId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.CARD_RECORD));
                        }

                    }

                }
            }



            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rHolder.AddAsync(domain);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> UploadImageAsync(string userid,Stream stream)
        {
            if (string.IsNullOrEmpty(userid)) return ResponseHelper.BadRequest<bool>();
            if (!await qHolder.IsAnyByUserId(userid)) return ResponseHelper.NotFoundBuilder<bool>();

            var path = await file.SaveUserAsync(stream,userid);

            var status = await rHolder.UpdateImagePathAsync(path,userid);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string UserId)
        {
            List<string> errors = new List<string>();
            if (!await qHolder.IsAnyByUserId(UserId)) return ResponseHelper.NotFoundBuilder<bool>();

            var en = await qHolder.GetByUserIdAsync(UserId);

            var domain = HolderMapper.ToDomain(en);

            foreach (var alvl in domain.AccessLevels)
            {
                foreach (var cred in domain.Credentials)
                {
                    foreach (var component in alvl.Components)
                    {
                        var id = await qHw.GetComponentIdFromMacAsync(component.Mac);
                        if (!hol.CardDelete(id, cred.CardNo))
                        {
                            errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.DELETE_CARD));
                        }

                    }

                }
            }



            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rHolder.DeleteByUserIdAsync(UserId);

            

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, []);

            file.DeleteUserAsync(UserId);
            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<bool>> DeleteImageAsync(string userid)
        {
            var en = await qHolder.GetByUserIdAsync(userid);
            if(en is null) return ResponseHelper.NotFoundBuilder<bool>();

            file.DeleteUserAsync(userid);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<CardHolderDto>>> GetAsync()
        {
            var dtos = await qHolder.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<CardHolderDto>>(dtos);

        }

        public async Task<ResponseDto<IEnumerable<CardHolderDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await qHolder.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<CardHolderDto>>(dtos);

        }

        public async Task<ResponseDto<CardHolderDto>> GetByUserIdAsync(string UserId)
        {
            var dto = await qHolder.GetByUserIdAsync(UserId);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<Pagination<CardHolderDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await qHolder.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<CardHolderDto>> UpdateAsync(CardHolderDto dto)
        {
            List<string> errors = new List<string>();

            if (!await qHolder.IsAnyByUserId(dto.UserId)) return ResponseHelper.NotFoundBuilder<CardHolderDto>();
            List<short> CredentialComponentId = new List<short>();

            var en = await qHolder.GetByUserIdAsync(dto.UserId);

            var domain = HolderMapper.ToDomain(en);

            foreach (var alvl in domain.AccessLevels)
            {
                foreach (var cred in domain.Credentials)
                {
                    foreach (var component in alvl.Components)
                    {
                        var id = await qHw.GetComponentIdFromMacAsync(component.Mac);
                        if (!hol.CardDelete(id, cred.CardNo))
                        {
                            errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.DELETE_CARD));
                        }

                    }

                }
            }

            domain = HolderMapper.ToDomain(dto);


            var status = await rHolder.DeleteReferenceByUserIdAsync(dto.UserId);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.REMOVE_OLD_REF_UNSUCCESS, []);


            // Send data 
            foreach (var alvl in domain.AccessLevels)
            {
                foreach (var cred in domain.Credentials)
                {
                    CredentialComponentId.Add(await qCred.GetLowestUnassignedNumberAsync(10,""));
                    cred.IssueCode = await qCred.GetLowestUnassignedIssueCodeByUserIdAsync(8, dto.UserId);

                    foreach (var component in alvl.Components)
                    {

                        var id = await qHw.GetComponentIdFromMacAsync(component.Mac);
                        if (!holder.AccessDatabaseCardRecord(id, domain.Flag, cred.CardNo, cred.IssueCode, cred.Pin,domain.AccessLevels.Where(x => x.Components.Any(x => x.Mac.Equals(x))).Select(x => x.ComponentId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.CARD_RECORD));
                        }

                    }

                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            status = await rHolder.UpdateAsync(domain);
            if (status <= 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS, errors);
            return ResponseHelper.SuccessBuilder<CardHolderDto>(dto);


        }
    }
}
