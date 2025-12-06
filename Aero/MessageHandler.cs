using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using HIDAeroService.Utility;
using static HIDAeroService.AeroLibrary.Description;

namespace HIDAeroService.AeroLibrary
{
    public sealed class MessageHandler(ITransactionService transactionService ,IModuleService moduleService,IControlPointService cpService, IMonitorPointService mpService, IDoorService doorService, ICredentialService credentialService, CmndService cmndService, IHelperService<CommandStatus> helperService, IAccessLevelService accessLevelService, SysService sysService)
    {

        public void SCPReplyNAKHandler(SCPReplyMessage message)
        {
            switch (message.nak.reason)
            {
                case 0:
                    Console.WriteLine($"reason: Invalid Packet Header, data: {message.nak.data}, command: {UtilityHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 1:
                case 2: 
                case 3:
                    Console.WriteLine($"reason: Invalid command type (firmware revision mismatch), data: {message.nak.data}, command: {UtilityHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 4:
                    Console.WriteLine($"reason: Command content error, data: {message.nak.data}, command: {UtilityHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 5:
                    Console.WriteLine($"reason: Cannot execute - requires password logon, data: {message.nak.data}, command: {UtilityHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 6:
                    Console.WriteLine($"reason: This port is in standby mode and cannot execute this command, data: {message.nak.data}, command: {UtilityHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 7:
                    Console.WriteLine($"reason: Failed logon - password and/or encryption key, data: {message.nak.data}, command: {UtilityHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 8:
                    Console.WriteLine($"reason: Command not accepted, controller is running in degraded mode and only a limited number of commands are accepted, data: {message.nak.data}, command: {UtilityHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
            }

        }

        public void SCPReplyTransactionHandler(SCPReplyMessage message,bool isWaitingCardScan,short ScanScpId,short ScanAcrNo)
        {

            switch (message.tran.tran_type)
            {      
                case (short)tranType.tranTypeSys:
                    ProcessTransaction.ProcessTypeSys(message);
                    break;
                case (short)tranType.tranTypeSioComm:
                    moduleService.GetSioStatus(message.SCPId, message.tran.source_number);
                    ProcessTransaction.ProcessTypeSioComm(message);
                    break;
                case (short)tranType.tranTypeCardBin:
                    ProcessTransaction.ProcessTypeCardBin(message);
                    break;
                case (short)tranType.tranTypeCardBcd:
                    ProcessTransaction.ProcessTypeCardBcd(message);
                    break;
                case (short)tranType.tranTypeCardFull:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        credentialService.TriggerCardScan(message.SCPId,message.tran.c_full.format_number,message.tran.c_full.facility_code,message.tran.c_full.cardholder_id,message.tran.c_full.issue_code,message.tran.c_full.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypeCardFull(message);
                    break;
                case (short)tranType.tranTypeDblCardFull:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        credentialService.TriggerCardScan(message.SCPId, message.tran.c_fulldbl.format_number, message.tran.c_fulldbl.facility_code, message.tran.c_fulldbl.cardholder_id, message.tran.c_fulldbl.issue_code, message.tran.c_fulldbl.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypeDblCardFull(message);
                    break;
                case (short)tranType.tranTypeI64CardFull:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        credentialService.TriggerCardScan(message.SCPId, message.tran.c_fulli64.format_number, message.tran.c_fulli64.facility_code, message.tran.c_fulli64.cardholder_id, message.tran.c_fulli64.issue_code, message.tran.c_fulli64.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypei64CardFull(message);
                    break;
                case (short)tranType.tranTypeI64CardFullIc32:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        credentialService.TriggerCardScan(message.SCPId, message.tran.c_fulli64i32.format_number, message.tran.c_fulli64i32.facility_code, message.tran.c_fulli64i32.cardholder_id, message.tran.c_fulli64i32.issue_code, message.tran.c_fulli64i32.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypei64CardFullc32(message);
                    break;
                case (short)tranType.tranTypeCardID:
                    ProcessTransaction.ProcessTypeCardID(message);
                    break;
                case (short)tranType.tranTypeDblCardID:
                    ProcessTransaction.ProcessTypeDblCardID(message);
                    break;
                case (short)tranType.tranTypeI64CardID:
                    ProcessTransaction.tranTypei64CardID(message);
                    break;
                case (short)tranType.tranTypeCoS:
                    switch (message.tran.source_type)
                    {
                        case (short)tranSrc.tranSrcSioCom:
                            moduleService.TriggerDeviceStatus(message.SCPId,message.tran.source_number,DecodeHelper.TypeSioCommStatusDecode(message.tran.cos.status),null,null,null);
                            break;
                        case (short)tranSrc.tranSrcMP:
                            mpService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, DecodeHelper.TypeCosStatusDecode(message.tran.cos.status));
                            break;
                        case (short)tranSrc.tranSrcCP:
                            cpService.TriggerDeviceStatus(helperService.GetMacFromId((short)message.SCPId), message.tran.source_number, DecodeHelper.TypeCosStatusDecode(message.tran.cos.status));
                            break;
                        default:
                            break;
                    }
                    ProcessTransaction.tranTypeCos(message);
                    break;
                case (short)tranType.tranTypeREX:
                    ProcessTransaction.tranTypeRex(message);
                    break;
                case (short)tranType.tranTypeCoSDoor:
                    doorService.TriggerDeviceStatusAsync(message.SCPId,message.tran.source_number,"",Description.GetAccessPointStatusFlagResult(message.tran.door.ap_status));
                    ProcessTransaction.tranTypeCosDoor(message);
                    break;
                case (short)tranType.tranTypeProcedure:
                    ProcessTransaction.tranTypeProcedure(message);
                    break;
                case (short)tranType.tranTypeUserCmnd:
                    ProcessTransaction.tranTypeUserCmnd(message);
                    break;
                case (short)tranType.tranTypeActivate:
                    ProcessTransaction.tranTypeActivate(message);
                    break;
                case (short)tranType.tranTypeAcr:
                    doorService.TriggerDeviceStatusAsync(message.SCPId, message.tran.source_number, Description.GetACRModeForStatus(message.tran.tran_code), "");
                    ProcessTransaction.tranTypeAcr(message);
                    break;
                case (short)tranType.tranTypeMpg:
                    ProcessTransaction.tranTypeMpg(message);
                    break;
                case (short)tranType.tranTypeArea:
                    ProcessTransaction.tranTypeArea(message);
                    break;
                case (short)tranType.tranTypeUseLimit:
                    ProcessTransaction.tranTypeUseLimit(message);
                    break;
                case (short)tranType.tranTypeWebActivity:
                    ProcessTransaction.tranTypeWebActivity(message);
                    break;
                case (short)tranType.tranTypeOperatingMode:
                    ProcessTransaction.tranTypeOperatingMode(message);
                    break;
                case (short)tranType.tranTypeCoSElevator:
                    ProcessTransaction.tranTypeCoSElevator(message);
                    break;
                case (short)tranType.tranTypeFileDownloadStatus:
                    ProcessTransaction.tranTypeFileDownloadStatus(message);
                    break;
                case (short)tranType.tranTypeCoSElevatorAccess:
                    ProcessTransaction.tranTypeCoSElevatorAccess(message);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureStls:
                    ProcessTransaction.tranTypeAcrExtFeatureStls(message);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureCoS:
                    ProcessTransaction.tranTypeAcrExtFeatureCoS(message);
                    break;
                case (short)tranType.tranTypeAsci:
                    ProcessTransaction.tranTypeAsci(message);
                    break;
                case (short)tranType.tranTypeSioDiag:
                    ProcessTransaction.tranTypeSioDiag(message);
                    break;
                default:
                    break;
            }

        }

        public void SCPReplyCommStatus(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyCommStatus ExtendDesc ######");
            Console.WriteLine($"status: {message.comm.status}");
            Console.WriteLine($"status_desc: {Description.GetReplyStatusDesc(message.comm.status)}");
            Console.WriteLine($"error_code: {message.comm.error_code}");
            Console.WriteLine($"error_code_desc: {Description.GetErrorCodeDesc((int)message.comm.error_code)}");
            Console.WriteLine($"nChannelId: : {message.comm.nChannelId}");
            Console.WriteLine($"current_primary_comm: : {message.comm.current_primary_comm}");
            Console.WriteLine($"current_primary_comm_desc: : {Description.GetCommStatusDesc(message.comm.current_primary_comm)}");
            Console.WriteLine($"previous_primary_comm: : {message.comm.previous_primary_comm}");
            Console.WriteLine($"previous_primary_comm_desc: : {Description.GetCommStatusDesc(message.comm.previous_primary_comm)}");
            Console.WriteLine($"current_alternate_comm: : {message.comm.current_alternate_comm}");
            Console.WriteLine($"current_alternate_comm_desc: : {Description.GetCommStatusDesc(message.comm.current_alternate_comm)}");
            Console.WriteLine($"previous_alternate_comm: : {message.comm.previous_alternate_comm}");
            Console.WriteLine($"previous_alternate_comm_desc: : {Description.GetCommStatusDesc(message.comm.previous_alternate_comm)}");
            Console.WriteLine("###### SCPReplyCommStatus ExtendDesc End ######");
        }

        public void SCPReplyIDReport(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyIDReport ExtendDesc ######");
            Console.WriteLine($"device_id: {message.id.device_id}");
            Console.WriteLine($"device_ver: {message.id.device_ver}");
            Console.WriteLine($"sft_rev_major: {message.id.sft_rev_major}");
            Console.WriteLine($"sft_rev_minor: {message.id.sft_rev_minor}");
            Console.WriteLine($"serial_number: {message.id.serial_number}");
            Console.WriteLine($"ram_size: {message.id.ram_size}");
            Console.WriteLine($"ram_free: {message.id.ram_free}");
            Console.WriteLine($"e_sec: {message.id.e_sec}");
            Console.WriteLine($"e_sec_datetime: {UtilityHelper.UnixToDateTime(message.id.e_sec)}");
            Console.WriteLine($"db_max: {message.id.db_max}");
            Console.WriteLine($"db_active: {message.id.db_active}");
            Console.WriteLine($"dip_switch_pwrup: {message.id.dip_switch_pwrup}");
            Console.WriteLine($"dip_switch_current: {message.id.dip_switch_current}");
            Console.WriteLine($"scp_id: {message.id.scp_id}");
            Console.WriteLine($"firmware_advisory: {message.id.firmware_advisory}");
            Console.WriteLine($"scp_in_1: {message.id.scp_in_1}");
            Console.WriteLine($"scp_in_2: {message.id.scp_in_2}");
            Console.WriteLine($"nOemCode: {message.id.nOemCode}");
            Console.WriteLine($"config_flags: {message.id.config_flags}");
            string bits = Convert.ToString(message.id.config_flags, 2).PadLeft(8, '0');
            Console.WriteLine($"config_flags: {bits}");
            Console.WriteLine($"mac_addr: {UtilityHelper.ByteToHexStr(message.id.mac_addr)}");
            Console.WriteLine($"tls_status: {message.id.tls_status}");
            Console.WriteLine($"oper_mode: {message.id.oper_mode}");
            Console.WriteLine($"scp_in_3: {message.id.scp_in_3}");
            Console.WriteLine($"cumulative_bld_cnt: {message.id.cumulative_bld_cnt}");
            
            Console.WriteLine("###### SCPReplyIDReport ExtendDesc End ######");
        }

        public void SCPReplyTranStatus(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyTranStatus ExtendDesc ######");
            Console.WriteLine($"capacity: {message.tran_sts.capacity}");
            Console.WriteLine($"oldest: {message.tran_sts.oldest}");
            Console.WriteLine($"last_rprtd: {message.tran_sts.last_rprtd}");
            Console.WriteLine($"last_loggd: {message.tran_sts.last_loggd}");
            Console.WriteLine($"disabled: {message.tran_sts.disabled}");
            Console.WriteLine($"disabled_desc: {Description.GetStatusTranReportDesc(message.tran_sts.disabled)}");      
            Console.WriteLine("###### SCPReplyTranStatus ExtendDesc End ######");
        }

        public void SCPReplySrMsp1Drvr(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrMsp1Drvr ExtendDesc ######");
            Console.WriteLine($"number: {message.sts_drvr.number}");
            Console.WriteLine($"port: {message.sts_drvr.port}");
            Console.WriteLine($"mode: {message.sts_drvr.mode}");
            Console.WriteLine($"baud_rate: {message.sts_drvr.baud_rate}");
            Console.WriteLine($"disabled: {message.sts_drvr.throughput}");
            Console.WriteLine("###### SCPReplySrMsp1Drvr ExtendDesc End ######");
        }

        public void SCPReplySrSio(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrSio ExtendDesc ######");
            Console.WriteLine($"number: {message.sts_sio.number}");
            Console.WriteLine($"com_status: {message.sts_sio.com_status}");
            Console.WriteLine($"com_status_desc: {Description.GetTranCodeTypeSioCommDesc(message.sts_sio.com_status)}");
            Console.WriteLine($"msp1_dnum: {message.sts_sio.msp1_dnum}");
            Console.WriteLine($"com_retries: {message.sts_sio.com_retries}");
            Console.WriteLine($"ct_stat: {message.sts_sio.ct_stat}");
            Console.WriteLine($"ct_stat_desc: {Description.GetTranCodeTypeCosDesc(message.sts_sio.ct_stat)}");
            Console.WriteLine($"pw_stat: {message.sts_sio.pw_stat}");
            Console.WriteLine($"pw_stat_desc: {Description.GetTranCodeTypeCosDesc(message.sts_sio.pw_stat)}");
            Console.WriteLine($"model: {message.sts_sio.model}");
            Console.WriteLine($"model_desc: {Description.GetSioModelDesc(message.sts_sio.model)}");
            Console.WriteLine($"revision: {message.sts_sio.revision}");
            Console.WriteLine($"serial_number: {message.sts_sio.serial_number}");
            Console.WriteLine("""
                     Serial number: A value of 2 indicates that the device was writing to EEPROM.
                Continuously returning a value of 2 indicates that there may be an abnormal hardware condition. A
                value of 4 indicates that the loader code is corrupted and the unit must be returned to factory for
                repair. No firmware can be downloaded when in this state and the loader revision reported is not
                vali
                """);
            Console.WriteLine($"inputs: {message.sts_sio.inputs}");
            Console.WriteLine($"outputs: {message.sts_sio.outputs}");
            Console.WriteLine($"readers: {message.sts_sio.readers}");
            Console.WriteLine($"ip_stat: ");
            int i = 0;
            foreach (short s in message.sts_sio.ip_stat) 
            {
                if(message.tran == null)
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                
                i++;
            }
            i = 0;
            Console.WriteLine($"op_stat: ");
            foreach (short s in message.sts_sio.op_stat)
            {
                if (message.tran == null)
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                i++;
            }
            i = 0;
            Console.WriteLine($"rdr_stat: ");
            foreach (short s in message.sts_sio.rdr_stat)
            {
                if (message.tran == null)
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                i++;
            }
           
            i = 0;
            Console.WriteLine($"nExtendedInfoValid: {message.sts_sio.nExtendedInfoValid}");
            if(message.sts_sio.nExtendedInfoValid != 0)
            {
                Console.WriteLine($"nHardwareId: {message.sts_sio.nHardwareId}");
                Console.WriteLine($"nHardwareId_desc: {Description.GetNHardwareIdDesc(message.sts_sio.nHardwareId)}");
                Console.WriteLine($"nHardwareRev: {message.sts_sio.nHardwareRev}");
                Console.WriteLine($"nProductId: {message.sts_sio.nProductId}");
                Console.WriteLine($"nProductId_desc: {Description.GetSioModelDesc(message.sts_sio.nProductId)}");
                Console.WriteLine($"nProductVer: {message.sts_sio.nProductVer}");
                Console.WriteLine($"nFirmwareBoot: {message.sts_sio.nFirmwareBoot}");
                Console.WriteLine($"nFirmwareLdr: {message.sts_sio.nFirmwareLdr}");
                Console.WriteLine($"nFirmwareApp: {message.sts_sio.nFirmwareApp}");
                Console.WriteLine($"nOemCode: {message.sts_sio.nOemCode}");
                Console.WriteLine($"nEncConfig: {message.sts_sio.nEncConfig}");
                Console.WriteLine($"nEncConfig_desc: {Description.GetNEncConfig(message.sts_sio.nEncConfig)}");
                Console.WriteLine($"nEncKeyStatus: {message.sts_sio.nEncKeyStatus}");
                Console.WriteLine($"nEncKeyStatus_desc: {Description.GetNEncKeyStatus(message.sts_sio.nEncKeyStatus)}");
                Console.WriteLine($"mac_addr: {UtilityHelper.ByteToHexStr(message.sts_sio.mac_addr)}");
                Console.WriteLine($"emg_stat: {message.sts_sio.emg_stat}");
            }
            Console.WriteLine("###### SCPReplySrSio ExtendDesc End ######");
        }

        public void SCPReplySrMp(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrMp ExtendDesc ######");
            Console.WriteLine($"first: {message.sts_mp.first}");
            Console.WriteLine($"count: {message.sts_mp.count}");
            Console.WriteLine($"status: ");
            int i = 0;
            foreach (short s in message.sts_mp.status)
            {
                if (message.tran == null)
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                i++;
            }
            Console.WriteLine("###### SCPReplySrMp ExtendDesc End ######");
        }

        public void SCPReplySrCp(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrCp ExtendDesc ######");
            Console.WriteLine($"first: {message.sts_cp.first}");
            Console.WriteLine($"count: {message.sts_cp.count}");
            Console.WriteLine($"status: ");
            int i = 0;
            foreach (short s in message.sts_cp.status)
            {
                if(message.tran == null)
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + Description.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                
                i++;
            }
            Console.WriteLine("###### SCPReplySrCp ExtendDesc End ######");
        }

        public void SCPReplySrAcr(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrAcr ExtendDesc ######");
            Console.WriteLine($"number: {message.sts_acr.number}");
            Console.WriteLine($"mode: {message.sts_acr.mode}");
            Console.WriteLine($"mode_desc: {Description.GetAcrAccessModeDesc(message.sts_acr.mode)}");
            Console.WriteLine($"rdr_status: {message.sts_acr.rdr_status}");
            if (message.tran != null)
            {
                Console.WriteLine($"rdr_status_desc: {Description.DecodeStatusTypeCoS(message.sts_acr.rdr_status, message.tran.source_type)}");
            }
                
            Console.WriteLine($"strk_status: {message.sts_acr.strk_status}");
            if (message.tran != null)
            {
                Console.WriteLine($"strk_status_desc: {Description.DecodeStatusTypeCoS(message.sts_acr.strk_status, message.tran.source_type)}");
            }
            
            Console.WriteLine($"door_status: {message.sts_acr.door_status}");
            Console.WriteLine($"door_status_desc: {Description.GetStatusTypeCosDoorDesc((byte)message.sts_acr.door_status)}");
            Console.WriteLine($"ap_status: {message.sts_acr.ap_status}");
            Console.WriteLine($"ap_status_desc: {Description.GetStatusTypeCosDoorDesc((byte)message.sts_acr.ap_status)}");
            Console.WriteLine($"rex_status0: {message.sts_acr.rex_status0}");
            if (message.tran != null)
            {
                Console.WriteLine($"rex_status0_desc: {Description.DecodeStatusTypeCoS(message.sts_acr.rex_status0, message.tran.source_type)}");
            }
            
            Console.WriteLine($"rex_status1: {message.sts_acr.rex_status1}");
            if (message.tran != null)
            {
                Console.WriteLine($"rex_status1_desc: {Description.DecodeStatusTypeCoS(message.sts_acr.rex_status1, message.tran.source_type)}");
            }
            
            Console.WriteLine($"led_mode: {message.sts_acr.led_mode}");
            Console.WriteLine($"actl_flags: {message.sts_acr.actl_flags}");
            Console.WriteLine($"actl_flags_desc: {Description.GetAccessControlFlagDesc(message.sts_acr.actl_flags)}");
            Console.WriteLine($"altrdr_status: {message.sts_acr.altrdr_status}");
            if (message.tran != null)
            {
                Console.WriteLine($"altrdr_status_desc: {Description.DecodeStatusTypeCoS(message.sts_acr.altrdr_status, message.tran.source_type)}");
            }
            
            Console.WriteLine($"actl_flags_extd: {message.sts_acr.actl_flags_extd}");
            Console.WriteLine($"actl_flags_extd_desc: {Description.GetAccessControlFlagDescExtend(message.sts_acr.actl_flags_extd)}");
            Console.WriteLine($"nExtFeatureType: {message.sts_acr.nExtFeatureType}");
            Console.WriteLine($"nExtFeatureType_desc: {Description.GetExtendFeatureType(message.sts_acr.nExtFeatureType)}");
            Console.WriteLine($"nHardwareType: {message.sts_acr.nHardwareType}");
            Console.WriteLine($"nHardwareType_desc: {Description.GetNHardwareTypeDesc(message.sts_acr.nHardwareType)}");
            Console.WriteLine($"nExtFeatureStatus: : {message.sts_acr.nExtFeatureStatus}"); 
            Console.WriteLine($"nAuthModFlags: : {message.sts_acr.nAuthModFlags}");
            Console.WriteLine("###### SCPReplySrAcr ExtendDesc End ######");
        }

        public void SCPReplySrTz(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrTz ExtendDesc ######");
            Console.WriteLine($"first: {message.sts_tz.first}");
            Console.WriteLine($"count: {message.sts_tz.count}");
            Console.WriteLine($"status:");
            int i = 0;
            foreach (short s in message.sts_tz.status) 
            {
                Console.WriteLine(i+" : " + Description.GetTimeZoneStatus(s));
            }
            Console.WriteLine("###### SCPReplySrTz ExtendDesc End ######");
        }

        public void SCPReplySrTv(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrTv ExtendDesc ######");
            Console.WriteLine($"first: {message.sts_tv.first}");
            Console.WriteLine($"count: {message.sts_tv.count}");
            Console.WriteLine($"status:");
            int i = 1;
            foreach (short s in message.sts_tv.status)
            {
                Console.WriteLine(i + " : " + Description.GetTriggerVariableStatus(s));
            }
            Console.WriteLine("###### SCPReplySrTv ExtendDesc End ######");
        }

        public void SCPReplySrMpg(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrMpg ExtendDesc ######");
            Console.WriteLine($"number: {message.sts_mpg.number}");
            Console.WriteLine($"mask_count: {message.sts_mpg.mask_count}");
            Console.WriteLine($"num_active: {message.sts_mpg.num_active}");
            Console.WriteLine($"active_mp_list:");
            int i = 1;
            foreach (short s in message.sts_mpg.active_mp_list)
            {
                Console.WriteLine(i + " : " + s);
            }
            Console.WriteLine("###### SCPReplySrMpg ExtendDesc End ######");
        }

        public void SCPReplySrArea(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrArea ExtendDesc ######");
            Console.WriteLine($"number: {message.sts_area.number}");
            Console.WriteLine($"flags: {message.sts_area.flags}");
            Console.WriteLine($"flags_desc: {Description.GetAreaFlagsDesc(message.sts_area.flags)}");
            Console.WriteLine($"occupancy: {message.sts_area.occupancy}");
            Console.WriteLine("###### SCPReplySrArea ExtendDesc End ######");
        }

        public void SCPReplySioRelayCounts(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySioRelayCounts ExtendDesc ######");
            Console.WriteLine($"sio_number: {message.sio_relay_counts.sio_number}");
            Console.WriteLine($"num_relays: {message.sio_relay_counts.num_relays}");
            int i = 1;
            foreach (int s in message.sio_relay_counts.data)
            {
                Console.WriteLine(i + " : " + s);
            }
            Console.WriteLine($"occupancy: {message.sts_area.occupancy}");
            Console.WriteLine("###### SCPReplySioRelayCounts ExtendDesc End ######");
        }

        public void SCPReplyStrStatus(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyStrStatus ExtendDesc ######");
            Console.WriteLine(message.str_sts.nListLength);
            foreach(var i in message.str_sts.sStrSpec)
            {
                Console.WriteLine("nStrType: " + i.nStrType);
                Console.WriteLine("nRecord: " + i.nRecords);
                Console.WriteLine("nRecSize: " + i.nRecSize);
                Console.WriteLine("nActive: " + i.nActive);
            }
            Console.WriteLine("###### SCPReplyStrStatus ExtendDesc End ######");
        }

        public void SCPReplyCmndStatus(SCPReplyMessage message,AeroCommand write)
        {
            
            Console.WriteLine("###### SCPReplyCmndStatus ExtendDesc ######");
            Console.WriteLine(message.cmnd_sts.status);
            Console.WriteLine(message.cmnd_sts.sequence_number);
            Console.WriteLine(message.cmnd_sts.nak);
            Console.WriteLine(message.cmnd_sts.nak.reason);
            Console.WriteLine(write.TagNo);
            Console.WriteLine(write.Command);
            Console.WriteLine("###### SCPReplyCmndStatus ExtendDesc End ######");


        }

        internal void SCPReplyWebConfigNetwork(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyWebConfigNetwork ExtendDesc ######");
            Console.WriteLine(message.web_network.method);
            Console.WriteLine(UtilityHelper.IntegerToIp(message.web_network.cIpAddr));
            Console.WriteLine(UtilityHelper.IntegerToIp(message.web_network.cSubnetMask));
            Console.WriteLine(UtilityHelper.IntegerToIp(message.web_network.cDfltGateway));
            Console.WriteLine(message.web_network.cHostName);
            Console.WriteLine(message.web_network.dnsType);
            Console.WriteLine(message.web_network.cDns);
            foreach(var s in message.web_network.cDnsSuffix)
            {
                Console.Write(s);
            }
            Console.WriteLine("");
            Console.WriteLine("###### SCPReplyWebConfigNetwork ExtendDesc End ######");
        }
    }
}
