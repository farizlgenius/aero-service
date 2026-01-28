using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class CardFormatMapper
{
      public static Aero.Infrastructure.Data.Entities.CardFormat ToEf(CardFormat data)
      {
            var res = new Aero.Infrastructure.Data.Entities.CardFormat();
            // Base
            NoMacBaseMapper.ToEf(data, res);
            res.name = data.Name;
            res.facility = data.Facility;
            res.offset = data.Offset;
            res.function_id = data.FunctionId;
            res.flags = data.Flags;
            res.bits = data.Bits;
            res.pe_ln = data.PeLn;
            res.pe_loc = data.PeLoc;
            res.po_ln = data.PoLn;
            res.po_loc = data.PoLoc;
            res.fc_ln = data.FcLn;
            res.fc_loc = data.FcLoc;
            res.ch_ln = data.ChLn;
            res.ch_loc = data.ChLoc;
            res.ic_ln = data.IcLn;
            res.ic_loc = data.IcLoc;

            return res;

      }

      public static void Update(Aero.Infrastructure.Data.Entities.CardFormat res, CardFormat data)
      {
            // Base
            NoMacBaseMapper.Update(data, res);
            res.name = data.Name;
            res.facility = data.Facility;
            res.offset = data.Offset;
            res.function_id = data.FunctionId;
            res.flags = data.Flags;
            res.bits = data.Bits;
            res.pe_ln = data.PeLn;
            res.pe_loc = data.PeLoc;
            res.po_ln = data.PoLn;
            res.po_loc = data.PoLoc;
            res.fc_ln = data.FcLn;
            res.fc_loc = data.FcLoc;
            res.ch_ln = data.ChLn;
            res.ch_loc = data.ChLoc;
            res.ic_ln = data.IcLn;
            res.ic_loc = data.IcLoc;

      }
}
