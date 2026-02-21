using System;

namespace Aero.Application.Interfaces;

public interface ITypeCardBin
{
      short bit_count { get; }

      //
      // Summary:
      //     first bit is (0x80 & bit_array[0])

      byte[] bit_array { get; }
}
