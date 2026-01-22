
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public class CardFormat : IComponentId,IDatetime
    {
        [Key]
        public int id { get; set; }
        public string uuid { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; } = string.Empty;
        public short component_id { get; set; }
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
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
