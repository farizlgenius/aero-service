

namespace Aero.Application.DTOs
{


    public sealed record PermissionDto(
        int SourceId,
        string Name,
        bool IsAllow,
        bool IsCreate,
        bool IsModify,
        bool IsDelete,
        bool IsAction);
}
