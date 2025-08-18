namespace HIDAeroService.Entity
{
    public sealed class ar_card_holder : ar_base_entity
    {
        public string card_holder_id { get; set; }
        public Guid card_holder_refenrence_number { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? description { get; set; }
        public string? holder_status { get; set; }
        public int issue_code_running_number { get; set; }
    }
}
