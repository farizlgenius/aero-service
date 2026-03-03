using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.Services
{
    public sealed class UserService(
        IDeviceRepository hw,
        IUserCommand user,
        IUserRepository repo,
        ICredRepository creds,
        IFileStorage file,
        ISettingRepository setting
        ) : IUserService
    {


        public async Task<ResponseDto<UserDto>> CreateAsync(UserDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.FirstName.Trim())) return ResponseHelper.BadRequestName<UserDto>();

            var ScpSetting = await setting.GetScpSettingAsync();


            if (string.IsNullOrEmpty(dto.UserId)) return ResponseHelper.BadRequest<UserDto>();
            if (await repo.IsAnyByUserId(dto.UserId)) return ResponseHelper.Duplicate<UserDto>();


            List<short> CredentialComponentId = new List<short>();
            List<string> errors = new List<string>();

            var domain = new User(
                dto.UserId,
                dto.Title,
                dto.FirstName,
                dto.MiddleName,
                dto.LastName,
                dto.Sex,
                dto.Email,
                dto.Phone,
                dto.Company,
                dto.Position,
                dto.Department,
                dto.Image,
                dto.Flag,
                dto.Additionals,
                dto.Credentials.Select(c => new Credential(
                    c.Bits,
                    c.IssueCode,
                    c.FacilityCode,
                    c.CardNo,
                    c.Pin,
                    c.ActiveDate,
                    c.DeactiveDate,
                    dto.UserId
                )).ToList(),
                dto.AccessLevels.Select(a => new AccessLevel(
                    a.Id,
                    a.Name,
                    a.Components.Select(c => new AccessLevelComponent(
                        c.DriverId,
                        c.DeviceId,
                        c.DoorId,
                        c.AcrId,
                        c.TimeZoneId
                    )).ToList(),
                    a.LocationId,
                    a.IsActive
                )).ToList(),
                dto.LocationId,
                dto.IsActive
                );

        

            foreach (var alvl in domain.AccessLevels)
            {
                // Get unique deviceid
                var deviceIds = alvl.Components.Select(x => x.DeviceId).Distinct().ToList();
                foreach (var cred in domain.Credentials)
                {
                    
                    cred.IssueCode = await creds.GetLowestUnassignedIssueCodeByUserIdAsync(8, dto.UserId);

                    foreach (var deviceId in deviceIds)
                    {

                        if (!user.AccessDatabaseCardRecord((short)deviceId, domain.Flag, cred.CardNo, cred.IssueCode, cred.Pin, alvl.Components.Where(x => x.DeviceId == deviceId).Select(x => x.DriverId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)deviceId), Command.CARD_RECORD));
                        }

                    }

                }
            }


            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<UserDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await repo.AddAsync(domain);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<UserDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<bool>> UploadImageAsync(string userid,Stream stream)
        {
            if (string.IsNullOrEmpty(userid)) return ResponseHelper.BadRequest<bool>();
            if (!await repo.IsAnyByUserId(userid)) return ResponseHelper.NotFoundBuilder<bool>();

            var path = await file.SaveUserAsync(stream,userid);

            var status = await repo.UpdateImagePathAsync(path,userid);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<UserDto>> DeleteAsync(string UserId)
        {
            List<string> errors = new List<string>();
            if (!await repo.IsAnyByUserId(UserId)) return ResponseHelper.NotFoundBuilder<UserDto>();

            var dto = await repo.GetByUserIdAsync(UserId);

            var domain = new User(
                dto.UserId,
                dto.Title,
                dto.FirstName,
                dto.MiddleName,
                dto.LastName,
                dto.Sex,
                dto.Email,
                dto.Phone,
                dto.Company,
                dto.Position,
                dto.Department,
                dto.Image,
                dto.Flag,
                dto.Additionals,
                dto.Credentials.Select(c => new Credential(
                    c.Bits,
                    c.IssueCode,
                    c.FacilityCode,
                    c.CardNo,
                    c.Pin,
                    c.ActiveDate,
                    c.DeactiveDate,
                    dto.UserId
                )).ToList(),
                dto.AccessLevels.Select(a => new AccessLevel(
                    a.Id,
                    a.Name,
                    a.Components.Select(c => new AccessLevelComponent(
                        c.DriverId,
                        c.DeviceId,
                        c.DoorId,
                        c.AcrId,
                        c.TimeZoneId
                    )).ToList(),
                    a.LocationId,
                    a.IsActive
                )).ToList(),
                dto.LocationId,
                dto.IsActive
                );

            // foreach (var alvl in domain.AccessLevels)
            // {
            //     foreach (var cred in domain.Credentials)
            //     {
            //         foreach (var component in alvl.Components)
            //         {
            //             var id = await hw.GetComponentIdFromMacAsync(component.Mac);
            //             if (!user.CardDelete(id, cred.CardNo))
            //             {
            //                 errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.DELETE_CARD));
            //             }

            //         }

            //     }
            // }

            foreach (var alvl in domain.AccessLevels)
            {
                // Get unique deviceid
                var deviceIds = alvl.Components.Select(x => x.DeviceId).Distinct().ToList();
                foreach (var cred in domain.Credentials)
                {
                    

                    foreach (var deviceId in deviceIds)
                    {

                        if (!user.CardDelete((short)deviceId,cred.CardNo ))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)deviceId), Command.CARD_RECORD));
                        }

                    }

                }
            }



            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<UserDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await repo.DeleteByUserIdAsync(UserId);

            

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<UserDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, []);

            file.DeleteUserAsync(UserId);
            return ResponseHelper.SuccessBuilder<UserDto>(dto);

        }

        public async Task<ResponseDto<bool>> DeleteImageAsync(string userid)
        {
            var en = await repo.GetByUserIdAsync(userid);
            if(en is null) return ResponseHelper.NotFoundBuilder<bool>();

            file.DeleteUserAsync(userid);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<UserDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<UserDto>>(dtos);

        }

        public async Task<ResponseDto<IEnumerable<UserDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<UserDto>>(dtos);

        }

        public async Task<ResponseDto<UserDto>> GetByUserIdAsync(string UserId)
        {
            var dto = await repo.GetByUserIdAsync(UserId);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<Pagination<UserDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<UserDto>> UpdateAsync(UserDto dto)
        {
            List<string> errors = new List<string>();

            if (!await repo.IsAnyByUserId(dto.UserId)) return ResponseHelper.NotFoundBuilder<UserDto>();
            List<short> CredentialComponentId = new List<short>();

            var en = await repo.GetByUserIdAsync(dto.UserId);

            var domain = new User(
                en.UserId,
                en.Title,
                en.FirstName,
                en.MiddleName,
                en.LastName,
                en.Sex,
                en.Email,
                en.Phone,
                en.Company,
                en.Position,
                en.Department,
                en.Image,
                en.Flag,
                en.Additionals,
                en.Credentials.Select(c => new Credential(
                    c.Bits,
                    c.IssueCode,
                    c.FacilityCode,
                    c.CardNo,
                    c.Pin,
                    c.ActiveDate,
                    c.DeactiveDate,
                    en.UserId
                )).ToList(),
                en.AccessLevels.Select(a => new AccessLevel(
                    a.Id,
                    a.Name,
                    a.Components.Select(c => new AccessLevelComponent(
                        c.DriverId,
                        c.DeviceId,
                        c.DoorId,
                        c.AcrId,
                        c.TimeZoneId
                    )).ToList(),
                    a.LocationId,
                    a.IsActive
                )).ToList(),
                en.LocationId,
                en.IsActive
                );

            foreach (var alvl in domain.AccessLevels)
            {
                // Get unique deviceid
                var deviceIds = alvl.Components.Select(x => x.DeviceId).Distinct().ToList();
                foreach (var cred in domain.Credentials)
                {
                    

                    foreach (var deviceId in deviceIds)
                    {

                        if (!user.CardDelete((short)deviceId,cred.CardNo ))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)deviceId), Command.CARD_RECORD));
                        }

                    }

                }
            }


            

            domain = new User(
                dto.UserId,
                dto.Title,
                dto.FirstName,
                dto.MiddleName,
                dto.LastName,
                dto.Sex,
                dto.Email,
                dto.Phone,
                dto.Company,
                dto.Position,
                dto.Department,
                dto.Image,
                dto.Flag,
                dto.Additionals,
                dto.Credentials.Select(c => new Credential(
                    c.Bits,
                    c.IssueCode,
                    c.FacilityCode,
                    c.CardNo,
                    c.Pin,
                    c.ActiveDate,
                    c.DeactiveDate,
                    dto.UserId
                )).ToList(),
                dto.AccessLevels.Select(a => new AccessLevel(
                    a.Id,
                    a.Name,
                    a.Components.Select(c => new AccessLevelComponent(
                        c.DriverId,
                        c.DeviceId,
                        c.DoorId,
                        c.AcrId,
                        c.TimeZoneId
                    )).ToList(),
                    a.LocationId,
                    a.IsActive
                )).ToList(),
                dto.LocationId,
                dto.IsActive
                );


            var status = await repo.DeleteReferenceByUserIdAsync(dto.UserId);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<UserDto>(ResponseMessage.REMOVE_OLD_REF_UNSUCCESS, []);


            // Send data 
            foreach (var alvl in domain.AccessLevels)
            {
                // Get unique deviceid
                var deviceIds = alvl.Components.Select(x => x.DeviceId).Distinct().ToList();
                foreach (var cred in domain.Credentials)
                {
                    
                    cred.IssueCode = await creds.GetLowestUnassignedIssueCodeByUserIdAsync(8, dto.UserId);

                    foreach (var deviceId in deviceIds)
                    {

                        if (!user.AccessDatabaseCardRecord((short)deviceId, domain.Flag, cred.CardNo, cred.IssueCode, cred.Pin, alvl.Components.Where(x => x.DeviceId == deviceId).Select(x => x.DriverId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)deviceId), Command.CARD_RECORD));
                        }

                    }

                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<UserDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            status = await repo.UpdateAsync(domain);
            if (status <= 0) return ResponseHelper.UnsuccessBuilder<UserDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS, errors);
            return ResponseHelper.SuccessBuilder<UserDto>(dto);


        }
    }
}
