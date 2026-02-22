using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{


    public sealed record CardFormatDto(
        string Name,
        short Fac,
        short Offset,
        short FuncId,
        short Flag,
        short Bits,
        short PeLn,
        short PeLoc,
        short PoLn,
        short PoLoc,
        short FcLn,
        short FcLoc,
        short ChLn,
        short ChLoc,
        short IcLn,
        short IcLoc,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
}
