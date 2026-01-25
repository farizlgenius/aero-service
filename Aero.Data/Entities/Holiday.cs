using System;
using Aero.Domain.Interface;

namespace Aero.Domain.Entities;

public sealed class Holiday : NoMacBaseEntity
{
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Extend { get; set; }
        public short TypeMask { get; set; }
  
}
