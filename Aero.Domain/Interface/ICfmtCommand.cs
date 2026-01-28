
using Aero.Domain.Entities;

namespace Aero.Domain.Interfaces;

public interface ICfmtCommand
{
      bool CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc);
      bool CardFormatterConfiguration(short ScpId, CardFormat dto, short funtionId);
}
