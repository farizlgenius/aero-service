using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ar_card_format
    {
        [Key]
        public int id { get; set; }
        public short number { get; set; }
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
    }
}
