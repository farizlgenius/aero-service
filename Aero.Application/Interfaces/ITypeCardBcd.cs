using System;

namespace Aero.Application.Interfaces;

public interface ITypeCardBcd
{
      //
            // Summary:
            //     number of valid digits (0-9 plus A-F)
            public short digit_count {get;}

            //
            // Summary:
            //     each entry holds a hex digit: 0x0 - 0xF
            public byte[] bcd_array{get;}
}
