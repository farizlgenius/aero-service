using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArScpSetting : ArBaseEntity
    {
        public short NMsp1Port { get; set; }
        public int NTransaction {  get; set; }
        public short NSio { get; set; }
        public short NMp { get; set; }
        public short NCp { get; set; }
        public short NAcr { get; set; }
        public short NAlvl { get; set; }
        public short NTrgr { get; set; }
        public short NProc { get; set; }
        public short GmtOffset { get; set; }
        public short NTz { get; set; }
        public short NHol { get; set; }
        public short NMpg { get; set; }
        public short NCard { get; set; }
    }
}
