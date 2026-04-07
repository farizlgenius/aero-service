using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class DeviceCredential : IDeviceId
    {
        [Key]
        public int id { get; set; }
        public int scp_id { get; set; } 
        public short credential_id { get; set; }
        public Device hardware { get; set; }
        public Credential credential { get; set; }
    }
}
