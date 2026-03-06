using System;

namespace Aero.Application.Interfaces;

public interface ISrSio
{
      //
            // Summary:
            //     SIO number
             short number {get;}

            //
            // Summary:
            //     comm status: encoded per tran codes for tranTypeSioComm
             short com_status{get;}

            //
            // Summary:
            //     MSP1 driver number (0, 1, ...)
             short msp1_dnum{get;}

            //
            // Summary:
            //     retries since power-up, cumulative
             int com_retries{get;}

            //
            // Summary:
            //     cabinet tamper status: TranCoS::status encoded
             short ct_stat{get;}

            //
            // Summary:
            //     power monitor status: TranCoS::status encoded
             short pw_stat{get;}

            //
            // Summary:
            //     identification: see C03_02
             short model{get;}

            //
            // Summary:
            //     firmware revision number: see C03_02
             short revision{get;}

            //
            // Summary:
            //     serial number
             int serial_number{get;}

            //
            // Summary:
            //     number of inputs
             short inputs{get;}

            //
            // Summary:
            //     number of outputs
             short outputs{get;}

            //
            // Summary:
            //     number of readers
             short readers{get;}

            //
            // Summary:
            //     input point status: TranCoS::status encoded
             short[] ip_stat{get;}

            //
            // Summary:
            //     output point status: TranCoS::status encoded
             short[] op_stat{get;}

            //
            // Summary:
            //     reader tamper status: TranCoS::status encoded
             short[] rdr_stat{get;}

            //
            // Summary:
            //     use the data below only if this field is non-zero
             short nExtendedInfoValid{get;}

            //
            // Summary:
            //     MR-50 =80, MR-16IN = 81,MR-16OUT = 82, MR-DT = 83, MR-52 = 84, EP-1502 (internal
            //     SIO) = 85
             short nHardwareId{get;}

            //
            // Summary:
            //     Hardware Revision
             short nHardwareRev{get;}

            //
            // Summary:
            //     same as model
             short nProductId{get;}

            //
            // Summary:
            //     application specific version of this ProductId
             short nProductVer{get;}

            //
            // Summary:
            //     BOOT code version info: (maj(4) << 12)+(min(8) << 4) + (bld(4))
             short nFirmwareBoot{get;}

            //
            // Summary:
            //     Loader code version info: (maj(4) << 12)+(min(8) << 4) + (bld(4))
             short nFirmwareLdr{get;}

            //
            // Summary:
            //     App code version info: (maj(4) << 12)+(min(8) << 4) + (bld(4))
             short nFirmwareApp{get;}

            //
            // Summary:
            //     OEM code assigned to this SIO (0 == none)
             short nOemCode{get;}

            //
            // Summary:
            //     SIO comm encryption support: 0=None, 1=AES Default Key, 2=AES Master/Secret Key,
            //     3= PKI
             byte nEncConfig{get;}

            //
            // Summary:
            //     Status of Master/Secret Key{get;} 0=Not Loaded to EP, 1=Loaded, unverified, 2=Loaded,
            //     conflicts w/SIO, 3=Loaded, Verified.
             byte nEncKeyStatus{get;}

            //
            // Summary:
            //     MAC Address, if applicable, LSB first.
             byte[] mac_addr{get;}

            //
            // Summary:
            //     emergency switch status: TranCoS::status encoded
             short emg_stat{get;}
}
