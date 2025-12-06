namespace HIDAeroService.Entity
{
    public sealed class Procedure : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Trigger Trigger { get; set; }
        public ICollection<Action> Actions { get; set; }
    }
}
