namespace HIDAeroService.DTO
{
    public sealed class EventDto
    {
        public string Date { get; set; }
        public string Time { get; set; }
        //public int SerialNumber { get; set; }
        public string Source { get; set; }

        public string SourceNumber { get; set; }
        //public string Type { get; set; }
        public string Description { get; set; }
        public string Additional { get; set; } = "";

    }
}
