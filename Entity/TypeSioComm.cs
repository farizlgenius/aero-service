using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TypeSioComm : BaseTransactionType
    {

        public string CommSts { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public short Revision { get; set; }
        public int Serial { get; set; }
        public short nExtendedInfoValid { get; set; }
        public string Hardware { get; set; } = string.Empty;
        public short nHardwareRev { get; set; }
        public short nProductId { get; set; }
        public short nProductVer { get; set; }
        public short nFirmwareBoot { get; set; }
        public short nFirmwareLdr { get; set; }
        public short nFirmwareApp { get; set; }
        public short nOemCode { get; set; }
        public string nEncConfig { get; set; } = string.Empty;
        public string nKeyStatus { get; set; } = String.Empty;
        public int nHardwareComponents { get; set; } 
     

    }
}
