using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{


    public sealed record ActionDto(
        short ScpId,
        short ActionType,
        string ActionDetail,
        short Arg1,
        short Arg2,
        short Arg3,
        short Arg4,
        short Arg5,
        short Arg6,
        short Arg7,
        string StrArg,
        short DelayTime,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
}
