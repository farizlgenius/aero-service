using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using HIDAeroService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDAeroService.AeroLibrary
{
    public sealed class ProcessTransaction
    {

        
        public ProcessTransaction() 
        {
            
        }
        /*
         * ser_num
         * time
         * source_type
         * source_number
         * tran_type
         * tran_code
         */
        private static void BaseTransactionProcess(SCPReplyMessage message,string tranCodeDesc)
        {
            string typeDesc = Description.GetTypeDesc(message.tran.tran_type);
            string sourceDesc = Description.GetSourceDesc(message.tran.source_type);
            Console.WriteLine($"ser_num: {message.tran.ser_num}, {DateTimeOffset.FromUnixTimeMilliseconds(message.tran.time).UtcDateTime}, source_desc: {sourceDesc}, source_number: {message.tran.source_number}, tran_type_desc: {typeDesc}, tran_code_desc: {message.tran.tran_code} {tranCodeDesc}");
        }

        public static void ProcessTypeSys(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeSysDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"error_code: {message.tran.sys_comm.error_code}");
            Console.WriteLine($"current_primary_comm: {message.tran.sys_comm.current_alternate_comm}");
            Console.WriteLine($"previous_primary_comm: {message.tran.sys_comm.previous_alternate_comm}");
            Console.WriteLine("###### transaction extend_desc End ######");

        }

        public static void ProcessTypeSioComm(SCPReplyMessage message) 
        {

            string codeDesc = Description.GetTranCodeTypeSioCommDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"error_code (ref from tran_code): {message.tran.tran_code}");
            Console.WriteLine($"comm_sts: {message.tran.s_comm.comm_sts}");
            Console.WriteLine($"model: {message.tran.s_comm.model}");
            Console.WriteLine($"revision: {message.tran.s_comm.revision}");
            Console.WriteLine($"ser_num: {message.tran.s_comm.ser_num}");
            Console.WriteLine($"nExtendedInfoValid: {message.tran.s_comm.nExtendedInfoValid}");
            Console.WriteLine($"n_hardware_id: {Description.GetNHardwareIdDesc(message.tran.s_comm.nHardwareId)}");
            Console.WriteLine($"n_hardware_rev: {message.tran.s_comm.nHardwareRev}");
            Console.WriteLine($"n_product_id: {message.tran.s_comm.nProductId}");
            Console.WriteLine($"n_product_ver: {message.tran.s_comm.nProductVer}");
            Console.WriteLine($"nFirmwareBoot: {message.tran.s_comm.nFirmwareBoot}");
            Console.WriteLine($"nFirmwareLdr: {message.tran.s_comm.nFirmwareLdr}");
            Console.WriteLine($"nFirmwareApp: {message.tran.s_comm.nFirmwareApp}");
            Console.WriteLine($"nOemCode: {message.tran.s_comm.nOemCode}");
            Console.WriteLine($"n_enc_config: {message.tran.s_comm.nEncConfig}");
            Console.WriteLine($"n_enc_key_status: {message.tran.s_comm.nEncKeyStatus}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypeCardBin(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardBinDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            string additional = String.Format("bit: {0},data: {1}", message.tran.c_bin.bit_count, UtilityHelper.ByteToHex(message.tran.c_bin.bit_array));
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"bit_count: {message.tran.c_bin.bit_count}");
            Console.WriteLine($"bit_array: {UtilityHelper.ByteToHex(message.tran.c_bin.bit_array)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypeCardBcd(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardBcdDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"digit_count: {message.tran.c_bcd.digit_count}");
            Console.WriteLine($"bcd_array: {UtilityHelper.ByteToHexStr(message.tran.c_bcd.bcd_array)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypeCardFull(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardFullDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"format_number: {message.tran.c_full.format_number}");
            Console.WriteLine($"facility_code: {message.tran.c_full.facility_code}");
            Console.WriteLine($"cardholder_id: {message.tran.c_full.cardholder_id}");
            Console.WriteLine($"issue_code: {message.tran.c_full.issue_code}");
            Console.WriteLine($"floor_number: {message.tran.c_full.floor_number}");
            Console.WriteLine($"encoded_card: {UtilityHelper.ByteToHexStr(message.tran.c_full.encoded_card)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypeDblCardFull(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardFullDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"format_number: {message.tran.c_fulldbl.format_number}");
            Console.WriteLine($"facility_code: {message.tran.c_fulldbl.facility_code}");
            Console.WriteLine($"cardholder_id: {message.tran.c_fulldbl.cardholder_id}");
            Console.WriteLine($"issue_code: {message.tran.c_fulldbl.issue_code}");
            Console.WriteLine($"floor_number: {message.tran.c_fulldbl.floor_number}");
            Console.WriteLine($"encoded_card: {UtilityHelper.ByteToHexStr(message.tran.c_fulldbl.encoded_card)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypei64CardFull(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardFullDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"format_number: {message.tran.c_fulli64.format_number}");
            Console.WriteLine($"facility_code: {message.tran.c_fulli64.facility_code}");
            Console.WriteLine($"cardholder_id: {message.tran.c_fulli64.cardholder_id}");
            Console.WriteLine($"issue_code: {message.tran.c_fulli64.issue_code}");
            Console.WriteLine($"floor_number: {message.tran.c_fulli64.floor_number}");
            Console.WriteLine($"encoded_card: {UtilityHelper.ByteToHexStr(message.tran.c_fulli64.encoded_card)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypei64CardFullc32(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardFullDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"format_number: {message.tran.c_fulli64i32.format_number}");
            Console.WriteLine($"facility_code: {message.tran.c_fulli64i32.facility_code}");
            Console.WriteLine($"cardholder_id: {message.tran.c_fulli64i32.cardholder_id}");
            Console.WriteLine($"issue_code: {message.tran.c_fulli64i32.issue_code}");
            Console.WriteLine($"floor_number: {message.tran.c_fulli64i32.floor_number}");
            Console.WriteLine($"encoded_card: {UtilityHelper.ByteToHexStr(message.tran.c_fulli64i32.encoded_card)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypeCardID(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardIDDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"format_number: {message.tran.c_id.format_number}");
            Console.WriteLine($"cardholder_id: {message.tran.c_id.cardholder_id}");
            Console.WriteLine($"floor_number: {message.tran.c_id.floor_number}");
            Console.WriteLine($"card_type_flags: {message.tran.c_id.card_type_flags}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void ProcessTypeDblCardID(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardIDDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"format_number: {message.tran.c_iddbl.format_number}");
            Console.WriteLine($"cardholder_id: {message.tran.c_iddbl.cardholder_id}");
            Console.WriteLine($"floor_number: {message.tran.c_iddbl.floor_number}");
            Console.WriteLine($"card_type_flags: {message.tran.c_iddbl.card_type_flags}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypei64CardID(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCardIDDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"format_number: {message.tran.c_idi64.format_number}");
            Console.WriteLine($"cardholder_id: {message.tran.c_idi64.cardholder_id}");
            Console.WriteLine($"floor_number: {message.tran.c_idi64.floor_number}");
            Console.WriteLine($"card_type_flags: {message.tran.c_idi64.card_type_flags}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeCos(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCosDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"status: {message.tran.cos.status}");
            Console.WriteLine($"status_desc: {Description.DecodeStatusTypeCoS(message.tran.cos.status,message.tran.source_type)}");
            Console.WriteLine($"old_sts: {message.tran.cos.old_sts}");
            Console.WriteLine("###### transaction extend_desc End ######");

        }

        public static void tranTypeRex(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeREXDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"rex_number: {message.tran.rex.rex_number}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeCosDoor(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCosDoorDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"door_status: {message.tran.door.door_status}");
            Console.WriteLine($"door_status_desc: {Description.GetStatusTypeCosDoorDesc(message.tran.door.door_status)}");
            Console.WriteLine($"ap_prior: {message.tran.door.ap_prior}");
            Console.WriteLine($"ap_prior_desc: {Description.GetStatusTypeCosDoorDesc(message.tran.door.ap_prior)}");
            Console.WriteLine($"door_prior: {message.tran.door.door_prior}");
            Console.WriteLine($"door_prior_desc: {Description.GetStatusTypeCosDoorDesc(message.tran.door.door_prior)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeProcedure(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeProcedureDesc(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"tran_code: {message.tran.proc}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeUserCmnd(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeUserCmnd(message.tran.tran_code);
            BaseTransactionProcess(message, "command entered by the user");
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"tran_code: {message.tran.tran_code}");
            Console.WriteLine("###### transaction extend_desc End ######");

        }

        public static void tranTypeActivate(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeActivate(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"tran_code: {message.tran.tran_code}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeAcr(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeAcr(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"actl_flags: {message.tran.acr.actl_flags}");
            Console.WriteLine($"actl_flags_desc: {Description.GetAccessControlFlagDesc(message.tran.acr.actl_flags)}");
            Console.WriteLine($"prior_flags: {message.tran.acr.prior_flags}");
            Console.WriteLine($"prior_flags_desc: {Description.GetAccessControlFlagDesc(message.tran.acr.prior_flags)}");
            Console.WriteLine($"prior_mode: {message.tran.acr.prior_mode}");
            Console.WriteLine($"actl_flags_e: {message.tran.acr.actl_flags_e}");
            Console.WriteLine($"actl_flags_e_desc: {Description.GetAccessControlFlagDesc(message.tran.acr.actl_flags_e)}");
            Console.WriteLine($"prior_flags_e: {message.tran.acr.prior_flags_e}");
            Console.WriteLine($"prior_flags_e_desc: {Description.GetAccessControlFlagDesc(message.tran.acr.prior_flags_e)}");
            Console.WriteLine($"auth_mod_flags: {message.tran.acr.auth_mod_flags}");
            Console.WriteLine($"prior_auth_mod_flags: {message.tran.acr.prior_auth_mod_flags}");
            Console.WriteLine("###### transaction extend_desc End ######"); 
        }

        public static void tranTypeMpg(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeMpg(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"mask_count: {message.tran.mpg.mask_count}");
            Console.WriteLine($"nActiveMps: {message.tran.mpg.nActiveMps}");
            Console.Write("n_mp_list: ");
            foreach(short s in message.tran.mpg.nMpList)
            {
                Console.Write(s + ", ");
            }
            Console.WriteLine("");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeArea(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeArea(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"status: {message.tran.area.status}");
            Console.WriteLine($"status_desc: {Description.GetTypeAreaStatusDesc(message.tran.area.status)}");
            Console.WriteLine($"occupancy: {message.tran.area.occupancy}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeUseLimit(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeUseLimit(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"use_count: {message.tran.c_uselimit.use_count}");
            Console.WriteLine($"cardholder_id: {message.tran.c_uselimit.cardholder_id}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeWebActivity(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeWebActivity(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"iType: {message.tran.web_activity.iType}");
            Console.WriteLine($"iCurUserId: {message.tran.web_activity.iCurUserId}");
            Console.WriteLine($"iCurUserId: {message.tran.web_activity.iObjectUserId}");
            Console.WriteLine($"szObjectUser: {message.tran.web_activity.szObjectUser}");
            Console.WriteLine($"ipAddress: {message.tran.web_activity.ipAddress}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeOperatingMode(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeOperatingMode(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"prev_oper: {message.tran.oper_mode.prev_oper}");
            Console.WriteLine($"prev_oper_desc: {Description.GetTranCodeTypeOperatingMode(message.tran.oper_mode.prev_oper)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeCoSElevator(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCosElevator(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"prevFloorStatus: {Description.GetTranCodeTypeOperatingMode(message.tran.floor.prevFloorStatus)}");
            Console.WriteLine($"floorNumber: {message.tran.floor.floorNumber}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeFileDownloadStatus(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeFileDownloadStatus(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"fileType: {message.tran.file_download.fileType}");
            Console.WriteLine($"fileName: {message.tran.file_download.fileName}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeCoSElevatorAccess(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeCoSElevatorAccess(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"cardholder_id: {message.tran.elev_access.cardholder_id}");
            Console.WriteLine($"floor: {UtilityHelper.ByteToHexStr(message.tran.elev_access.floors)}");
            Console.WriteLine($"nCardFormat: {message.tran.elev_access.nCardFormat}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeAcrExtFeatureStls(SCPReplyMessage message)
        {
            string codeDesc = Description.GetTranCodeTypeAcrExtFeatureStls(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"n_ext_feature_type: {message.tran.extfeat_stls.nExtFeatureType}");
            Console.WriteLine($"nHardwareType: {message.tran.extfeat_stls.nHardwareType}");
            Console.WriteLine($"nExtFeatureData: {UtilityHelper.ByteToHexStr(message.tran.extfeat_stls.nExtFeatureData)}");
            Console.WriteLine($"nExtFeatureStatus: {UtilityHelper.ByteToHexStr(message.tran.extfeat_stls.nExtFeatureStatus)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeAcrExtFeatureCoS(SCPReplyMessage message) 
        {
            string codeDesc = Description.GetTranCodeTypeAcrExtFeatureCoS(message.tran.tran_code);
            BaseTransactionProcess(message, codeDesc);
            
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"n_ext_feature_type: {message.tran.extfeat_cos.nExtFeatureType}");
            Console.WriteLine($"nHardwareType: {message.tran.extfeat_cos.nHardwareType}");
            Console.WriteLine($"nExtFeaturePoint: {message.tran.extfeat_cos.nExtFeaturePoint}");
            Console.WriteLine($"nStatus: {message.tran.extfeat_cos.nStatus}");
            Console.WriteLine($"nStatus_desc: {Description.DecodeStatusTypeCoS(message.tran.extfeat_cos.nStatus,message.tran.source_type)}");
            Console.WriteLine($"nStatusPrior: {message.tran.extfeat_cos.nStatusPrior}");
            Console.WriteLine($"nStatusPrior_desc: {Description.DecodeStatusTypeCoS(message.tran.extfeat_cos.nStatusPrior,message.tran.source_type)}");
            Console.WriteLine($"nExtFeatureData: {UtilityHelper.ByteToHexStr(message.tran.extfeat_stls.nExtFeatureData)}");
            Console.WriteLine($"nExtFeatureStatus: {UtilityHelper.ByteToHexStr(message.tran.extfeat_stls.nExtFeatureStatus)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

        public static void tranTypeAsci(SCPReplyMessage message)
        {
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"bfr: {UtilityHelper.CharToString(message.tran.t_diag.bfr)}");
            Console.WriteLine("###### transaction extend_desc End ######");
            
        }

        public static void tranTypeSioDiag(SCPReplyMessage message)
        {
            Console.WriteLine("###### transaction extend_desc ######");
            Console.WriteLine($"length: {message.tran.s_diag.length}");
            Console.WriteLine($"bfr: {UtilityHelper.ByteToHexStr(message.tran.s_diag.bfr)}");
            Console.WriteLine("###### transaction extend_desc End ######");
        }

    }
}
