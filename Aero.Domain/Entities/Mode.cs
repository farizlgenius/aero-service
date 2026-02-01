namespace Aero.Domain.Entities
{
    public sealed class Mode
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
