
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Infrastructure.Persistences.Entities
{
    public class CardFormat : BaseEntity,IDriverId
    {
        public string name { get; set; } = string.Empty;
        public short driver_id { get; set; }
        public short facility { get; set; }
        public short offset { get; set; }
        public short function_id { get; set; }
        public short flags { get; set; }
        public short bits { get; set; }
        public short pe_ln { get; set; }
        public short pe_loc { get; set; }
        public short po_ln { get; set; }
        public short po_loc { get; set; }
        public short fc_ln { get; set; }
        public short fc_loc { get; set; }
        public short ch_ln { get; set; }
        public short ch_loc { get; set; }
        public short ic_ln { get; set; }
        public short ic_loc { get; set; }


        public CardFormat(string name,short driver,short fac,short offset,short function,short flags,short bits,short peLn,short peLoc,short poLn,short poLoc,short fcLn,short fcLoc,short chLn,short chLoc,short icLn,short icLoc,int location_id) : base(location_id)
        {
            this.name = name;
            this.driver_id = driver;
            this.facility = fac;
            this.offset = offset;
            this.function_id = function;
            this.flags = flags;
            this.bits = bits;
            this.pe_ln = peLn;
            this.pe_loc = peLoc;
            this.fc_ln = fcLn;
            this.fc_loc = fcLoc;
            this.ch_ln = chLn;
            this.ch_loc = chLoc;
            this.ic_ln = icLn;
            this.ic_loc = icLoc;
            this.po_ln = poLn;
            this.po_loc = poLoc;

        }

        public void Update(Aero.Domain.Entities.CardFormat data)
        {
            this.name = data.Name;
            this.facility = data.Facility;
            this.offset = data.Offset;
            this.function_id = data.FunctionId;
            this.flags = data.Flags;
            this.bits = data.Bits;
            this.pe_ln = data.PeLn;
            this.pe_loc = data.PeLoc;
            this.fc_ln = data.FcLn;
            this.fc_loc = data.FcLoc;
            this.ch_ln = data.ChLn;
            this.ch_loc = data.ChLoc;
            this.ic_ln = data.IcLn;
            this.ic_loc = data.IcLoc;
            this.po_ln = data.PoLn;
            this.po_loc = data.PoLoc;
            updated_date = DateTime.UtcNow;
        }


    }

    
}
