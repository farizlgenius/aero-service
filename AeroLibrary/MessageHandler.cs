using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Dto.Scp;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using static HIDAeroService.AeroLibrary.Description;

namespace HIDAeroService.AeroLibrary
{
    public sealed class MessageHandler
    {
        private readonly EventService _eventService;
        private readonly ScpService _scpService;
        private readonly SioService _sioService;
        private readonly CpService _cpService;
        private readonly MpService _mpService;
        private readonly AcrService _acrService;
        private readonly CredentialService _credentialService;
        private readonly CmndService _cmndService;
        private readonly HelperService _helperService;
        private readonly AlvlService _alvlService;
        private readonly TZService _tzService;
        public MessageHandler(EventService eventService,ScpService scpService,SioService sioService,CpService cpService,MpService mpService,AcrService acrService,CredentialService credentialService,CmndService cmndService,HelperService helperService,AlvlService alvlService,TZService tzService) 
        {
            _tzService = tzService;
            _helperService = helperService;
            _cmndService = cmndService;
            _credentialService = credentialService;
            _acrService = acrService;
            _mpService = mpService;
            _cpService = cpService;
            _eventService = eventService;
            _scpService = scpService;
            _sioService = sioService;
            _alvlService = alvlService;
        }

        public void SCPReplyNAKHandler(SCPReplyMessage message)
        {
            switch (message.nak.reason)
            {
                case 0:
                    Console.WriteLine($"reason: Invalid Packet Header, data: {message.nak.data}, command: {Utility.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 1:
                case 2: 
                case 3:
                    Console.WriteLine($"reason: Invalid command type (firmware revision mismatch), data: {message.nak.data}, command: {Utility.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 4:
                    Console.WriteLine($"reason: Command content error, data: {message.nak.data}, command: {Utility.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 5:
                    Console.WriteLine($"reason: Cannot execute - requires password logon, data: {message.nak.data}, command: {Utility.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 6:
                    Console.WriteLine($"reason: This port is in standby mode and cannot execute this command, data: {message.nak.data}, command: {Utility.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 7:
                    Console.WriteLine($"reason: Failed logon - password and/or encryption key, data: {message.nak.data}, command: {Utility.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 8:
                    Console.WriteLine($"reason: Command not accepted, controller is running in degraded mode and only a limited number of commands are accepted, data: {message.nak.data}, command: {Utility.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
            }

        }

        public void SCPReplyTransactionHandler(SCPReplyMessage message,bool isWaitingCardScan,short ScanScpId,short ScanAcrNo)
        {
            switch (message.tran.tran_type)
            {      
                case (short)tranType.tranTypeSys:
                    ProcessTransaction.ProcessTypeSys(message,_eventService);
                    break;
                case (short)tranType.tranTypeSioComm:
                    _sioService.GetSioStatus(message.SCPId, message.tran.source_number);
                    ProcessTransaction.ProcessTypeSioComm(message, _eventService);
                    break;
                case (short)tranType.tranTypeCardBin:
                    ProcessTransaction.ProcessTypeCardBin(message, _eventService);
                    break;
                case (short)tranType.tranTypeCardBcd:
                    ProcessTransaction.ProcessTypeCardBcd(message, _eventService);
                    break;
                case (short)tranType.tranTypeCardFull:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        _credentialService.TriggerCardScan(message.SCPId,message.tran.c_full.format_number,message.tran.c_full.facility_code,message.tran.c_full.cardholder_id,message.tran.c_full.issue_code,message.tran.c_full.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypeCardFull(message, _eventService);
                    break;
                case (short)tranType.tranTypeDblCardFull:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        _credentialService.TriggerCardScan(message.SCPId, message.tran.c_fulldbl.format_number, message.tran.c_fulldbl.facility_code, message.tran.c_fulldbl.cardholder_id, message.tran.c_fulldbl.issue_code, message.tran.c_fulldbl.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypeDblCardFull(message, _eventService);
                    break;
                case (short)tranType.tranTypeI64CardFull:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        _credentialService.TriggerCardScan(message.SCPId, message.tran.c_fulli64.format_number, message.tran.c_fulli64.facility_code, message.tran.c_fulli64.cardholder_id, message.tran.c_fulli64.issue_code, message.tran.c_fulli64.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypei64CardFull(message, _eventService);
                    break;
                case (short)tranType.tranTypeI64CardFullIc32:
                    if (isWaitingCardScan && ScanScpId == message.SCPId && ScanAcrNo == message.tran.source_number)
                    {
                        _credentialService.TriggerCardScan(message.SCPId, message.tran.c_fulli64i32.format_number, message.tran.c_fulli64i32.facility_code, message.tran.c_fulli64i32.cardholder_id, message.tran.c_fulli64i32.issue_code, message.tran.c_fulli64i32.floor_number);
                        isWaitingCardScan = false;
                        ScanAcrNo = -1;
                        ScanScpId = -1;
                    }
                    ProcessTransaction.ProcessTypei64CardFullc32(message, _eventService);
                    break;
                case (short)tranType.tranTypeCardID:
                    ProcessTransaction.ProcessTypeCardID(message,_eventService);
                    break;
                case (short)tranType.tranTypeDblCardID:
                    ProcessTransaction.ProcessTypeDblCardID(message,_eventService);
                    break;
                case (short)tranType.tranTypeI64CardID:
                    ProcessTransaction.tranTypei64CardID(message,_eventService);
                    break;
                case (short)tranType.tranTypeCoS:
                    switch (message.tran.source_type)
                    {
                        case (short)tranSrc.tranSrcSioCom:
                            _sioService.TriggerDeviceStatus(message.SCPId,message.tran.source_number,message.tran.cos.status,0,0,0);
                            break;
                        case (short)tranSrc.tranSrcMP:
                            _mpService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, 1, [message.tran.cos.status]);
                            break;
                        case (short)tranSrc.tranSrcCP:
                            _cpService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, 1, [message.tran.cos.status]);
                            break;
                        default:
                            break;
                    }
                    ProcessTransaction.tranTypeCos(message, _eventService);
                    break;
                case (short)tranType.tranTypeREX:
                    ProcessTransaction.tranTypeRex(message, _eventService);
                    break;
                case (short)tranType.tranTypeCoSDoor:
                    _acrService.TriggerDeviceStatus(message.SCPId,message.tran.source_number,"",Description.GetAccessPointStatusFlagResult(message.tran.door.ap_status));
                    ProcessTransaction.tranTypeCosDoor(message, _eventService);
                    break;
                case (short)tranType.tranTypeProcedure:
                    ProcessTransaction.tranTypeProcedure(message, _eventService);
                    break;
                case (short)tranType.tranTypeUserCmnd:
                    ProcessTransaction.tranTypeUserCmnd(message,_eventService);
                    break;
                case (short)tranType.tranTypeActivate:
                    ProcessTransaction.tranTypeActivate(message,_eventService);
                    break;
                case (short)tranType.tranTypeAcr:
                    _acrService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, Description.GetACRModeForStatus(message.tran.tran_code), "");
                    ProcessTransaction.tranTypeAcr(message, _eventService);
                    break;
                case (short)tranType.tranTypeMpg:
                    ProcessTransaction.tranTypeMpg(message, _eventService);
                    break;
                case (short)tranType.tranTypeArea:
                    ProcessTransaction.tranTypeArea(message, _eventService);
                    break;
                case (short)tranType.tranTypeUseLimit:
                    ProcessTransaction.tranTypeUseLimit(message,_eventService);
                    break;
                case (short)tranType.tranTypeWebActivity:
                    ProcessTransaction.tranTypeWebActivity(message,_eventService);
                    break;
                case (short)tranType.tranTypeOperatingMode:
                    ProcessTransaction.tranTypeOperatingMode(message,_eventService);
                    break;
                case (short)tranType.tranTypeCoSElevator:
                    ProcessTransaction.tranTypeCoSElevator(message,_eventService);
                    break;
                case (short)tranType.tranTypeFileDownloadStatus:
                    ProcessTransaction.tranTypeFileDownloadStatus(message,_eventService);
                    break;
                case (short)tranType.tranTypeCoSElevatorAccess:
                    ProcessTransaction.tranTypeCoSElevatorAccess(message,_eventService);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureStls:
                    ProcessTransaction.tranTypeAcrExtFeatureStls(message, _eventService);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureCoS:
                    ProcessTransaction.tranTypeAcrExtFeatureCoS(message, _eventService);
                    break;
                case (short)tranType.tranTypeAsci:
                    ProcessTransaction.tranTypeAsci(message,_eventService);
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
            Console.WriteLine("###### SCPReplyCommStatus Detail ######");
            _scpService.TriggerDeviceStatus(message.comm.status,message.SCPId);
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
            Console.WriteLine("###### SCPReplyCommStatus Detail End ######");
        }

        public void SCPReplyIDReport(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyIDReport Detail ######");
            Console.WriteLine($"device_id: {message.id.device_id}");
            Console.WriteLine($"device_ver: {message.id.device_ver}");
            Console.WriteLine($"sft_rev_major: {message.id.sft_rev_major}");
            Console.WriteLine($"sft_rev_minor: {message.id.sft_rev_minor}");
            Console.WriteLine($"serial_number: {message.id.serial_number}");
            Console.WriteLine($"ram_size: {message.id.ram_size}");
            Console.WriteLine($"ram_free: {message.id.ram_free}");
            Console.WriteLine($"e_sec: {message.id.e_sec}");
            Console.WriteLine($"e_sec_datetime: {Utility.UnixToDateTime(message.id.e_sec)}");
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
            Console.WriteLine($"mac_addr: {Utility.ByteToHexStr(message.id.mac_addr)}");
            Console.WriteLine($"tls_status: {message.id.tls_status}");
            Console.WriteLine($"oper_mode: {message.id.oper_mode}");
            Console.WriteLine($"scp_in_3: {message.id.scp_in_3}");
            Console.WriteLine($"cumulative_bld_cnt: {message.id.cumulative_bld_cnt}");
            
            Console.WriteLine("###### SCPReplyIDReport Detail End ######");
        }

        public void SCPReplyTranStatus(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyTranStatus Detail ######");
            Console.WriteLine($"capacity: {message.tran_sts.capacity}");
            Console.WriteLine($"oldest: {message.tran_sts.oldest}");
            Console.WriteLine($"last_rprtd: {message.tran_sts.last_rprtd}");
            Console.WriteLine($"last_loggd: {message.tran_sts.last_loggd}");
            Console.WriteLine($"disabled: {message.tran_sts.disabled}");
            Console.WriteLine($"disabled_desc: {Description.GetStatusTranReportDesc(message.tran_sts.disabled)}");      
            Console.WriteLine("###### SCPReplyTranStatus Detail End ######");
        }

        public void SCPReplySrMsp1Drvr(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrMsp1Drvr Detail ######");
            Console.WriteLine($"number: {message.sts_drvr.number}");
            Console.WriteLine($"port: {message.sts_drvr.port}");
            Console.WriteLine($"mode: {message.sts_drvr.mode}");
            Console.WriteLine($"baud_rate: {message.sts_drvr.baud_rate}");
            Console.WriteLine($"disabled: {message.sts_drvr.throughput}");
            Console.WriteLine("###### SCPReplySrMsp1Drvr Detail End ######");
        }

        public void SCPReplySrSio(SCPReplyMessage message)
        {
            _sioService.TriggerDeviceStatus(message.SCPId, message.sts_sio.number, message.sts_sio.com_status, message.sts_sio.ip_stat[4], message.sts_sio.ip_stat[5], message.sts_sio.ip_stat[6]);
            Console.WriteLine("###### SCPReplySrSio Detail ######");
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
                Console.WriteLine($"mac_addr: {Utility.ByteToHexStr(message.sts_sio.mac_addr)}");
                Console.WriteLine($"emg_stat: {message.sts_sio.emg_stat}");
            }
            Console.WriteLine("###### SCPReplySrSio Detail End ######");
        }

        public void SCPReplySrMp(SCPReplyMessage message)
        {
            _mpService.TriggerDeviceStatus(message.SCPId, message.sts_mp.first, message.sts_mp.count, message.sts_mp.status);
            Console.WriteLine("###### SCPReplySrMp Detail ######");
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
            Console.WriteLine("###### SCPReplySrMp Detail End ######");
        }

        public void SCPReplySrCp(SCPReplyMessage message)
        {
            _cpService.TriggerDeviceStatus(message.SCPId,message.sts_cp.first,message.sts_cp.count,message.sts_cp.status);
            Console.WriteLine("###### SCPReplySrCp Detail ######");
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
            Console.WriteLine("###### SCPReplySrCp Detail End ######");
        }

        public void SCPReplySrAcr(SCPReplyMessage message)
        {
            _acrService.TriggerDeviceStatus(message.SCPId,message.sts_acr.number, Description.GetACRModeForStatus(message.sts_acr.door_status),Description.GetAccessPointStatusFlagResult((byte)message.sts_acr.ap_status));
            Console.WriteLine("###### SCPReplySrAcr Detail ######");
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
            Console.WriteLine("###### SCPReplySrAcr Detail End ######");
        }

        public void SCPReplySrTz(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrTz Detail ######");
            Console.WriteLine($"first: {message.sts_tz.first}");
            Console.WriteLine($"count: {message.sts_tz.count}");
            Console.WriteLine($"status:");
            int i = 0;
            foreach (short s in message.sts_tz.status) 
            {
                Console.WriteLine(i+" : " + Description.GetTimeZoneStatus(s));
            }
            Console.WriteLine("###### SCPReplySrTz Detail End ######");
        }

        public void SCPReplySrTv(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrTv Detail ######");
            Console.WriteLine($"first: {message.sts_tv.first}");
            Console.WriteLine($"count: {message.sts_tv.count}");
            Console.WriteLine($"status:");
            int i = 1;
            foreach (short s in message.sts_tv.status)
            {
                Console.WriteLine(i + " : " + Description.GetTriggerVariableStatus(s));
            }
            Console.WriteLine("###### SCPReplySrTv Detail End ######");
        }

        public void SCPReplySrMpg(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrMpg Detail ######");
            Console.WriteLine($"number: {message.sts_mpg.number}");
            Console.WriteLine($"mask_count: {message.sts_mpg.mask_count}");
            Console.WriteLine($"num_active: {message.sts_mpg.num_active}");
            Console.WriteLine($"active_mp_list:");
            int i = 1;
            foreach (short s in message.sts_mpg.active_mp_list)
            {
                Console.WriteLine(i + " : " + s);
            }
            Console.WriteLine("###### SCPReplySrMpg Detail End ######");
        }

        public void SCPReplySrArea(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrArea Detail ######");
            Console.WriteLine($"number: {message.sts_area.number}");
            Console.WriteLine($"flags: {message.sts_area.flags}");
            Console.WriteLine($"flags_desc: {Description.GetAreaFlagsDesc(message.sts_area.flags)}");
            Console.WriteLine($"occupancy: {message.sts_area.occupancy}");
            Console.WriteLine("###### SCPReplySrArea Detail End ######");
        }

        public void SCPReplySioRelayCounts(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySioRelayCounts Detail ######");
            Console.WriteLine($"sio_number: {message.sio_relay_counts.sio_number}");
            Console.WriteLine($"num_relays: {message.sio_relay_counts.num_relays}");
            int i = 1;
            foreach (int s in message.sio_relay_counts.data)
            {
                Console.WriteLine(i + " : " + s);
            }
            Console.WriteLine($"occupancy: {message.sts_area.occupancy}");
            Console.WriteLine("###### SCPReplySioRelayCounts Detail End ######");
        }

        public void SCPReplyStrStatus(SCPReplyMessage message)
        {
            string ScpIp = _helperService.GetScpIpFromId((short)message.SCPId);
            string ScpMac = _helperService.GetMacFromId((short)message.SCPId);
            VerifyScpConfigDto rec = new VerifyScpConfigDto();
            rec.Ip = ScpIp;
            rec.Mac = ScpMac;
            Console.WriteLine("###### SCPReplyStrStatus Detail ######");
            Console.WriteLine(message.str_sts.nListLength);
            foreach(var i in message.str_sts.sStrSpec)
            {

                switch ((ScpStructure)i.nStrType)
                {
                    case ScpStructure.SCPSID_TRAN:
                        // Handle Transactions
                        break;

                    case ScpStructure.SCPSID_TZ:
                        // Handle Time zones
                        int ntz = _tzService.GetTzRecAlloc();
                        rec.RecAllocTimezone = i.nActive < ntz ? 1 : i.nActive > ntz ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_HOL:
                        // Handle Holidays
                        break;

                    case ScpStructure.SCPSID_MSP1:
                        // Handle Msp1 ports (SIO drivers)
                        break;

                    case ScpStructure.SCPSID_SIO:
                        // Handle SIOs
                        
                        int nsio = _sioService.GetRecAllocSio(ScpIp);
                        rec.RecAllocSio = i.nActive < nsio ? 1 : i.nActive > nsio ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_MP:
                        // Handle Monitor points
                        int nmp = _mpService.GetMpRecAlloc(ScpIp);
                        rec.RecAllocMp = i.nActive < nmp ? 1 : i.nActive > nmp ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_CP:
                        int ncp = _cpService.GetCpRecAlloc(ScpIp);
                        rec.RecAllocCp = i.nActive < ncp ? 1 : i.nActive > ncp ? -1 : 0;
                        // Handle Control points
                        break;

                    case ScpStructure.SCPSID_ACR:
                        // Handle Access control readers
                        int nacr = _acrService.GetAcrRecAlloc(ScpIp);
                        rec.RecAllocAcr = i.nActive < nacr ? 1 : i.nActive > nacr ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_ALVL:
                        // Handle Access levels
                        int nalvl = _alvlService.GetAlvlRecAlloc();
                        rec.RecAllocAlvl = i.nActive < nalvl ? 1 : i.nActive > nalvl ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_TRIG:
                        // Handle Triggers
                        break;

                    case ScpStructure.SCPSID_PROC:
                        // Handle Procedures
                        break;

                    case ScpStructure.SCPSID_MPG:
                        // Handle Monitor point groups
                        break;

                    case ScpStructure.SCPSID_AREA:
                        // Handle Access areas
                        break;

                    case ScpStructure.SCPSID_EAL:
                        // Handle Elevator access levels
                        break;

                    case ScpStructure.SCPSID_CRDB:
                        // Handle Cardholder database
                        int ncard = _credentialService.GetCredentialRecAlloc();
                        rec.RecAllocCrdb = i.nRecords < ncard ? 1 : i.nRecords > ncard ? -1 : 0;
                        int ncardac = _credentialService.GetActiveCredentialRecAlloc();
                        rec.RecAllocCardActive = i.nActive < ncardac ? 1 : i.nActive > ncardac ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_FLASH:
                        // Handle FLASH specs
                        break;

                    case ScpStructure.SCPSID_BSQN:
                        // Handle Build sequence number
                        break;

                    case ScpStructure.SCPSID_SAVE_STAT:
                        // Handle Flash save status
                        break;

                    case ScpStructure.SCPSID_MAB1_FREE:
                        // Handle Memory alloc block 1 free memory
                        break;

                    case ScpStructure.SCPSID_MAB2_FREE:
                        // Handle Memory alloc block 2 free memory
                        break;

                    case ScpStructure.SCPSID_ARQ_BUFFER:
                        // Handle Access request buffers
                        break;

                    case ScpStructure.SCPSID_PART_FREE_CNT:
                        // Handle Partition memory free info
                        break;

                    case ScpStructure.SCPSID_LOGIN_STANDARD:
                        // Handle Web logins - standard
                        break;
                    case ScpStructure.SCPSID_FILE_SYSTEM:
                        break;

                    default:
                        // Handle unknown/unsupported types
                        break;
                }
                Console.WriteLine("nStrType: " + i.nStrType);
                Console.WriteLine("nRecord: " + i.nRecords);
                Console.WriteLine("nRecSize: " + i.nRecSize);
                Console.WriteLine("nActive: " + i.nActive);
            }
            Console.WriteLine("###### SCPReplyStrStatus Detail End ######");
            _cmndService.TriggerVerifyScpConfiguration(rec);
        }

        public void SCPReplyCmndStatus(SCPReplyMessage message,WriteAeroDriver write)
        {
            
            _cmndService.TriggerCommandResponse(message.cmnd_sts.status,message.cmnd_sts.sequence_number,Description.GetNakReasonDescription(message.cmnd_sts.nak.reason), message.cmnd_sts.nak.description_code);
            Console.WriteLine("###### SCPReplyCmndStatus Detail ######");
            Console.WriteLine(message.cmnd_sts.status);
            Console.WriteLine(message.cmnd_sts.sequence_number);
            Console.WriteLine(message.cmnd_sts.nak);
            Console.WriteLine("###############");
            Console.WriteLine(message.cmnd_sts.status);
            Console.WriteLine(message.cmnd_sts.sequence_number);
            Console.WriteLine(message.cmnd_sts.nak.reason);
            Console.WriteLine(write.TagNo);

            if (message.cmnd_sts.status == 2 && message.cmnd_sts.sequence_number == write.TagNo && message.cmnd_sts.nak.reason == 4)
            {
                write.ResetSCP((short)message.SCPId);
            }
            Console.WriteLine("###### SCPReplyCmndStatus Detail End ######");
        }

        internal void SCPReplyWebConfigNetwork(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyWebConfigNetwork Detail ######");
            Console.WriteLine(message.web_network.method);
            Console.WriteLine(Utility.IntegerToIp(message.web_network.cIpAddr));
            Console.WriteLine(Utility.IntegerToIp(message.web_network.cSubnetMask));
            Console.WriteLine(Utility.IntegerToIp(message.web_network.cDfltGateway));
            Console.WriteLine(message.web_network.cHostName);
            Console.WriteLine(message.web_network.dnsType);
            Console.WriteLine(message.web_network.cDns);
            foreach(var s in message.web_network.cDnsSuffix)
            {
                Console.Write(s);
            }
            Console.WriteLine("");
            Console.WriteLine("###### SCPReplyWebConfigNetwork Detail End ######");
        }
    }
}
