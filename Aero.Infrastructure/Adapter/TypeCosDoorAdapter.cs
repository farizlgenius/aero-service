using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Adapter
{
    public sealed class TypeCosDoorAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeCoSDoor
    {
        //
        // Summary:
        //     door status map
        //
        // Remarks:
        //     door_status byte encoding is the same as tagTypeCoS::status
        public byte door_status => tran.door.door_status;

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
        public byte ap_status => tran.door.ap_status;

        //
        // Summary:
        //     previous ap status
        public byte ap_prior => tran.door.ap_prior;

        //
        // Summary:
        //     previous door status map
        public byte door_prior => tran.door.door_prior;
    }
}
