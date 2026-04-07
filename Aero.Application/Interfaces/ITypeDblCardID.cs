using System;

namespace Aero.Application.Interfaces;

public interface ITypeDblCardID
{
      // Summary:
      //     index to the format table that was used, negative if reverse read
      short format_number { get; }

      //
      // Summary:
      //     cardholder ID number
      int cardholder_id { get; }

      //
      // Summary:
      //     zero if not available (or not supported), else 1==first floor, ...
      short floor_number { get; }

      //
      // Summary:
      //     Card type flags (bit-0 = escort, bit-1 = escort required)
      short card_type_flags { get; }

      //
      // Summary:
      //     Elevator cab number
      short elev_cab { get; }

}
