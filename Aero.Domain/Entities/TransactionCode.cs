using System;

namespace Aero.Domain.Entities;

public sealed class TransactionCode
{
      public string Name { get; set; } = string.Empty;
        public short Value { get; set; }
        public string Description { get; set; } = string.Empty;
}
