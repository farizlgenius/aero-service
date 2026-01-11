namespace AeroService.Entity
{
    public sealed class Procedure : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public Trigger trigger { get; set; }
        public ICollection<Action> actions { get; set; }
    }
}
