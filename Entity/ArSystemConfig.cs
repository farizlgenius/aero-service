using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArSystemConfig : ArBaseEntity
    {
        public short NPorts { get; set; }
        public short NScp { get; set; }
        public short NChannelId { get; set; }
        public short CType { get; set; }
        public short CPort { get; set; }

    }
}
