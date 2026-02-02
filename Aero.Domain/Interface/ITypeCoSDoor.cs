using System;

namespace Aero.Domain.Interfaces;

public interface ITypeCoSDoor
{
       //
            // Summary:
            //     door status map
            //
            // Remarks:
            //     door_status byte encoding is the same as tagTypeCoS::status
             byte door_status{ get; }

            //
            // Summary:
            //     supplemental access point status
            //
            // Remarks:
            //     ap_status byte encoding
            //
            //     0x01 - flag: set if a/p is unlocked
            //
            //     0x02 - flag: access (exit) cycle in progress
            //
            //     0x04 - flag: forced open status
            //
            //     0x08 - flag: forced open mask status
            //
            //     0x10 - flag: held open status
            //
            //     0x20 - flag: held open mask status
            //
            //     0x40 - flag: held open pre-alarm condition
            //
            //     0x80 - flag: door is in "extended held open" mode
             byte ap_status{ get; }

            //
            // Summary:
            //     previous ap status
             byte ap_prior{ get; }

            //
            // Summary:
            //     previous door status map
             byte door_prior{ get; }
}
