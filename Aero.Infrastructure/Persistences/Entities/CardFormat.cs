
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public class CardFormat : BaseEntity
    {
        public string name { get; set; } = string.Empty;
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


        public CardFormat(, int location_id) : base(location_id)
        {

        }


    }

    
}
