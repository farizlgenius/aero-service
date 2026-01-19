


using Aero.Infrastructure.Data.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Holiday : NoMacBaseEntity,IComponentId
    {
        public short component_id { get; set; }
        public short year { get; set; }
        public short month { get; set; }
        public short day { get; set; }
        public short extend { get; set; }
        public short type_mask { get; set; }
    }
}
