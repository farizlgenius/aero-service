using System;

namespace Aero.Domain.Entities;

public sealed class Feature
{
       public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public List<SubFeature> SubItems { get; set; } = new List<SubFeature>();
        public bool IsAllow { get; set; }
        public bool IsCreate { get; set; }
        public bool IsModify { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAction { get; set; }

}
