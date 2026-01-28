using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class CardHolderService(
        IQHolderRepository qHolder,
        IQHwRepository qHw,
        IHolderCommand holder,
        IQCredRepository qCred,
        IHolderRepository rHolder,
        IHolderCommand hol,
        ICredRepository rCred
        ) : ICardHolderService
    {
        public async Task<ResponseDto<bool>> CreateAsync(CardHolderDto dto)
        {
            if (await qHolder.IsAnyByUserId(dto.UserId)) return ResponseHelper.Duplicate<bool>();
            List<short> CredentialComponentId = new List<short>();
            List<string> errors = new List<string>();
            // Send data 
            var ScpIds = await qHw.GetComponentIdsAsync();

            var domain = HolderMapper.ToDomain(dto);

            foreach (var level in domain.AccessLevels)
            {
                CredentialComponentId.Add(await qCred.GetLowestUnassignedNumberAsync(10));
                cred.IssueCode = await qCred.GetLowestUnassignedIssueCodeByUserIdAsync(8, dto.UserId);

                foreach (var id in ScpIds)
                {
                    if (!holder.AccessDatabaseCardRecord(id, domain.Flag, cred.CardNo, cred.IssueCode, cred.Pin, domain.AccessLevels.Select(x => x.ComponentId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.CARD_RECORD));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rHolder.AddAsync(domain);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string UserId)
        {
            List<string> errors = new List<string>();
            if (!await qHolder.IsAnyByUserId(UserId)) return ResponseHelper.NotFoundBuilder<bool>();

            var Macs = await qHw.GetMacsAsync();

            var en = await qHolder.GetByUserIdAsync(UserId);

            var domain = HolderMapper.ToDomain(en);

            foreach (var mac in Macs)
            {
                var ScpId = await qHw.GetComponentFromMacAsync(mac);
                foreach (var cred in domain.Credentials)
                {
                    if (!hol.CardDelete(ScpId, cred.CardNo))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C3305));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rHolder.DeleteByUserIdAsync(UserId);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, []);

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

        public async Task<ResponseDto<CardHolderDto>> UpdateAsync(CardHolderDto dto)
        {
            List<string> errors = new List<string>();

            if (!await qHolder.IsAnyByUserId(dto.UserId)) return ResponseHelper.NotFoundBuilder<CardHolderDto>();
            List<short> CredentialComponentId = new List<short>();

            var domain = HolderMapper.ToDomain(dto);



            for (int i = 0; i < domain.Credentials.Count(); i++)
            {
                CredentialComponentId.Add(await qCred.GetLowestUnassignedNumberAsync(10));
            }


            // DeleteAsync Old
            var macs = qHolder
            var macs = entity.credentials.SelectMany(x => x.hardware_credentials.Select(x => x.hardware_mac)).ToList();
            foreach (var mac in macs)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                foreach (var cred in entity.credentials)
                {
                    if (!hol.CardDelete(ScpId, cred.card_no))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C3305));
                    }
                }
            }

            var status = await rHolder.DeleteReferenceByUserIdAsync(dto.UserId);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.REMOVE_OLD_REF_UNSUCCESS, []);



            // Send data 
            var ScpIds = await context.hardware.Select(x => new { x.component_id, x.mac }).ToArrayAsync();
            foreach (var cred in entity.credentials)
            {
                foreach (var id in ScpIds)
                {
                    if (!hol.AccessDatabaseCardRecord(id.component_id, dto.Flag, cred.card_no, cred.issue_code, string.IsNullOrEmpty(cred.pin) ? "" : cred.pin, entity.access_levels is null ? new List<AccessLevel>() : entity.access_levels.Select(x => x.access_level).ToList(), (int)helperService.DateTimeToElapeSecond(cred.active_date), (int)helperService.DateTimeToElapeSecond(cred.deactive_date)))
                    {
                        errors.Add(MessageBuilder.Unsuccess(id.mac, Command.CARD_RECORD));
                    }
                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            var status = await rHolder.UpdateAsync(domain);
            if (status <= 0) return ResponseHelper.UnsuccessBuilder<CardHolderDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS, errors);
            return ResponseHelper.SuccessBuilder<CardHolderDto>(dto);


        }
    }
}
