
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record UserDto(
        int Id,
        string UserId,
        string Identification,
        string Title,
        string FirstName,
        string MiddleName,
        string LastName,
        int Gender,
        string DateOfBirth,
        string Email,
        string Phone,
        int CompanyId,
        string Company,
        int PositionId,
        string Position,
        string Image,
        int DepartmentId,
        string Department,
        string Address,
        short Flag,
        List<string> Additionals,
        List<CredentialDto> Credentials,
        List<AccessLevelDto> AccessLevels,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
}
