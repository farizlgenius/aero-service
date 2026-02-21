
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Holiday : BaseEntity
    {
        public short year { get; set; }
        public short month { get; set; }
        public short day { get; set; }
        public short extend { get; set; }
        public short type_mask { get; set; }
    }
}
