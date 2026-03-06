using System;

namespace Aero.Application.Interfaces;

public interface ITypeSioComm
{
      //
            // Summary:
            //     comm status
            //
            // Remarks:
            //     0 - not configured
            //
            //     1 - not tried: active, have not tried to poll it
            //
            //     2 - off-line
            //
            //     3 - on-line
             short comm_sts{ get; }

            //
            // Summary:
            //     sio model number (VALID ONLY IF "ON-LINE")
             byte model{ get; }

            //
            // Summary:
            //     sio firmware revision number (VALID ONLY IF "ON-LINE")
             byte revision{ get; }

            //
            // Summary:
            //     sio serial number (VALID ONLY IF "ON-LINE")
             int ser_num{ get; }

            //
            // Summary:
            //     use the data below only if this field is non-zero
             short nExtendedInfoValid{ get; }

            //
            // Summary:
            //     Series-2: MR50==176, MR50idh==208, MR52==180, MR16in==177, MR16out==178, MRDT
            //     - H/W rev. B and up==179, EP1502 (SIO)==111, EP1501 (SIO)==112, MR51e==140
             short nHardwareId{ get; }

            //
            // Summary:
            //     Hardware Revision
             short nHardwareRev{ get; }

            //
            // Summary:
            //     same as model
             short nProductId{ get; }

            //
            // Summary:
            //     application specific version of this ProductId
             short nProductVer{ get; }

            //
            // Summary:
            //     BOOT code version info: (maj(4) << 12)+(min(8)<< 4) + (bld(4))
             short nFirmwareBoot{ get; }

            //
            // Summary:
            //     Loader code version info: (maj(4) << 12)+(min(8)<< 4) + (bld(4))
             short nFirmwareLdr{ get; }

            //
            // Summary:
            //     App code version info: (maj(4) << 12)+(min(8) << 4) + (bld(4))
             short nFirmwareApp{ get; }

            //
            // Summary:
            //     OEM code assigned to this SIO (0 == none)
             short nOemCode{ get; }

            //
            // Summary:
            //     Master key used for encryption: 0=None, 1=AES Default Key, 2=AES Master/Secret
            //     Key, 3= PKI
             byte nEncConfig{ get; }

            //
            // Summary:
            //     Status of Master/Secret Key{ get; } 0=Not Loaded to EP, 1=Loaded, unverified, 2=Loaded,
            //     conflicts w/SIO, 3=Loaded, Verified.
             byte nEncKeyStatus{ get; }

            //
            // Summary:
            //     MAC Address, if applicable, LSB first.
             byte[] mac_addr{ get; }

            //
            // Summary:
            //     sio hardware components
             int nHardwareComponents{ get; }

}
