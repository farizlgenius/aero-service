
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{


    public sealed record RoleDto(
        int Id,
        string Name,
        List<PermissionDto> Permissions,
        int LocationId,
        bool Status) : BaseDto(LocationId,Status);
}
