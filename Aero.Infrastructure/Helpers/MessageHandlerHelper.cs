

using System.Threading.Channels;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Adapter;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aero.Infrastructure.Helpers
{
    public sealed class MessageHandlerHelper()
    {

        public static void SCPReplyNakHandler(SCPReplyMessage message,ILogger logger)
        {
            switch (message.nak.reason)
            {
                case 0:
                    logger.LogError($"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Invalid Packet Header, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 1:
                case 2: 
                case 3:
                     logger.LogError($"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Invalid command type (firmware revision mismatch), data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 4:
                    logger.LogError($"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: command content error, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 5:
                    logger.LogError($"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Cannot execute - requires password logon, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 6:
                    logger.LogError($"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: This port is in standby mode and cannot execute this command, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 7:
                    logger.LogError($"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Failed logon - password and/or encryption key, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
                case 8:
                    logger.LogError($"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: command not accepted, controller is running in degraded mode and only a limited number of commands are accepted, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}");
                    break;
            }

        }

        public static void SCPReplyTransactionHandler(SCPReplyMessage message,Channel<IScpReply> queue,ILogger logger,IServiceScopeFactory scopeFactory)
        {

            switch (message.tran.tran_type)
            {      
                case (short)tranType.tranTypeSys:
                    ProcessAeroTransactionHelper.ProcessTypeSys(message);
                    break;
                case (short)tranType.tranTypeSioComm:
                    {
                        using var scope = scopeFactory.CreateScope();
                        var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
                        moduleService.GetSioStatus(message.SCPId, message.tran.source_number);
                        ProcessAeroTransactionHelper.ProcessTypeSioComm(message);
                        break;
                    }
                case (short)tranType.tranTypeCardBin:
                    ProcessAeroTransactionHelper.ProcessTypeCardBin(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeCardBcd:
                    ProcessAeroTransactionHelper.ProcessTypeCardBcd(message);
                    break;
                case (short)tranType.tranTypeCardFull:
                    ProcessAeroTransactionHelper.ProcessTypeCardFull(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeDblCardFull:
                    ProcessAeroTransactionHelper.ProcessTypeDblCardFull(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeI64CardFull:
                    ProcessAeroTransactionHelper.ProcessTypei64CardFull(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeI64CardFullIc32:
                    ProcessAeroTransactionHelper.ProcessTypei64CardFullc32(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeCardID:
                    ProcessAeroTransactionHelper.ProcessTypeCardID(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeDblCardID:
                    ProcessAeroTransactionHelper.ProcessTypeDblCardID(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeI64CardID:
                    ProcessAeroTransactionHelper.tranTypei64CardID(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeCoS:
                    switch (message.tran.source_type)
                    {
                        case (short)tranSrc.tranSrcSioCom:
                            queue.Writer.TryWrite(new ScpReplyAdapter(message));
                            break;
                        case (short)tranSrc.tranSrcMP:
                            queue.Writer.TryWrite(new ScpReplyAdapter(message));
                            break;
                        case (short)tranSrc.tranSrcCP:
                            queue.Writer.TryWrite(new ScpReplyAdapter(message));
                            break;
                        default:
                            break;
                    }
                    ProcessAeroTransactionHelper.tranTypeCos(message);
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    break;
                case (short)tranType.tranTypeREX:
                    ProcessAeroTransactionHelper.tranTypeRex(message);
                    break;
                case (short)tranType.tranTypeCoSDoor:
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    ProcessAeroTransactionHelper.tranTypeCosDoor(message);
                    break;
                case (short)tranType.tranTypeProcedure:
                    ProcessAeroTransactionHelper.tranTypeProcedure(message);
                    break;
                case (short)tranType.tranTypeUserCmnd:
                    ProcessAeroTransactionHelper.tranTypeUserCmnd(message);
                    break;
                case (short)tranType.tranTypeActivate:
                    ProcessAeroTransactionHelper.tranTypeActivate(message);
                    break;
                case (short)tranType.tranTypeAcr:
                    queue.Writer.TryWrite(new ScpReplyAdapter(message));
                    ProcessAeroTransactionHelper.tranTypeAcr(message);
                    break;
                case (short)tranType.tranTypeMpg:
                    ProcessAeroTransactionHelper.tranTypeMpg(message);
                    break;
                case (short)tranType.tranTypeArea:
                    ProcessAeroTransactionHelper.tranTypeArea(message);
                    break;
                case (short)tranType.tranTypeUseLimit:
                    ProcessAeroTransactionHelper.tranTypeUseLimit(message);
                    break;
                case (short)tranType.tranTypeWebActivity:
                    ProcessAeroTransactionHelper.tranTypeWebActivity(message);
                    break;
                case (short)tranType.tranTypeOperatingMode:
                    ProcessAeroTransactionHelper.tranTypeOperatingMode(message);
                    break;
                case (short)tranType.tranTypeCoSElevator:
                    ProcessAeroTransactionHelper.tranTypeCoSElevator(message);
                    break;
                case (short)tranType.tranTypeFileDownloadStatus:
                    ProcessAeroTransactionHelper.tranTypeFileDownloadStatus(message);
                    break;
                case (short)tranType.tranTypeCoSElevatorAccess:
                    ProcessAeroTransactionHelper.tranTypeCoSElevatorAccess(message);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureStls:
                    ProcessAeroTransactionHelper.tranTypeAcrExtFeatureStls(message);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureCoS:
                    ProcessAeroTransactionHelper.tranTypeAcrExtFeatureCoS(message);
                    break;
                case (short)tranType.tranTypeAsci:
                    ProcessAeroTransactionHelper.tranTypeAsci(message);
                    break;
                case (short)tranType.tranTypeSioDiag:
                    ProcessAeroTransactionHelper.tranTypeSioDiag(message);
                    break;
                default:
                    break;
            }

        }

        public static void SCPReplyCommStatus(SCPReplyMessage message,ILogger logger)
        {
            logger.LogInformation($"{DateTime.UtcNow.ToLocalTime()} [CommStatus] status: {message.comm.status}, status_desc: {DescriptionHelper.GetReplyStatusDesc(message.comm.status)} error_code: {message.comm.error_code} error_code_desc: {DescriptionHelper.GetErrorCodeDesc((int)message.comm.error_code)} n_channel_id: : {message.comm.nChannelId}");
            Console.WriteLine("###### SCPReplyCommStatus extend_desc ######");
            Console.WriteLine($"status: {message.comm.status}");
            Console.WriteLine($"status_desc: {DescriptionHelper.GetReplyStatusDesc(message.comm.status)}");
            Console.WriteLine($"error_code: {message.comm.error_code}");
            Console.WriteLine($"error_code_desc: {DescriptionHelper.GetErrorCodeDesc((int)message.comm.error_code)}");
            Console.WriteLine($"n_channel_id: : {message.comm.nChannelId}");
            Console.WriteLine($"current_primary_comm: : {message.comm.current_primary_comm}");
            Console.WriteLine($"current_primary_comm_desc: : {DescriptionHelper.GetCommStatusDesc(message.comm.current_primary_comm)}");
            Console.WriteLine($"previous_primary_comm: : {message.comm.previous_primary_comm}");
            Console.WriteLine($"previous_primary_comm_desc: : {DescriptionHelper.GetCommStatusDesc(message.comm.previous_primary_comm)}");
            Console.WriteLine($"current_alternate_comm: : {message.comm.current_alternate_comm}");
            Console.WriteLine($"current_alternate_comm_desc: : {DescriptionHelper.GetCommStatusDesc(message.comm.current_alternate_comm)}");
            Console.WriteLine($"previous_alternate_comm: : {message.comm.previous_alternate_comm}");
            Console.WriteLine($"previous_alternate_comm_desc: : {DescriptionHelper.GetCommStatusDesc(message.comm.previous_alternate_comm)}");
            Console.WriteLine("###### SCPReplyCommStatus extend_desc End ######");
        }

        public static void SCPReplyIDReport(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyIDReport extend_desc ######");
            Console.WriteLine($"device_id: {message.id.device_id}");
            Console.WriteLine($"device_ver: {message.id.device_ver}");
            Console.WriteLine($"sft_rev_major: {message.id.sft_rev_major}");
            Console.WriteLine($"sft_rev_minor: {message.id.sft_rev_minor}");
            Console.WriteLine($"serial_number: {message.id.serial_number}");
            Console.WriteLine($"ram_size: {message.id.ram_size}");
            Console.WriteLine($"ram_free: {message.id.ram_free}");
            Console.WriteLine($"e_sec: {message.id.e_sec}");
            Console.WriteLine($"e_sec_datetime: {UtilitiesHelper.UnixToDateTime(message.id.e_sec)}");
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
            Console.WriteLine($"mac_addr: {UtilitiesHelper.ByteToHexStr(message.id.mac_addr)}");
            Console.WriteLine($"tls_status: {message.id.tls_status}");
            Console.WriteLine($"oper_mode: {message.id.oper_mode}");
            Console.WriteLine($"scp_in_3: {message.id.scp_in_3}");
            Console.WriteLine($"cumulative_bld_cnt: {message.id.cumulative_bld_cnt}");
            
            Console.WriteLine("###### SCPReplyIDReport extend_desc End ######");
        }

        public static void SCPReplyTranStatus(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyTranStatus extend_desc ######");
            Console.WriteLine($"capacity: {message.tran_sts.capacity}");
            Console.WriteLine($"oldest: {message.tran_sts.oldest}");
            Console.WriteLine($"last_rprtd: {message.tran_sts.last_rprtd}");
            Console.WriteLine($"last_loggd: {message.tran_sts.last_loggd}");
            Console.WriteLine($"disabled: {message.tran_sts.disabled}");
            Console.WriteLine($"disabled_desc: {DescriptionHelper.GetStatusTranReportDesc(message.tran_sts.disabled)}");      
            Console.WriteLine("###### SCPReplyTranStatus extend_desc End ######");
        }

        public static void SCPReplySrMsp1Drvr(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrMsp1Drvr extend_desc ######");
            Console.WriteLine($"number: {message.sts_drvr.number}");
            Console.WriteLine($"port: {message.sts_drvr.port}");
            Console.WriteLine($"mode: {message.sts_drvr.mode}");
            Console.WriteLine($"baud_rate: {message.sts_drvr.baud_rate}");
            Console.WriteLine($"disabled: {message.sts_drvr.throughput}");
            Console.WriteLine("###### SCPReplySrMsp1Drvr extend_desc End ######");
        }

        public static void SCPReplySrSio(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrSio extend_desc ######");
            Console.WriteLine($"number: {message.sts_sio.number}");
            Console.WriteLine($"com_status: {message.sts_sio.com_status}");
            Console.WriteLine($"com_status_desc: {DescriptionHelper.GetTranCodeTypeSioCommDesc(message.sts_sio.com_status)}");
            Console.WriteLine($"msp1_dnum: {message.sts_sio.msp1_dnum}");
            Console.WriteLine($"com_retries: {message.sts_sio.com_retries}");
            Console.WriteLine($"ct_stat: {message.sts_sio.ct_stat}");
            Console.WriteLine($"ct_stat_desc: {DescriptionHelper.GetTranCodeTypeCosDesc(message.sts_sio.ct_stat)}");
            Console.WriteLine($"pw_stat: {message.sts_sio.pw_stat}");
            Console.WriteLine($"pw_stat_desc: {DescriptionHelper.GetTranCodeTypeCosDesc(message.sts_sio.pw_stat)}");
            Console.WriteLine($"model: {message.sts_sio.model}");
            Console.WriteLine($"model_desc: {DescriptionHelper.GetSioModelDesc(message.sts_sio.model)}");
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
            Console.WriteLine($"reader: {message.sts_sio.readers}");
            Console.WriteLine($"ip_stat: ");
            int i = 0;
            foreach (short s in message.sts_sio.ip_stat) 
            {
                if(message.tran == null)
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                
                i++;
            }
            i = 0;
            Console.WriteLine($"op_stat: ");
            foreach (short s in message.sts_sio.op_stat)
            {
                if (message.tran == null)
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                i++;
            }
            i = 0;
            Console.WriteLine($"rdr_stat: ");
            foreach (short s in message.sts_sio.rdr_stat)
            {
                if (message.tran == null)
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                i++;
            }
           
            i = 0;
            Console.WriteLine($"nExtendedInfoValid: {message.sts_sio.nExtendedInfoValid}");
            if(message.sts_sio.nExtendedInfoValid != 0)
            {
                Console.WriteLine($"n_hardware_id: {message.sts_sio.nHardwareId}");
                Console.WriteLine($"nHardwareId_desc: {DescriptionHelper.GetNHardwareIdDesc(message.sts_sio.nHardwareId)}");
                Console.WriteLine($"n_hardware_rev: {message.sts_sio.nHardwareRev}");
                Console.WriteLine($"n_product_id: {message.sts_sio.nProductId}");
                Console.WriteLine($"nProductId_desc: {DescriptionHelper.GetSioModelDesc(message.sts_sio.nProductId)}");
                Console.WriteLine($"n_product_ver: {message.sts_sio.nProductVer}");
                Console.WriteLine($"nFirmwareBoot: {message.sts_sio.nFirmwareBoot}");
                Console.WriteLine($"nFirmwareLdr: {message.sts_sio.nFirmwareLdr}");
                Console.WriteLine($"nFirmwareApp: {message.sts_sio.nFirmwareApp}");
                Console.WriteLine($"nOemCode: {message.sts_sio.nOemCode}");
                Console.WriteLine($"n_enc_config: {message.sts_sio.nEncConfig}");
                Console.WriteLine($"nEncConfig_desc: {DescriptionHelper.GetNEncConfig(message.sts_sio.nEncConfig)}");
                Console.WriteLine($"n_enc_key_status: {message.sts_sio.nEncKeyStatus}");
                Console.WriteLine($"nEncKeyStatus_desc: {DescriptionHelper.GetNEncKeyStatus(message.sts_sio.nEncKeyStatus)}");
                Console.WriteLine($"mac_addr: {UtilitiesHelper.ByteToHexStr(message.sts_sio.mac_addr)}");
                Console.WriteLine($"emg_stat: {message.sts_sio.emg_stat}");
            }
            Console.WriteLine("###### SCPReplySrSio extend_desc End ######");
        }

        public static void SCPReplySrMp(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrMp extend_desc ######");
            Console.WriteLine($"first: {message.sts_mp.first}");
            Console.WriteLine($"count: {message.sts_mp.count}");
            Console.WriteLine($"status: ");
            int i = 0;
            foreach (short s in message.sts_mp.status)
            {
                if (message.tran == null)
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                i++;
            }
            Console.WriteLine("###### SCPReplySrMp extend_desc End ######");
        }

        public static void SCPReplySrCp(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrCp extend_desc ######");
            Console.WriteLine($"first: {message.sts_cp.first}");
            Console.WriteLine($"count: {message.sts_cp.count}");
            Console.WriteLine($"status: ");
            int i = 0;
            foreach (short s in message.sts_cp.status)
            {
                if(message.tran == null)
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s));
                }
                else
                {
                    Console.WriteLine(i + ": " + DescriptionHelper.DecodeStatusTypeCoS(s, message.tran.source_type));
                }
                
                i++;
            }
            Console.WriteLine("###### SCPReplySrCp extend_desc End ######");
        }

        public static void SCPReplySrAcr(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplySrAcr extend_desc ######");
            Console.WriteLine($"number: {message.sts_acr.number}");
            Console.WriteLine($"mode: {message.sts_acr.mode}");
            Console.WriteLine($"mode_desc: {DescriptionHelper.GetAcrAccessModeDesc(message.sts_acr.mode)}");
            Console.WriteLine($"rdr_status: {message.sts_acr.rdr_status}");
            if (message.tran != null)
            {
                Console.WriteLine($"rdr_status_desc: {DescriptionHelper.DecodeStatusTypeCoS(message.sts_acr.rdr_status, message.tran.source_type)}");
            }
                
            Console.WriteLine($"strk_status: {message.sts_acr.strk_status}");
            if (message.tran != null)
            {
                Console.WriteLine($"strk_status_desc: {DescriptionHelper.DecodeStatusTypeCoS(message.sts_acr.strk_status, message.tran.source_type)}");
            }
            
            Console.WriteLine($"door_status: {message.sts_acr.door_status}");
            Console.WriteLine($"door_status_desc: {DescriptionHelper.GetStatusTypeCosDoorDesc((byte)message.sts_acr.door_status)}");
            Console.WriteLine($"ap_status: {message.sts_acr.ap_status}");
            Console.WriteLine($"ap_status_desc: {DescriptionHelper.GetStatusTypeCosDoorDesc((byte)message.sts_acr.ap_status)}");
            Console.WriteLine($"rex_status0: {message.sts_acr.rex_status0}");
            if (message.tran != null)
            {
                Console.WriteLine($"rex_status0_desc: {DescriptionHelper.DecodeStatusTypeCoS(message.sts_acr.rex_status0, message.tran.source_type)}");
            }
            
            Console.WriteLine($"rex_status1: {message.sts_acr.rex_status1}");
            if (message.tran != null)
            {
                Console.WriteLine($"rex_status1_desc: {DescriptionHelper.DecodeStatusTypeCoS(message.sts_acr.rex_status1, message.tran.source_type)}");
            }
            
            Console.WriteLine($"led_mode: {message.sts_acr.led_mode}");
            Console.WriteLine($"actl_flags: {message.sts_acr.actl_flags}");
            Console.WriteLine($"actl_flags_desc: {DescriptionHelper.GetAccessControlFlagDesc(message.sts_acr.actl_flags)}");
            Console.WriteLine($"altrdr_status: {message.sts_acr.altrdr_status}");
            if (message.tran != null)
            {
                Console.WriteLine($"altrdr_status_desc: {DescriptionHelper.DecodeStatusTypeCoS(message.sts_acr.altrdr_status, message.tran.source_type)}");
            }
            
            Console.WriteLine($"actl_flags_extd: {message.sts_acr.actl_flags_extd}");
            Console.WriteLine($"actl_flags_extd_desc: {DescriptionHelper.GetAccessControlFlagDescExtend(message.sts_acr.actl_flags_extd)}");
            Console.WriteLine($"n_ext_feature_type: {message.sts_acr.nExtFeatureType}");
            Console.WriteLine($"nExtFeatureType_desc: {DescriptionHelper.GetExtendFeatureType(message.sts_acr.nExtFeatureType)}");
            Console.WriteLine($"nHardwareType: {message.sts_acr.nHardwareType}");
            Console.WriteLine($"nHardwareType_desc: {DescriptionHelper.GetNHardwareTypeDesc(message.sts_acr.nHardwareType)}");
            Console.WriteLine($"nExtFeatureStatus: : {message.sts_acr.nExtFeatureStatus}"); 
            Console.WriteLine($"nAuthModFlags: : {message.sts_acr.nAuthModFlags}");
            Console.WriteLine("###### SCPReplySrAcr extend_desc End ######");
        }

        public static void SCPReplySrTz(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrTz extend_desc ######");
            Console.WriteLine($"first: {message.sts_tz.first}");
            Console.WriteLine($"count: {message.sts_tz.count}");
            Console.WriteLine($"status:");
            int i = 0;
            foreach (short s in message.sts_tz.status) 
            {
                Console.WriteLine(i+" : " + DescriptionHelper.GetTimeZoneStatus(s));
            }
            Console.WriteLine("###### SCPReplySrTz extend_desc End ######");
        }

        public static void SCPReplySrTv(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrTv extend_desc ######");
            Console.WriteLine($"first: {message.sts_tv.first}");
            Console.WriteLine($"count: {message.sts_tv.count}");
            Console.WriteLine($"status:");
            int i = 1;
            foreach (short s in message.sts_tv.status)
            {
                Console.WriteLine(i + " : " + DescriptionHelper.GetTriggerVariableStatus(s));
            }
            Console.WriteLine("###### SCPReplySrTv extend_desc End ######");
        }

        public static void SCPReplySrMpg(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrMpg extend_desc ######");
            Console.WriteLine($"number: {message.sts_mpg.number}");
            Console.WriteLine($"mask_count: {message.sts_mpg.mask_count}");
            Console.WriteLine($"num_active: {message.sts_mpg.num_active}");
            Console.WriteLine($"active_mp_list:");
            int i = 1;
            foreach (short s in message.sts_mpg.active_mp_list)
            {
                Console.WriteLine(i + " : " + s);
            }
            Console.WriteLine("###### SCPReplySrMpg extend_desc End ######");
        }

        public static void SCPReplySrArea(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySrArea extend_desc ######");
            Console.WriteLine($"number: {message.sts_area.number}");
            Console.WriteLine($"flags: {message.sts_area.flags}");
            Console.WriteLine($"flags_desc: {DescriptionHelper.GetAreaFlagsDesc(message.sts_area.flags)}");
            Console.WriteLine($"occupancy: {message.sts_area.occupancy}");
            Console.WriteLine("###### SCPReplySrArea extend_desc End ######");
        }

        public static void SCPReplySioRelayCounts(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplySioRelayCounts extend_desc ######");
            Console.WriteLine($"sio_number: {message.sio_relay_counts.sio_number}");
            Console.WriteLine($"num_relays: {message.sio_relay_counts.num_relays}");
            int i = 1;
            foreach (int s in message.sio_relay_counts.data)
            {
                Console.WriteLine(i + " : " + s);
            }
            Console.WriteLine($"occupancy: {message.sts_area.occupancy}");
            Console.WriteLine("###### SCPReplySioRelayCounts extend_desc End ######");
        }

        public static void SCPReplyStrStatus(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyStrStatus extend_desc ######");
            Console.WriteLine(message.str_sts.nListLength);
            foreach(var i in message.str_sts.sStrSpec)
            {
                Console.WriteLine("nStrType: " + i.nStrType);
                Console.WriteLine("nRecord: " + i.nRecords);
                Console.WriteLine("nRecSize: " + i.nRecSize);
                Console.WriteLine("nActive: " + i.nActive);
            }
            Console.WriteLine("###### SCPReplyStrStatus extend_desc End ######");
        }

        public static void SCPReplyCmndStatus(SCPReplyMessage message)
        {
            
            Console.WriteLine("###### SCPReplyCmndStatus extend_desc ######");
            Console.WriteLine(message.cmnd_sts.status);
            Console.WriteLine(message.cmnd_sts.sequence_number);
            Console.WriteLine(message.cmnd_sts.nak);
            Console.WriteLine(message.cmnd_sts.nak.reason);
            Console.WriteLine("###### SCPReplyCmndStatus extend_desc End ######");


        }

        public static void SCPReplyWebConfigNetwork(SCPReplyMessage message)
        {
            Console.WriteLine("###### SCPReplyWebConfigNetwork extend_desc ######");
            Console.WriteLine(message.web_network.method);
            Console.WriteLine(UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));
            Console.WriteLine(UtilitiesHelper.IntegerToIp(message.web_network.cSubnetMask));
            Console.WriteLine(UtilitiesHelper.IntegerToIp(message.web_network.cDfltGateway));
            Console.WriteLine(message.web_network.cHostName);
            Console.WriteLine(message.web_network.dnsType);
            Console.WriteLine(message.web_network.cDns);
            foreach(var s in message.web_network.cDnsSuffix)
            {
                Console.Write(s);
            }
            Console.WriteLine("");
            Console.WriteLine("###### SCPReplyWebConfigNetwork extend_desc End ######");
        }

        
    }
}
