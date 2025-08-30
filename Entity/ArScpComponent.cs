using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArScpComponent : ArBaseEntity
    {
        public short ModelNo { get; set; }
        public string Name { get; set; }
        public short NInput { get; set; }
        public short NOutput { get; set; }
        public short NReader { get; set; }

    }
}
