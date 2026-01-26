using System;

namespace Aero.Domain.Entities;

public sealed class Strike : BaseEntity
{
       public short ModuleId { get; set; }
        public short OutputNo { get; set; }
        public short RelayMode { get; set; }
        public short OfflineMode { get; set; }
        public short StrkMax { get; set; }
        public short StrkMin { get; set; }
        public short StrkMode { get; set; }
}
