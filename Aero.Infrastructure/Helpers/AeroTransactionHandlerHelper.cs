
using Aero.Application.Helpers;
using Aero.Domain.Entities;
using HID.Aero.ScpdNet.Wrapper;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;

namespace Aero.Infrastructure.Helpers
{
    public static class AeroTransactionHandlerHelper
    {

        #region Source

        public static string Source(tranSrc src)
        {
            return src switch 
            {
                tranSrc.tranSrcScpDiag => "hardware",
                tranSrc.tranSrcScpCom => "hardware",
                tranSrc.tranSrcScpLcl => "hardware",
                tranSrc.tranSrcSioDiag => "modules",
                tranSrc.tranSrcSioCom => "modules",
                tranSrc.tranSrcSioTmpr => "modules",
                tranSrc.tranSrcSioPwr => "modules",
                tranSrc.tranSrcMP => "Monitor Point",
                tranSrc.tranSrcCP => "Control Point",
                tranSrc.tranSrcACR => "door",
                tranSrc.tranSrcAcrTmpr => "door",
                tranSrc.tranSrcAcrDoor => "door",
                tranSrc.tranSrcAcrRex0 => "door REX",
                tranSrc.tranSrcAcrRex1 => "door REX",
                tranSrc.tranSrcTimeZone => "time Zone",
                tranSrc.tranSrcProcedure => "procedure",
                tranSrc.tranSrcTrigger => "trigger",
                tranSrc.tranSrcTrigVar => "trigger",
                tranSrc.tranSrcMPG => "Monitor Group",
                tranSrc.tranSrcArea => "Area",
                tranSrc.tranSrcAcrTmprAlt => "door",
                tranSrc.tranSrcLoginService => "Login Service",
                _ => "Unknown"
            };
        }

        public static string SourceDesc(tranSrc src)
        {
            return src switch
            {
                tranSrc.tranSrcScpDiag => "SCP diagnostics",
                tranSrc.tranSrcScpCom => "SCP to HOST communication driver - not defined",
                tranSrc.tranSrcScpLcl => "SCP local monitor points (tamper & power fault)",
                tranSrc.tranSrcSioDiag => "SIO diagnostics",
                tranSrc.tranSrcSioCom => "SIO communication driver",
                tranSrc.tranSrcSioTmpr => "SIO cabinet tamper",
                tranSrc.tranSrcSioPwr => "SIO power monitor",
                tranSrc.tranSrcMP => "Alarm monitor point",
                tranSrc.tranSrcCP => "Output control point",
                tranSrc.tranSrcACR => "Access Control Reader (ACR)",
                tranSrc.tranSrcAcrTmpr => "ACR: reader tamper monitor",
                tranSrc.tranSrcAcrDoor => "ACR: door position sensor",
                tranSrc.tranSrcAcrRex0 => "ACR: 1st \"Request to exit\" input",
                tranSrc.tranSrcAcrRex1 => "ACR: 2nd \"Request to exit\" input",
                tranSrc.tranSrcTimeZone => "time Zone",
                tranSrc.tranSrcProcedure => "procedure (action list)",
                tranSrc.tranSrcTrigger => "trigger",
                tranSrc.tranSrcTrigVar => "trigger variable",
                tranSrc.tranSrcMPG => "Monitor point group",
                tranSrc.tranSrcArea => "Access area",
                tranSrc.tranSrcAcrTmprAlt => "ACR: the alternate reader's tamper monitor source_number",
                tranSrc.tranSrcLoginService => "Login Service",
                _ => "Unknown"
            };
        }

        #endregion

        #region Type

        public static string TypeDesc(tranType type)
        {
            return type switch
            {
                tranType.tranTypeSys => "System",
                tranType.tranTypeSioComm => "SIO communication status ",
                tranType.tranTypeCardBin => "Binary card data",
                tranType.tranTypeCardBcd => "Card data",
                tranType.tranTypeCardFull => "32-bit CSN",
                tranType.tranTypeDblCardFull => "52-bit CSN",
                tranType.tranTypeI64CardFull => "64-bit CSN",
                tranType.tranTypeI64CardFullIc32 => "64-bit CSN / 32-bit issue code",
                tranType.tranTypeCardID => "32-bit CSN",
                tranType.tranTypeDblCardID => "52-bit CSN",
                tranType.tranTypeI64CardID => "64-bit CSN",
                tranType.tranTypeHostCardFullPin => "Card & PIN",
                tranType.tranTypeCoS => "status change",
                tranType.tranTypeREX => "REX used",
                tranType.tranTypeCoSDoor => "door status",
                tranType.tranTypeProcedure => "procedure",
                tranType.tranTypeUserCmnd => "User command",
                tranType.tranTypeActivate => "trigger & Timezone state",
                tranType.tranTypeAcr => "door mode",
                tranType.tranTypeMpg => "Monitor Group",
                tranType.tranTypeArea => "Access Area",
                tranType.tranTypeUseLimit => "Use limit",
                tranType.tranTypeWebActivity => "Web activity",
                tranType.tranTypeOperatingMode => "Operation mode",
                tranType.tranTypeCoSElevator => "Elevator relay",
                tranType.tranTypeFileDownloadStatus => "File download",
                tranType.tranTypeCoSElevatorAccess => "Elevator floor",
                tranType.tranTypeAcrExtFeatureStls => "door EXT feature",
                tranType.tranTypeAcrExtFeatureCoS => "door EXT status",
                tranType.tranTypeAsci => "ASCII Msg diagnostics",
                tranType.tranTypeSioDiag => "modules diagnostics",
                _ => "Unknown"
            };
        }

        #endregion

        #region TranCodeDesc

        public static string[] TranCodeDesc(tranType type,int code) 
        {
            switch (type)
            {
                case tranType.tranTypeSys:
                    return code switch
                    {
                        1 => ["Power Up","SCP power-up diagnostics"],
                        2 => ["Offline","Host communications offline"],
                        3 => ["Online","Host communications online"],
                        4 => ["Exceed Limit","transaction count exceeds the preset limit"],
                        5 => ["Config Saved","Configuration database save complete"],
                        6 => ["Card Saved","Card database save complete"],
                        7 => ["Card Cleared","Card database cleared due to SRAM buffer overflow"],
                        _ => ["",""]
                    };
                case tranType.tranTypeSioComm:
                    return code switch
                    {
                        1 =>["Disabled", "ommunication disabled (result of host command)"],
                        2 => ["Offline", "Timeout (no/bad response from unit)"],
                        3 => ["Offline", "Invalid identification from modules"],
                        4 => ["Offline","command too long"],
                        5 => ["Online","Normal connection"],
                        6 => ["HexLoad", "ser_num is address loaded (-1 = last record)"],
                        _ => ["",""]
                    };
                case tranType.tranTypeCardBin:
                    return code switch
                    {
                        1 => ["Access denied", "Invalid card format"],
                        _ => ["",""]
                    };
                case tranType.tranTypeCardBcd:
                    return code switch
                    {
                        1 => ["Access denied", "Invalid card format, forward read"],
                        _ => ["Access denied", "Invalid card format, reverse read"]
                    };
                case tranType.tranTypeCardFull:
                case tranType.tranTypeDblCardFull:
                case tranType.tranTypeI64CardFull:
                case tranType.tranTypeI64CardFullIc32:
                    return code switch
                    {
                        1 => ["Request rejected", "Access point \"locked\""],
                        2 => ["Request accepted", "Access point \"unlocked\""],
                        3 => ["Request rejected", "Invalid facility code"],
                        4 => ["Request rejected", "Invalid facility code extension"],
                        5 => ["Request rejected", "Not in card file"],
                        6 => ["Request rejected", "Invalid issue code"],
                        7 => ["Request granted", "facility code verified, not used"],
                        8 => ["Request granted", "facility code verified, door used"],
                        9 => ["Access denied", "Asked for host approval, then timed out"],
                        10 => ["Host approval pending", "Card is about to get access granted (waiting for command 329 host response)"],
                        11 => ["Access denied", "Access denied count exceeded"],
                        12 => ["Access denied", "Asked for host approval, then host denied"],
                        13 => ["Request rejected", "Airlock is busy"],
                        _ => ["", ""]
                    };
                case tranType.tranTypeCardID:
                case tranType.tranTypeDblCardID:
                case tranType.tranTypeI64CardID:
                    return code switch
                    {
                        1 => ["Request rejected", "Deactivated card"],
                        2 => ["Request rejected", "Before activation date"],
                        3 => ["Request rejected", "After expiration date"],
                        4 => ["Request rejected", "Invalid time"],
                        5 => ["Request rejected", "Invalid PIN"],
                        6 => ["Request rejected", "Anti-passback violation"],
                        7 => ["Request granted", "APB violation, not used"],
                        8 => ["Request granted", "APB violation, used"],
                        9 => ["Request rejected", "Duress code detected"],
                        10 => ["Request granted", "Duress, used"],
                        11 => ["Request granted", "Duress, not used"],
                        12 => ["Request granted", "Full test, not used"],
                        13 => ["Request granted", "Full test, used"],
                        14 => ["Request denied", "Never allowed at this reader (all Tz's = 0)"],
                        15 => ["Request denied", "No second card presented"],
                        16 => ["Request denied", "Occupancy limit reached"],
                        17 => ["Request denied", "The area is NOT enabled"],
                        18 => ["Request denied", "Use limit exceeded"],
                        21 => ["Access granting in progress", "Used/not used transaction will follow"],
                        24 => ["Request rejected", "No escort card presented"],
                        25 => ["Reserved","Reserved"],
                        26 => ["Reserved", "Reserved"],
                        27 => ["Reserved", "Reserved"],
                        29 => ["Request rejected", "Airlock is busy"],
                        30 => ["Request rejected", "Incomplete CARD & PIN sequence"],
                        31 => ["Request granted", "Double-card event"],
                        32 => ["Request granted", "Double-card event while in uncontrolled state (locked/unlocked)"],
                        39 => ["Access granting in progress", "Requires escort, pending escort card"],
                        40 => ["Request rejected", "Violates minimum occupancy count"],
                        41 => ["Request rejected", "Card pending at another reader"],
                        _ => ["", ""]
                    };
                case tranType.tranTypeHostCardFullPin:
                    return code switch
                    {
                        1 => ["Requesting access", "Waiting for host response"],
                        _ => ["",""]
                    };
                case tranType.tranTypeCoS:
                    return code switch
                    {
                        1 => ["Disconnected", "Disconnected (from an input point ID)"],
                        2 => ["Offline", ""],
                        3 => ["Secure", ""],
                        4 => ["Alarm", ""],
                        5 => ["Fault",""],
                        6 => ["Exit delay in progress",""],
                        7 => ["Entry delay in progress",""],
                        _ => ["",""]
                    };
                case tranType.tranTypeREX:
                    return code switch
                    {
                        1 => ["Exit cycle", "door use not verified"],
                        2 => ["Exit cycle","door not used"],
                        3 => ["Exit cycle","door used"],
                        4 => ["Host initiated request", "door use not verified"],
                        5 => ["Host initiated request", "door not used"],
                        6 => ["Host initiated request", "door used"],
                        9 => ["Exit Cycle","Started"],
                        _ => ["",""]
                    };
                case tranType.tranTypeCoSDoor:
                    return code switch
                    {
                        1 => ["Disconnected",""],
                        2 => ["Unknown_RS", "Unknown _RS bits: last known status"],
                        3 => ["Secure",""],
                        4 => ["Alarm", ""],
                        5 => ["Fault", "Fault (fault type is encoded in door_status byte)"],
                        _ => ["",""]
                    };
                case tranType.tranTypeProcedure:
                    return code switch
                    {
                        1 => ["Cancel", "Cancel procedure (abort delay)"],
                        2 => ["Execute", "Execute procedure (start new)"],
                        3 => ["Resume", "Resume procedure, if paused"],
                        4 => ["Execute", "Execute procedure with prefix 256 actions"],
                        5 => ["Execute", "Execute procedure with prefix 512 actions"],
                        6 => ["Execute", "Execute procedure with prefix 1024 actions"],
                        7 => ["Resume", "Resume procedure with prefix 256 actions"],
                        8 => ["Resume", "Resume procedure with prefix 512 actions"],
                        9 => ["Resume", "Resume procedure with prefix 1024 actions"],
                        10 => ["Issued", "command was issued to procedure with no actions - (NOP)"],
                        _ => ["",""]
                    };
                case tranType.tranTypeUserCmnd:
                    return code switch
                    {
                        1 => ["command","command entered by the user"],
                        _ => ["", ""]
                    };
                case tranType.tranTypeActivate:
                    return code switch
                    {
                        1 => ["Inactive", "Became inactive"],
                        2 => ["Active", "Became active"],
                        _ => ["",""]
                    };
                case tranType.tranTypeAcr:
                    return code switch
                    {
                        1 => ["Disabled",""],
                        2 => ["Unlocked", ""],
                        3 => ["Locked", "Exit request enabled"],
                        4 => ["FAC only", ""],
                        5 => ["Card only", ""],
                        6 => ["PIN only", ""],
                        7 => ["Card and PIN", ""],
                        8 => ["PIN or card", ""],
                        _ => ["",""]
                    };
                case tranType.tranTypeMpg:
                    return code switch
                    {
                        1 => ["Executed", "First disarm command executed (mask_count was 0, all MPs got masked)"],
                        2 => ["Executed", "Subsequent disarm command executed (mask_count incremented, MPs already masked)"],
                        3 => ["Armed", "Override command: armed (mask_count cleared, all points unmasked)"],
                        4 => ["Disarmed", "Override command: disarmed (mask_count set, unmasked all points)"],
                        5 => ["MPG armed", "Force arm command, MPG armed, (may have active zones, mask_count is now zero)"],
                        6 => ["MPG not armed", "Force arm command, MPG not armed (mask_count decremented)"],
                        7 => ["MPG armed", "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)"],
                        8 => ["MPG did not arm", "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)"],
                        9 => ["MPG still armed", "Standard arm command, MPG still armed, (mask_count decremented)"],
                        10 => ["MPG armed", "Override arm command, MPG armed (mask_count is now zero)"],
                        11 => ["MPG did not arm", "Override arm command, MPG did not arm, (mask_count decremented)"],
                        _ => ["",""]
                    };
                case tranType.tranTypeArea:
                    return code switch
                    {
                        1 => ["Disabled", "Area disabled"],
                        2 => ["Enabled", "Area enabled"],
                        3 => ["Zero", "Occupancy count reached zero"],
                        4 => ["Reach Max", "Occupancy count reached the \"downward-limit\""],
                        5 => ["Reach Min", "Occupancy count reached the \"upward-limit\""],
                        6 => ["Exceed Limit", "Occupancy count reached the \"max-occupancy-limit\""],
                        7 => ["mode change", "Multi-occupancy mode changed"],
                        _ => ["",""]
                    };
                case tranType.tranTypeUseLimit:
                    return code switch
                    {
                        1 => ["Use limit changed", "Use limit changed, reporting new limit"],
                        _ => ["",""]
                    };
                case tranType.tranTypeWebActivity:
                    return code switch
                    {
                        1 => ["Web", "Save home notes"],
                        2 => ["Web", "Save network settings"],
                        3 => ["Web", "Save host communication settings"],
                        4 => ["Web", "Add user"],
                        5 => ["Web", "Delete user"],
                        6 => ["Web", "Modify user"],
                        7 => ["Web", "Save password strength and session timer"],
                        8 => ["Web", "Save web server options"],
                        9 => ["Web", "Save time server settings"],
                        10 => ["Web", "Auto save timer settings"],
                        11 => ["Web", "Load certificate"],
                        12 => ["Web", "Logged out by link"],
                        13 => ["Web", "Logged out by timeout"],
                        14 => ["Web", "Logged out by user"],
                        15 => ["Web", "Logged out by apply"],
                        16 => ["Web", "Invalid login"],
                        17 => ["Web", "Successful login"],
                        18 => ["Web", "Network diagnostic saved"],
                        19 => ["Web", "Card DB size saved"],
                        21 => ["Web", "Diagnostic page saved"],
                        22 => ["Web", "Security options page saved"],
                        23 => ["Web", "Add-on package page saved"],
                        24 => ["Web", "Not used"],
                        25 => ["Web", "Not used"],
                        26 => ["Web", "Not used"],
                        27 => ["Web", "Invalid login limit reached"],
                        28 => ["Web", "firmware download initiated"],
                        29 => ["Web", "Advanced networking routes saved"],
                        30 => ["Web", "Advanced networking reversion timer started"],
                        31 => ["Web", "Advanced networking reversion timer elapsed"],
                        32 => ["Web", "Advanced networking route changes reverted"],
                        33 => ["Web", "Advanced networking route changes cleared"],
                        34 => ["Web", "Certificate generation started"],
                        _ => ["", ""]
                    };
                case tranType.tranTypeOperatingMode:
                    return code switch
                    {
                        1 => ["mode changed", "Operating mode changed to mode 0"],
                        2 => ["mode changed", "Operating mode changed to mode 1"],
                        3 => ["mode changed", "Operating mode changed to mode 2"],
                        4 => ["mode changed", "Operating mode changed to mode 3"],
                        5 => ["mode changed", "Operating mode changed to mode 4"],
                        6 => ["mode changed", "Operating mode changed to mode 5"],
                        7 => ["mode changed", "Operating mode changed to mode 6"],
                        8 => ["mode changed", "Operating mode changed to mode 7"],
                        _ => ["", ""]
                    };
                case tranType.tranTypeCoSElevator:
                    return code switch
                    {
                        1 => ["Secure", "Floor status is secure"],
                        2 => ["Public", "Floor status is public"],
                        3 => ["Disabled", "Floor status is disabled (override)"],
                        _ => ["",""]
                    };
                case tranType.tranTypeFileDownloadStatus:
                    return code switch
                    {
                        1 => ["Success", "File transfer success"],
                        2 => ["Error", "File transfer error"],
                        3 => ["Success", "File delete successful"],
                        4 => ["Error", "File delete unsuccessful"],
                        5 => ["Completed", "OSDP file transfer complete (primary ACR) - look at source number for ACR number"],
                        6 => ["Error", "OSDP file transfer error (primary ACR) - look at source number for ACR number"],
                        7 => ["Completed", "OSDP file transfer complete (alternate ACR) - look at source number for ACR number"],
                        8 => ["Error", "OSDP file transfer error (alternate ACR) - look at source number for ACR number"],
                        _ => ["",""]
                    };
                case tranType.tranTypeCoSElevatorAccess:
                    return code switch
                    {
                        1 => ["Success","Elevator Success"],
                        _ => ["",""]
                    };
                case tranType.tranTypeAcrExtFeatureStls:
                    return code switch
                    {
                        1 => ["Updated", "Extended status updated"],
                        _ => ["",""]
                    };
                case tranType.tranTypeAcrExtFeatureCoS:
                    return code switch
                    {
                        3 => ["Secure",""],
                        4 => ["Alarm",""],
                        _ => ["",""]
                    };
                default:
                    return ["", ""];
                
            }
        }

        #endregion

        #region TypeSys


        // public static string TypeSysCommToText(TypeSysComm v)
        // {
        //     return v switch
        //     {
        //         0 => "Offline",
        //         1 => "Online",
        //         2 => "Standby",
        //         _ => ""

        //     };
        // }

        public static List<TransactionFlag> TypeSysErrorFlag(short error_code)
        {
            var flag = new List<TransactionFlag>();
            if (UtilitiesHelper.IsBitSet(error_code, 2)) flag.Add(new TransactionFlag { Topic="Error flag", Name = "External Reset", Description = "The reset button was pressed" });
            if (UtilitiesHelper.IsBitSet(error_code, 3)) flag.Add(new TransactionFlag { Topic = "Error flag", Name = "Power on Reset", Description = "A reset that occurs during power up." });
            if (UtilitiesHelper.IsBitSet(error_code, 4)) flag.Add(new TransactionFlag { Topic = "Error flag", Name = "Watchdog Timer", Description = "A watchdog reset occurs when the CPU detects that a task is hogging the CPU for an extended period, so it performs a reboot to correct the issue." });
            if (UtilitiesHelper.IsBitSet(error_code, 5)) flag.Add(new TransactionFlag { Topic = "Error flag", Name = "Software", Description = "Software caused the reset to occur. This can be through a valid means where the board needed to be rebooted, for example during a firmware download or applying settings on the web page, or could occur due to a software crash" });
            if (UtilitiesHelper.IsBitSet(error_code, 6)) flag.Add(new TransactionFlag { Topic = "Error flag", Name = "Low Voltage", Description = "The reset was caused by a low voltage detection" });
            if (UtilitiesHelper.IsBitSet(error_code, 7)) flag.Add(new TransactionFlag { Topic = "Error flag", Name = "Fault", Description = "A software fault/crash caused the reset" });
            return flag;
        }

        #endregion

        #region TypeFileDownloadStatus

        public static string FileTypeToText(byte a)
        {
            return a switch
            {
                0 => "Host Comm certificate file",
                1 => "User defined file",
                2 => "HID License file",
                3 => "Peer certificate",
                4 => "OSDP file transfer files",
                7 => "Linq certificate",
                8 => "Over-Watch certificate",
                9 => "Web server certificate",
                10 => "HID Origo™ certificate",
                11 => "Aperio certificate",
                12 => "Host translator service for OEM cloud certificate",
                13 => "Driver trust store",
                16 => "802.1x TLS authentication",
                18 => "HTS OEM cloud authentication",
                _ => "Unknown file type"
            };
        }

        #endregion

        #region TypeCos

        private static string TypeCoseStatusFirstThree(short status,short Source,tranSrc src)
        {
            status = (short)(status & 0x07);
            if (src == tranSrc.tranSrcAcrTmpr)
            {
                return status switch
                {
                    0 => "Online, reader tamper inactive",
                    1 => "Online, reader tamper active",
                    2 => "N/A",
                    3 => "Communication broken (reader offline)",
                    _ => ""
                };
            }
            if (Source == 5 || Source == 6)
            {
                return status switch
                {
                    0 => "Fault input inactive, local battery = good",
                    1 => "Fault input active, local battery = good",
                    2 => "Fault input inactive, local battery = low",
                    3 => "Fault input active, local battery = low",
                    _ => ""
                };
            }
            return status switch
            {
                0 => "inactive",
                1 => "active",
                2 => "ground fault",
                3 => "short",
                4 => "open circuit",
                5 => "foreign voltage",
                6 => "non-settling error",
                7 => "supervisory fault codes",
                _ => ""
            };
        }

        public static List<TransactionFlag> TypeCosStatus(short status,short Source, tranSrc src)
        {
            var flag = new List<TransactionFlag>();
            flag.Add(new TransactionFlag { Name = TypeCoseStatusFirstThree(status, Source,src), Description = TypeCoseStatusFirstThree(status, Source,src) });
            if (UtilitiesHelper.IsBitSet(status, 0x08)) flag.Add(new TransactionFlag { Topic="status", Name = "Offline", Description = "Communication to the input point is not valid" });
            if (UtilitiesHelper.IsBitSet(status, 0x10)) flag.Add(new TransactionFlag { Topic = "status", Name = "Mask flag", Description = "Set if the monitor point is MASKed" });
            if (UtilitiesHelper.IsBitSet(status, 0x20)) flag.Add(new TransactionFlag { Topic = "status", Name = "Local mask flag", Description = "Entry or exit delay in progress" });
            if (UtilitiesHelper.IsBitSet(status, 0x40)) flag.Add(new TransactionFlag { Topic = "status", Name = "Entry delay in progress", Description = "Entry delay in progress" });
            if (UtilitiesHelper.IsBitSet(status, 0x80)) flag.Add(new TransactionFlag { Topic = "status", Name = "Not attached ", Description = "he monitor point is not linked to an Input" });

            return flag;
        }

        #endregion

        #region TypeSioComm

        public static string TypeSioCommStatus(short status)
        {
            return status switch
            {
                0 => "not configured",
                1 => "not tried: active, have not tried to poll it",
                2 => "offline",
                3 => "online"

            };
        }

        public static string TypeSioCommModel(short model,short status)
        {
            if(status == 2)
            {
                return model switch
                {
                    0 => "no error",
                    1 => "timeout",
                    2 => "length - Rx too long",
                    3 => "length - Rx to SIO was NAK'ed",
                    4 => "checksum error",
                    5 => "sequence error",
                    6 => "busy(not a real error,system will retry)",
                    _ => ""
                };
            }

            return model switch
            {
                196 => "HID Aero x1100",
                193 => "HID Aero x100",
                194 => "HID Aero x200",
                195 => "HID Aero x300",
                190 => "VertX v100",
                191 => "VertX v200",
                192 => "VertX V300",
                57 => "Aperio AH40 IP Hub",
                _ => "Unknown"
            };
        }

        public static string TypeSioCommHardwareId(short id) 
        {
            return id switch
            {
                217 => "HID Aero x1100",
                218 => "HID Aero x100",
                228 => "HID Aero x100B",
                219 => "HID Aero x200",
                233 => "HID Aero x200B",
                220 => "HID Aero x300",
                234 => "HID Aero x300B",
                0 => "Vert X",
                _ => "Unknown"
            };
        }

        public static string TypeSioCommEncConfig(short i)
        {
            return i switch
            {
                0 => "None",
                1 => "AES Default Key",
                2 => "AES Master/Secret Key",
                3 => "PKI(Reserved)",
                6 => "AES 256 Session Key derived from default key",
                _ => "Unknown"
            };
        }

        public static string TypeSioCommEncKeyStatus(short i)
        {
            return i switch
            {
                0 => "Not loaded to controller",
                1 => "Loaded to controller, unverified w/SIO",
                2 => "Loaded to controller, conflicts w/SIO",
                3 => "Loaded to controller, verified w/ SIO",
                4 => "AES 256 Verified",
                _ => "Unknown"
            };
        }

        public static List<TransactionFlag> TypeSioHardwareComponent(int i)
        {
            var low = i & 0x0F;
            var high = (i >> 4) & 0x0F;
            var flag = new List<TransactionFlag>();
            flag.Add(new TransactionFlag { Topic="hardware HardwareComponent", Name = "HARDWARE_COMP_CRYPTO", Description = TypeSioCommCrypto(low) });
            flag.Add(new TransactionFlag { Topic = "hardware HardwareComponent", Name = "HARDWARE_COMP_PHY", Description = TypeSioCommPhy(high) });
            return flag;
            
            
        }

        private static string TypeSioCommCrypto(int i)
        {
            return i switch
            {
                0 => "Driver Version < 4.7.1.34",
                1 => "Board has AES132 crypto chip",
                2 => "Board has ATSHA204 crypto chip",
                3 => "Board has ATSHA206 Crypto chip",
                4 => "Board has Renesas chip"
            };
        }

        private static string TypeSioCommPhy(int i)
        {
            return i switch
            {
                0 => "Driver Version < 4.7.1.34",
                1 => "Board has TI PHY",
                2 => "Board has Wiznet PHY"
            };
        }


        #endregion

        #region TyprCardID

        public static List<TransactionFlag> TypeCardIDCardTypeFlag(short i)
        {
            var flag = new List<TransactionFlag>();
            if (UtilitiesHelper.IsBitSet(i, 0)) flag.Add(new TransactionFlag { Name = "Escort", Description = "Escort" });
            if (UtilitiesHelper.IsBitSet(i, 1)) flag.Add(new TransactionFlag { Name = "Requires Escort", Description = "Requires Escort" });
            return flag;
        }


        #endregion

        #region TypeAcr

        public static List<TransactionFlag> TypeAcrAccessControlFlag(short s)
        {
            var flags = new List<TransactionFlag>();

            if ((s & 0x0001) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_DCR", Description = "Decrement use limits on access" });

            if ((s & 0x0002) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_CUL", Description = "Require use limit to be non-zero" });

            if ((s & 0x0004) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_DRSS", Description = "Deny duress request instead of granting" });

            if ((s & 0x0008) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_ALLUSED", Description = "Log access as used immediately" });

            if ((s & 0x0010) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_QEXIT", Description = "Do not pulse door strike on REX (quiet exit)" });

            if ((s & 0x0020) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_FILTER", Description = "Filter change-of-state door transactions" });

            if ((s & 0x0040) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_2CARD", Description = "Require two-card control at this reader" });

            if ((s & 0x0400) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_HOST_CBG", Description = "Check with HOST before granting access (online)" });

            if ((s & 0x0800) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_HOST_SFT", Description = "If HOST is offline/timeouts, still grant access" });

            if ((s & 0x1000) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_CIPHER", Description = "Enable cipher keypad mode for card entry" });

            if ((s & 0x4000) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_LOG_EARLY", Description = "Log access grant immediately, then used/not-used" });

            if ((s & 0x8000) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_F_CNIF_WAIT", Description = "Show 'wait' pattern for card-not-in-file" });

            return flags;
        }

        public static List<TransactionFlag> TypeAcrSpareFlag(short s)
        {
            var flags = new List<TransactionFlag>();

            if ((s & 0x0001) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_NOEXTEND", Description = "Do not resume the extended door held open timer on new access grant" });

            if ((s & 0x0002) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_NOPINCARD", Description = "Card+PIN mode: Do not accept PIN followed by card (card must be first)" });

            if ((s & 0x0008) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_DFO_FLTR", Description = "Enable door Forced Open Filter (door opens within 3 seconds of closing won't count)" });

            if ((s & 0x0010) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_NO_ARQ", Description = "Do not process access requests; all requests reported as ‘Access Denied, door Locked’" });

            if ((s & 0x0020) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_SHNTRLY", Description = "Enable shunt relay behavior linked to door unlock and close timing" });

            if ((s & 0x0040) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_FLOOR_PIN", Description = "Enable floor/output selection via PIN when in elevator hardware_type 1 + Card+PIN mode" });

            if ((s & 0x0080) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_LINK_MODE", Description = "ACR is in linking mode (acr_mode 29 starts, 30 aborts)" });

            if ((s & 0x0100) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_DCARD", Description = "Enable double card function (two valid reads within 5 seconds)" });

            if ((s & 0x0200) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_OVERRIDE", Description = "ACR is in a temporary mode override" });

            if ((s & 0x0400) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_CRD_OVR_EN", Description = "Allow override credential to gain access even when locked" });

            if ((s & 0x0800) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_ELV_DISABLE", Description = "ACR supports disabling elevator floors via offline_mode (hardware_type 1 & 2 only)" });

            if ((s & 0x1000) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_LINK_MODE_ALT", Description = "ACR is in alternate reader linking mode (acr_mode 32/33)" });

            if ((s & 0x2000) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_REX_HOLD", Description = "extend REX grant time while REX input is active" });

            if ((s & 0x4000) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_HOST_BYPASS", Description = "Bypass local DB and use HOST for grant decisions (requires ACR_F_HOST_CBG)" });

            if ((s & 0x4000) != 0)
                flags.Add(new TransactionFlag { Name = "ACR_FE_REX_EARLYTXN", Description = "flag to enable generating a transaction at the start of the REX cycle." });

            return flags;
        }

        public static string TypeAcrMode(short s) 
        {
            return s switch
            {
                1 => "Disabled",
                2 => "Unlocked",
                3 => "Locked (exit request enabled)",
                4 => "facility code only",
                5 => "Card only",
                6 => "PIN only",
                7 => "Card and PIN",
                8 => "PIN or Card",
                _ => "Unknown"
            };
        }



        #endregion

        #region TypeCosElevator

        public static string TypeCosElevatorFloorStatus(byte s) 
        {
            return s switch
            {
                1 => "Secure",
                2 => "Public",
                3 => "Disabled"
            };
        }

        #endregion

        #region TypeAcrExtFeatureCoS

        public static string TypeAcrExtFeatureCoSFeaturePoint(short s)
        {
            return s switch
            {
                0 => "Interior Push Button (IPB) active",
                1 => "Deadbolt engaged",
                11 => "Escape & Return unlock is active",
                _ => "Unknown"
            };
        }

        #endregion

        #region TypeCosDoor

        public static List<TransactionFlag> TypeCosDoorAccessPointStatus(short s)
        {
            var flags = new List<TransactionFlag>();
            if ((s & 0x01) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "unlocked" });
            if ((s & 0x02) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "access (exit) cycle in progress" });
            if ((s & 0x04) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "forced open" });
            if ((s & 0x08) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "forced open mask" });
            if ((s & 0x10) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "held open" });
            if ((s & 0x20) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "held open mask" });
            if ((s & 0x40) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "held open pre-alarm status" });
            if ((s & 0x80) != 0) flags.Add(new TransactionFlag { Name = "Ap", Description = "door is in \"extended held open\" mode" });
            return flags;
        }

        #endregion

        #region TypeArea

        public static string TypeAreaStatus(short s)
        {
            return s switch
            {
                1 => "Enabled",
                2 => "Require Multiple Occupancy",
                128 => "NOT been configured ",
                _ => ""
            };
        }

        #endregion
    }
}
