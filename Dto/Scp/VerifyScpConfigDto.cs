namespace HIDAeroService.Dto.Scp
{
    public sealed class VerifyScpConfigDto
    {
        public string Ip { get; set; }
        public string Mac { get; set; }
        public int RecAllocTransaction { get; set; }
        public int RecAllocTimezone { get; set; }
        public int RecAllocHoliday { get; set; }
        public int RecAllocSio { get; set; }
        public int RecAllocSioPort { get; set; }
        public int RecAllocMp { get; set; }
        public int RecAllocCp { get; set; }
        public int RecAllocAcr { get; set; }
        public int RecAllocAlvl { get; set; }
        public int RecAllocTrig { get; set; }
        public int RecAllocProc { get; set; }
        public int RecAllocMpg { get; set; }
        public int RecAllocArea { get; set; }
        public int RecAllocEal { get; set; }
        public int RecAllocCrdb { get; set; }
        public int RecAllocCardActive { get; set; }
    }
}
