
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record UserDto(
        string UserId,
        string Title,
        string FirstName,
        string MiddleName,
        string LastName,
        string Sex,
        string Email,
        string Phone,
        string Company,
        string Position,
        string Image,
        string Department,
        short Flag,
        List<string> Additionals,
        List<CredentialDto> Credentials,
        List<AccessLevelDto> AccessLevels,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
}
