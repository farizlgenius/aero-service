using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class SystemSetting 
    {
        [Key]
        public int Id { get; set; }
        public short nMsp1Port { get; set; }
        public int nTransaction {  get; set; }
        public short nSio { get; set; }
        public short nMp { get; set; }
        public short nCp { get; set; }
        public short nAcr { get; set; }
        public short nAlvl { get; set; }
        public short nTrgr { get; set; }
        public short nProc { get; set; }
        public short GmtOffset { get; set; }
        public short nTz { get; set; }
        public short nHol { get; set; }
        public short nMpg { get; set; }
        public short nCard { get; set; }
        public short nArea { get; set; }
    }
}
