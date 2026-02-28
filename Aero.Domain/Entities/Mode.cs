using Aero.Domain.Helpers;

namespace Aero.Domain.Entities
{
    public sealed class Mode
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Description { get; set; } = string.Empty;

        public Mode(string name,int value,string description)
        {
            SetName(name);
            this.Value = value;
            this.Description = description;
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.");
            Name = name;
        }
    }
}
