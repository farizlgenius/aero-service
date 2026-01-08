namespace HIDAeroService.Entity
{
    public class RequestExit : BaseEntity
    {
        public Module module { get; set; }
        public Door door { get; set; }
        public short module_id { get; set; }
        public short input_no { get; set; }
        public short input_mode { get; set; }
        public short debounce { get; set; }
        public short holdtime { get; set; }
        public short mask_timezone { get; set; } = 0;
    }
}
