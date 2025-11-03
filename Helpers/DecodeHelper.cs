using HID.Aero.ScpdNet.Wrapper;

namespace HIDAeroService.Utility
{
    public sealed class DecodeHelper
    {
        [Flags]
        private enum StatusFlags : byte
        {
            None = 0x00,
            Offline = 0x08,
            Masked = 0x10,
            LocalMask = 0x20,
            EntryDelay = 0x40,
            NotAttached = 0x80
        }


        private enum TamperReaderStatusType : byte
        {
            OnlineTamperActive = 0,
            OnlineTamperInactive = 1,
            NA = 2,
            Offline = 3
            
        }

        private enum StatusType : byte
        {
            Inactive = 0,
            Active = 1,
            GroundFault = 2,
            Short = 3,
            OpenCircuit = 4,
            ForeignVoltage = 5,
            NonSettlingErr = 6,
            SupervisoryFault = 7
        }

        private enum ModuleStatus : byte
        {
            Notconfig = 0,
            Active = 1,
            Offline = 2,
            Online = 3
        }

        private enum ModuleTypeStatus : short
        {
            Disabled = 1,
            Timeout = 2,
            Invalid = 3,
            Commandlong = 4,
            Online = 5,
            hexLoad = 6
        }

        public static string TypeSioCommTranCodeDecode(short b)
        {
            return ((ModuleTypeStatus)b).ToString();
        }

        public static string TypeSioCommStatusDecode(byte b)
        {
            return ((ModuleStatus)b).ToString();
        }

        public static string TypeCosStatusDecode(byte statusByte,short? source = -1)
        {
            if(source == (short)tranSrc.tranSrcAcrTmpr)
            {
                var flags = ((StatusFlags)((byte)(statusByte & 0xF8)));
                if(flags == StatusFlags.None) return ((TamperReaderStatusType)((byte)(statusByte & 0x07))).ToString();
                return flags.ToString();

            }
            else
            {
                var flags = ((StatusFlags)((byte)(statusByte & 0xF8)));
                if(flags == StatusFlags.None) return ((StatusType)((byte)(statusByte & 0x07))).ToString();
                return flags.ToString();

            }

        }
    }
}
