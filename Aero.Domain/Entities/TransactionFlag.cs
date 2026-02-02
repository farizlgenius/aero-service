using System;

namespace Aero.Domain.Entities;

public sealed class TransactionFlag
{
       public string Topic { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

}
