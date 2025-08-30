using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArScp : ArBaseEntity
    {
        public short ScpId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Mac { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string SerialNumber { get; set; }
        public short NSio { get; set; }
        public short NMp { get; set; }
        public short NCp { get; set; }
        public short NAcr { get; set; }
        public short NAlvl { get; set; }
        public short Ntrgr { get; set; }
        public short Nproc { get; set; }
        public short NTz { get; set; }
        public short NHol { get; set; }
        public short NMpg { get; set; }
        public bool IsUpload { get; set; }
        public bool IsReset { get; set; }
        public DateTime LastSync { get; set; }


    }
}
