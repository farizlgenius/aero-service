using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
using HIDAeroService.DTO.AccessLevel;
using HIDAeroService.DTO.Acr;
using HIDAeroService.DTO.Action;
using HIDAeroService.DTO.CardFormat;
using HIDAeroService.DTO.Interval;
using HIDAeroService.DTO.Procedure;
using HIDAeroService.DTO.Trigger;
using HIDAeroService.Entity;
using HIDAeroService.Model;
using HIDAeroService.Models;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using HIDAeroService.Utility;
using LibNoise.Modifier;
using Microsoft.Extensions.Logging;
using MiNET.Entities.Passive;
using Newtonsoft.Json.Linq;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using System;
using System.Collections.Concurrent;
using static MiNET.Net.McpeUpdateBlock;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HIDAeroService.Aero.CommandService
{
    public class AeroCommandService(ILogger<AeroCommandService> logger)
    {
        //protected readonly ConcurrentDictionary<string, PendingCommand> _pending = new();

        //private int _timeoutSeconds = 10;

        //public Task<bool> TrackCommandAsync(
        //   string tag,
        //   int mac,
        //   string command)
        //{
        //    logger.LogWarning("Track " + tag );
        //    var tcs = new TaskCompletionSource<bool>(
        //        TaskCreationOptions.RunContinuationsAsynchronously);

        //    var cts = new CancellationTokenSource();

        //    var pending = new PendingCommand
        //    {
        //        Tag = tag,
        //        SentAt = DateTime.UtcNow,
        //        Tcs = tcs,
        //        TimeoutCts = cts
        //    };

        //    if (!_pending.TryAdd(tag, pending))
        //        throw new InvalidOperationException($"Duplicate tag {tag}");

        //    _ = Task.Delay(TimeSpan.FromSeconds(_timeoutSeconds), cts.Token)
        //        .ContinueWith(_ =>
        //        {
        //            if (_pending.TryRemove(tag, out var pc))
        //            {
        //                logger.LogWarning($"[{tag}] [{mac}] [{command}] => TIMEOUT");
        //                pc.Tcs.TrySetResult(false);
        //            }
        //        }, TaskScheduler.Default);

        //    return tcs.Task;
        //}

        //public void CompleteCommand(string tag, bool success)
        //{
        //    logger.LogWarning("Complete " + tag);
        //    if (_pending.TryRemove(tag, out var cmd))
        //    {
        //        cmd.TimeoutCts.Cancel(); // stop timeout task
        //        cmd.Tcs.TrySetResult(success);
        //    }
        //    else
        //    {
        //        logger.LogWarning($"Late or unknown command result: tag={tag}");
        //    }
        //}


        #region Configure the driver

        //////
        // Method: SendCommand to Driver
        // Target: For Setting Base Driver Specification
        // port: port number of driver that Controller need to set
        // maxController: maximum controller that can have in system 1-16384
        //////
        public bool SystemLevelSpecification(short nPort, short nScp)
        {
            CC_SYS cc_sys = new CC_SYS();
            cc_sys.nPorts = nPort;
            cc_sys.nScps = nScp;
            cc_sys.nTimezones = 0;
            cc_sys.nHolidays = 0;
            cc_sys.bDirectMode = 1;
            cc_sys.debug_rq = 0;
            for (int i = 0; i < cc_sys.nDebugArg.Length; i++)
            {
                cc_sys.nDebugArg[i] = 0;
            }
            bool flag = SendCommand((short)enCfgCmnd.enCcSystem, cc_sys);
            return flag;
        }


        //////
        // Method: SendCommand to Driver
        // Target: For CreateAsync Channel
        // channelId: Channel component_id to create
        // commuType: Communication type_desc ip-client / ip-server
        // port: port number of driver that Controller need to set
        // controllerReplyTimeout: maximum controller Reply Waiting Before Timeout Default 3000ms
        //////
        public bool CreateChannel(short cPort, short nChannelId, short cType)
        {
            CC_CHANNEL cc_channel = new CC_CHANNEL();
            cc_channel.nChannelId = nChannelId;
            cc_channel.cType = cType;
            cc_channel.cPort = cPort;
            cc_channel.baud_rate = 0;
            cc_channel.timer1 = 3000;
            cc_channel.timer2 = 0;
            for (int i = 0; i < cc_channel.cModemId.Length; i++)
            {
                cc_channel.cModemId[i] = '\0';
            }
            cc_channel.cRTSMode = 0;
            bool flag = SendCommand((short)enCfgCmnd.enCcCreateChannel, cc_channel);
            return flag;
        }


        #endregion

        #region Configuring the intelligent controller: pre-connection

        public bool SCPDeviceSpecification(short ScpId, SystemSetting setting)
        {
            var _commandValue = (short)enCfgCmnd.enCcScpScp;
            CC_SCP_SCP cc_scp_scp = new CC_SCP_SCP();
            cc_scp_scp.lastModified = 0;
            cc_scp_scp.number = ScpId;
            cc_scp_scp.ser_num_low = 0;
            cc_scp_scp.ser_num_high = 0;
            cc_scp_scp.rev_major = 0;
            cc_scp_scp.rev_minor = 0;
            cc_scp_scp.nMsp1Port = setting.m_msp1_port;
            cc_scp_scp.nTransactions = setting.n_transaction;
            cc_scp_scp.nSio = setting.n_sio;
            cc_scp_scp.nMp = setting.n_mp;
            cc_scp_scp.nCp = setting.n_cp;
            cc_scp_scp.nAcr = setting.n_acr;
            cc_scp_scp.nAlvl = setting.n_alvl;
            cc_scp_scp.nTrgr = setting.n_trgr;
            cc_scp_scp.nProc = setting.n_proc;
            cc_scp_scp.gmt_offset = setting.gmt_offset;
            cc_scp_scp.nDstID = 0;
            cc_scp_scp.nTz = setting.n_tz;
            cc_scp_scp.nHol = setting.n_hol;
            cc_scp_scp.nMpg = setting.n_mpg;
            cc_scp_scp.nTranLimit = 60000;
            cc_scp_scp.nAuthModType = 0;
            cc_scp_scp.nOperModes = 0;
            cc_scp_scp.oper_type = 1;
            cc_scp_scp.nLanguages = 0;
            cc_scp_scp.nSrvcType = 0;
            
            bool flag = SendCommand(_commandValue, cc_scp_scp);
            return flag; 
        }


        public bool AccessDatabaseSpecification(short ScpId, SystemSetting setting)
        {
            CC_SCP_ADBS cc_scp_adbs = new CC_SCP_ADBS();
            cc_scp_adbs.lastModified = 0;
            cc_scp_adbs.nScpID = ScpId;
            cc_scp_adbs.nCards = setting.n_card;
            //cc_scp_adbs.nCards = 100;
            cc_scp_adbs.nAlvl = 32;
            // pin Constant = 1
            cc_scp_adbs.nPinDigits = 324;
            cc_scp_adbs.bIssueCode = 2;
            cc_scp_adbs.bApbLocation = 1;
            cc_scp_adbs.bActDate = 2;
            cc_scp_adbs.bDeactDate = 2;
            cc_scp_adbs.bVacationDate = 1;
            cc_scp_adbs.bUpgradeDate = 0;
            cc_scp_adbs.bUserLevel = 0;
            cc_scp_adbs.bUseLimit = 1;
            cc_scp_adbs.bSupportTimedApb = 1;
            cc_scp_adbs.nTz = 64;
            cc_scp_adbs.bAssetGroup = 0;
            cc_scp_adbs.nHostResponseTimeout = 5;
            cc_scp_adbs.nMxmTypeIndex = 0;
            cc_scp_adbs.nAlvlUse4Arq = 0;
            cc_scp_adbs.nFreeformBlockSize = 0;
            cc_scp_adbs.nEscortTimeout = 15;
            cc_scp_adbs.nMultiCardTimeout = 15;
            cc_scp_adbs.nAssetTimeout = 0;
            cc_scp_adbs.bAccExceptionList = 0;
            cc_scp_adbs.adbFlags = 1;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcScpAdbSpec, cc_scp_adbs);
            return flag;
        }

        //public async Task<bool> ElevatorAccessLevelSpecification(short hardware_id, short MaxElvAvl, short MaxFloor)
        //{
        //    CC_ELALVLSPC cc = new CC_ELALVLSPC();
        //    cc.scp_number = hardware_id;
        //    cc.max_elalvl = MaxElvAvl;
        //    cc.max_floors = MaxFloor;
        //    bool flag = SendCommand((Int16)enCfgCmnd.enCcAlvlEx, cc);
        //    if (flag)
        //    {
        //        var tag = SCPDLL.scpGetTagLastPosted(hardware_id);
        //        return await TrackCommandAsync(tag, hardware_id, command.C1105);
        //    }
        //    return flag;
        //}

        #endregion

        #region SCP

        public bool DeleteScp(short ScpId)
        {
            CC_NEWSCP c = new CC_NEWSCP();
            c.nSCPId = ScpId;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcDeleteScp, c); 
            return flag;

        }

        public bool DetachScp(short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcDetachScp;
            CC_ATTACHSCP c = new CC_ATTACHSCP();
            c.nSCPId = ScpId;
            c.nChannelId = 0;
            
            bool flag = SendCommand(_commandValue, c);
            return flag;
        }

        public bool ReadStructureStatus(short ScpId)
        {
            CC_STRSRQ cc_strsq = new CC_STRSRQ();
            cc_strsq.nScpID = ScpId;
            cc_strsq.nListLength = 24;
            for (int i = 0; i < cc_strsq.nListLength; i++)
            {
                switch (i)
                {
                    case >= 15 and <= 19:
                        cc_strsq.nStructId[i] = (short)(i + 5);
                        break;
                    case 20:
                        cc_strsq.nStructId[i] = 26;
                        break;
                    case 21:
                        cc_strsq.nStructId[i] = 27;
                        break;
                    case 22:
                        cc_strsq.nStructId[i] = 33;
                        break;
                    case 23:
                        cc_strsq.nStructId[i] = 35;
                        break;
                    default:
                        cc_strsq.nStructId[i] = (short)(i + 1);
                        break;
                }
            }
            
            bool flag = SendCommand((short)enCfgCmnd.enCcStrSRq, cc_strsq);
            return flag;
        }

        public bool SetScpId(short oldId, short newId)
        {
            CC_SCPID cc = new CC_SCPID();
            cc.scp_number = oldId;
            cc.scp_id = newId;
            bool flag = SendCommand((short)enCfgCmnd.enCcScpID, cc);
            return flag;
        }



        #endregion

        #region Transaction

        public bool GetTransactionLogStatus(short ScpId)
        {
            CC_TRANSRQ c = new CC_TRANSRQ();
            c.scp_number = ScpId;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcTranSrq, c);
            return flag;
        }

        public bool SetTransactionLogIndex(short ScpId, bool isEnable)
        {
            var _commandValue = (short)enCfgCmnd.enCcTranIndex;
            CC_TRANINDEX cc_tranindex = new CC_TRANINDEX();
            cc_tranindex.scp_number = ScpId;
            cc_tranindex.tran_index = isEnable ? -2 : -1;
            
            bool flag = SendCommand(_commandValue, cc_tranindex);
            return flag;
        }

        #endregion



        #region Access Level

        public bool AccessLevelConfigurationExtended(short ScpId, short number, short TzAcr)
        {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            for (int i = 0; i < cc.tz.Length; i++)
            {

                cc.tz[i] = TzAcr;

            }
            
            bool flag = SendCommand((short)enCfgCmnd.enCcAlvlEx, cc);
            return flag;
        }


        public bool AccessLevelConfigurationExtendedCreate(short ScpId, short Number, List<CreateUpdateAccessLevelDoorTimeZoneDto> AccessLevelDoorTimeZoneDto)
        {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = Number;
            foreach (var d in AccessLevelDoorTimeZoneDto)
            {
                cc.tz[d.DoorId] = d.TimeZoneId;
            }
            
            bool flag = SendCommand((short)enCfgCmnd.enCcAlvlEx, cc);
            return flag;
        }


        #endregion

        #region SIO


        public bool SioDriverConfiguration(short ScpId, short SIODriverNo, short IOModulePort, int BaudRate, short ProtocolType)
        {
            CC_MSP1 cc_msp1 = new CC_MSP1();
            cc_msp1.lastModified = 0;
            cc_msp1.scp_number = ScpId;
            cc_msp1.msp1_number = SIODriverNo;
            cc_msp1.port_number = IOModulePort;
            cc_msp1.baud_rate = BaudRate;
            cc_msp1.reply_time = 90;
            cc_msp1.nProtocol = ProtocolType;
            cc_msp1.nDialect = 0;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcMsp1, cc_msp1);
            return flag;
        }



        public bool SioPanelConfiguration(short ScpId, short SioNo, short Model, short nInput, short nOutput, short nReader, short ModuleAddress, short SIODriverPort, bool isEnable)
        {

            CC_SIO cc_sio = new CC_SIO();
            cc_sio.lastModified = 0;
            cc_sio.scp_number = ScpId;
            cc_sio.sio_number = SioNo;
            cc_sio.nInputs = nInput;
            cc_sio.nOutputs = nOutput;
            cc_sio.nReaders = nReader;
            cc_sio.model = Model;
            cc_sio.revision = 0;
            cc_sio.ser_num_low = 0;
            cc_sio.ser_num_high = -1;
            cc_sio.enable = isEnable ? (short)1 : (short)0;
            cc_sio.port = SIODriverPort;
            cc_sio.channel_out = 0;
            cc_sio.channel_in = 0;
            cc_sio.address = ModuleAddress;
            cc_sio.e_max = 3;
            cc_sio.flags = 0x20;
            cc_sio.nSioNextIn = -1;
            cc_sio.nSioNextOut = -1;
            cc_sio.nSioNextRdr = -1;
            cc_sio.nSioConnectTest = 0;
            cc_sio.nSioOemCode = 0;
            cc_sio.nSioOemMask = 0;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcSio, cc_sio);
            return flag;
        }


        #endregion

        #region Control Point


        public bool OutputPointSpecification(short ScpId, short SioNo, short OutputNo, short OutputMode)
        {
            CC_OP cc_op = new CC_OP();
            cc_op.lastModified = 0;
            cc_op.scp_number = ScpId;
            cc_op.sio_number = SioNo;
            cc_op.output = OutputNo;
            cc_op.mode = OutputMode;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcOutput, cc_op);
            return flag;
        }


        public bool ControlPointConfiguration(short ScpId, short SioNo, short CpNo, short OutputNo, short DefaultPulseTime)
        {
            CC_CP cc_cp = new CC_CP();
            cc_cp.lastModified = 0;
            cc_cp.scp_number = ScpId;
            cc_cp.sio_number = SioNo;
            cc_cp.cp_number = CpNo;
            cc_cp.op_number = OutputNo;
            cc_cp.dflt_pulse = DefaultPulseTime;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcCP, cc_cp);
            return flag;

        }

       

        public bool ControlPointCommand(short ScpId, short cpNo, short command)
        {
            CC_CPCTL cc_cpctl = new CC_CPCTL();
            cc_cpctl.scp_number = ScpId;
            cc_cpctl.cp_number = cpNo;
            cc_cpctl.command = command;
            cc_cpctl.on_time = 0;
            cc_cpctl.off_time = 0;
            cc_cpctl.repeat = 0;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcCpCtl, cc_cpctl);
            return flag;

        }

        public bool GetCpStatus(short ScpId, short CpNo, short Count)
        {
            CC_CPSRQ cc = new CC_CPSRQ();
            cc.scp_number = ScpId;
            cc.first = CpNo;
            cc.count = Count;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcCpSrq, cc);
            return flag;
        }

        #endregion

        #region Monitor Point


        public bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime)
        {
            CC_IP cc_ip = new CC_IP();
            cc_ip.lastModified = 0;
            cc_ip.scp_number = ScpId;
            cc_ip.sio_number = SioNo;
            cc_ip.input = InputNo;
            cc_ip.icvt_num = InputMode;
            cc_ip.debounce = Debounce;
            cc_ip.hold_time = HoldTime;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcInput, cc_ip);
            return flag;
        }


        public bool MonitorPointConfiguration(short ScpId, short SioNo, short InputNo, short LfCode, short Mode, short DelayEntry, short DelayExit, short nMp)
        {
            CC_MP cc_mp = new CC_MP();
            cc_mp.lastModified = 0;
            cc_mp.scp_number = ScpId;
            cc_mp.sio_number = SioNo;
            cc_mp.mp_number = nMp;
            cc_mp.ip_number = InputNo;
            cc_mp.lf_code = LfCode;
            cc_mp.mode = Mode;
            cc_mp.delay_entry = DelayEntry;
            cc_mp.delay_exit = DelayExit;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcMP, cc_mp);
            return flag;
        }

        public bool MonitorPointMask(short ScpId, short MpNo, int SetClear)
        {
            CC_MPMASK cc = new CC_MPMASK();
            cc.scp_number = ScpId;
            cc.mp_number = MpNo;
            cc.set_clear = (short)SetClear;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcMpMask, cc);
            return flag;
        }

        public bool GetMpStatus(short ScpId, short MpNo, short Count)
        {
            CC_MPSRQ cc = new CC_MPSRQ();
            cc.scp_number = ScpId;
            cc.first = MpNo;
            cc.count = Count;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcMpSrq, cc);
            return flag;
        }



        #endregion

        #region Monitor Point Group

        public bool ConfigureMonitorPointGroup(short ScpId, short ComponentId, short nMonitor, List<MonitorGroupList> list)
        {
            CC_MPG c = new CC_MPG();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.mpg_number = ComponentId;
            c.nMpCount = nMonitor;
            int i = 0;
            foreach (var l in list)
            {
                c.nMpList[i] = l.point_type;
                i += 1;
                c.nMpList[i] = l.point_number;
            }
            
            bool flag = SendCommand((short)enCfgCmnd.enCcMpg, c);
            return flag;
        }

        public bool MonitorPointGroupArmDisarm(short ScpId, short ComponentId, short Command, short Arg1)
        {
            CC_MPGSET c = new CC_MPGSET();
            c.scp_number = ScpId;
            c.mpg_number = ComponentId;
            c.command = Command;
            c.arg1 = Arg1;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcMpgSet, c);
            //if (flag)
            //{
            //    return await TrackCommandAsync(tag, hardware_id, Constants.command.C321);
            //}
            return flag;
        }

        #endregion

        #region Reader

        public bool ReaderSpecification(short ScpId, short SioNo, short ReaderNo, short DataFormat, short KeyPadMode, short LedDriveMode, short OsdpFlag)
        {
            var _commandValue = (short)enCfgCmnd.enCcReader;
            CC_RDR cc_rdr = new CC_RDR();
            cc_rdr.lastModified = 0;
            cc_rdr.scp_number = ScpId;
            cc_rdr.sio_number = SioNo;
            cc_rdr.reader = ReaderNo;
            cc_rdr.dt_fmt = DataFormat;
            cc_rdr.keypad_mode = KeyPadMode;
            cc_rdr.led_drive_mode = LedDriveMode;
            cc_rdr.osdp_flags = OsdpFlag;
            //cc_rdr.device_id
            
            bool flag = SendCommand(_commandValue, cc_rdr);
            return flag;
        }

        #endregion

        #region Access Control Reader (ACR)

        public bool AccessControlReaderConfiguration(short ScpId, short AcrNo, Door dto)
        {
            CC_ACR cc_acr = new CC_ACR();
            cc_acr.lastModified = 0;
            cc_acr.scp_number = ScpId;
            cc_acr.acr_number = AcrNo;
            cc_acr.access_cfg = dto.access_config;
            cc_acr.pair_acr_number = dto.pair_door_no;
            cc_acr.rdr_sio = dto.readers.ElementAt(0).module_id;
            cc_acr.rdr_number = dto.readers.ElementAt(0).module_id;
            cc_acr.strk_sio = dto.strike.module_id;
            cc_acr.strk_number = dto.strike.output_no;
            cc_acr.strike_t_min = dto.strike.strike_min;
            cc_acr.strike_t_max = dto.strike.strike_max;
            cc_acr.strike_mode = dto.strike.strike_mode;
            cc_acr.door_sio = dto.sensor.module_id;
            cc_acr.door_number = dto.sensor.input_no;
            cc_acr.dc_held = dto.sensor.dc_held;
            if (dto.request_exits is not null)
            {
                cc_acr.rex0_sio = dto.request_exits.ElementAt(0).module_id;
                cc_acr.rex0_number = dto.request_exits.ElementAt(0).input_no;
                cc_acr.rex_tzmask[0] = dto.request_exits.ElementAt(0).mask_timezone;
                if (dto.request_exits.Count > 1)
                {
                    cc_acr.rex1_sio = dto.request_exits.ElementAt(1).module_id;
                    cc_acr.rex1_number = dto.request_exits.ElementAt(1).input_no;
                    cc_acr.rex_tzmask[1] = dto.request_exits.ElementAt(1).mask_timezone;
                }
            }
            if (dto.readers.Count > 1)
            {
                cc_acr.altrdr_sio = dto.readers.ElementAt(1).module_id;
                cc_acr.altrdr_number = dto.readers.ElementAt(1).reader_no;
                cc_acr.altrdr_spec = dto.reader_out_config;
            }
            cc_acr.cd_format = dto.card_format;
            cc_acr.apb_mode = dto.antipassback_mode;
            if (dto.antipassback_in > 0) cc_acr.apb_in = (short)dto.antipassback_in;
            if (dto.antipassback_out > 0) cc_acr.apb_to = (short)dto.antipassback_out!;
            if (dto.spare_tag != -1) cc_acr.spare = dto.spare_tag;
            if (dto.access_control_flag != -1) cc_acr.actl_flags = dto.access_control_flag;
            cc_acr.offline_mode = dto.offline_mode;
            cc_acr.default_mode = dto.default_mode;
            cc_acr.default_led_mode = dto.default_led_mode;
            cc_acr.pre_alarm = dto.pre_alarm;
            cc_acr.apb_delay = dto.antipassback_delay;
            cc_acr.strk_t2 = dto.strike_t2;
            cc_acr.dc_held2 = dto.dc_held2;
            cc_acr.strk_follow_pulse = 0;
            cc_acr.strk_follow_delay = 0;
            cc_acr.nAuthModFlags = 0;
            cc_acr.nExtFeatureType = 0;
            cc_acr.dfofFilterTime = 0;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcACR, cc_acr);
            return flag;

        }


        public bool MomentaryUnlock(short ScpId, short AcrNo)
        {
            CC_UNLOCK cc_unlock = new CC_UNLOCK();
            cc_unlock.scp_number = ScpId;
            cc_unlock.acr_number = AcrNo;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcUnlock, cc_unlock);
            return flag;

        }

        public bool GetAcrStatus(short ScpId, short AcrNo, short Count)
        {
            CC_ACRSRQ cc = new CC_ACRSRQ();
            cc.scp_number = ScpId;
            cc.first = AcrNo;
            cc.count = Count;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcAcrSrq, cc);
            return flag;
        }

        public bool AcrMode(short ScpId, short AcrNo, short Mode)
        {
            CC_ACRMODE cc = new CC_ACRMODE();
            cc.scp_number = ScpId;
            cc.acr_number = AcrNo;
            cc.acr_mode = Mode;
            cc.nAuthModFlags = 0;
            //cc.n_ext_feature_type
            
            bool flag = SendCommand((short)enCfgCmnd.enCcAcrMode, cc);
            return flag;
        }

        #endregion

        #region Access Area

        public bool ConfigureAccessArea(short ScpId, short AreaNo, short MultiOccu, short AccControl, short OccControl, short OccSet, short OccMax, short OccUp, short OccDown, short AreaFlag)
        {
            CC_AREA_SPC cc = new CC_AREA_SPC();
            cc.scp_number = ScpId;
            cc.area_number = AreaNo;
            cc.multi_occupancy = MultiOccu;
            cc.access_control = AccControl;
            cc.occ_control = OccControl;
            cc.occ_set = OccSet;
            cc.occ_max = OccMax;
            cc.occ_up = OccUp;
            cc.occ_down = OccDown;
            cc.area_flags = AreaFlag;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcAreaSpc, cc);
            return flag;
        }

        public bool GetAccessAreaStatus(short ScpId, short ComponentId, short Number)
        {
            CC_AREASRQ cc = new CC_AREASRQ();
            cc.scp_number = ScpId;
            cc.first = ComponentId;
            cc.count = Number;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcAreaSrq, cc);
            return flag;
        }

        #endregion

        #region Card Formatter

        public bool CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
        {
            CC_SCP_CFMT cc = new CC_SCP_CFMT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = FormatNo;
            cc.facility = facility;
            cc.offset = offset;
            cc.function_id = function_id;
            cc.arg.sensor.flags = flags;
            cc.arg.sensor.bits = bits;
            cc.arg.sensor.pe_ln = pe_ln;
            cc.arg.sensor.pe_loc = pe_loc;
            cc.arg.sensor.po_ln = po_ln;
            cc.arg.sensor.po_loc = po_loc;
            cc.arg.sensor.ch_ln = ch_ln;
            cc.arg.sensor.ch_loc = ch_loc;
            cc.arg.sensor.ic_ln = ic_ln;
            cc.arg.sensor.ic_loc = ic_loc;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcScpCfmt, cc);
            return flag;


        }

        public bool CardFormatterConfiguration(short ScpId, CardFormatDto dto, short funtionId)
        {
            CC_SCP_CFMT cc = new CC_SCP_CFMT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = dto.ComponentId;
            cc.facility = dto.Facility;
            cc.offset = 0;
            cc.function_id = funtionId;
            //cc.arg.sensor.flags = dto.f;
            cc.arg.sensor.bits = dto.Bits;
            cc.arg.sensor.pe_ln = dto.PeLn;
            cc.arg.sensor.pe_loc = dto.PeLoc;
            cc.arg.sensor.po_ln = dto.PoLn;
            cc.arg.sensor.po_loc = dto.PoLoc;
            cc.arg.sensor.ch_ln = dto.ChLn;
            cc.arg.sensor.ch_loc = dto.ChLoc;
            cc.arg.sensor.ic_ln = dto.IcLn;
            cc.arg.sensor.ic_loc = dto.IcLoc;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcScpCfmt, cc);
            return flag;

        }


        #endregion

        #region Credentials

        public bool AccessDatabaseCardRecord(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<AccessLevel> AccessLevel, int Active, int Deactive = 2085970000)
        {
            CC_ADBC_I64DTIC32 cc = new CC_ADBC_I64DTIC32();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.flags = Flags;
            cc.card_number = CardNumber;
            cc.issue_code = IssueCode;
            for (int i = 0; i < Pin.Length; i++)
            {
                cc.pin[i] = Pin[i];
                if (i == Pin.Length - 1)
                {
                    cc.pin[i] = '\0';
                }
            }
            for (int i = 0; i < AccessLevel.Count; i++)
            {
                cc.alvl[i] = AccessLevel[i].component_id;
            }
            cc.act_time = Active;
            cc.dact_time = Deactive;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcAdbCardI64DTic32, cc);
            return flag;

        }

        public bool CardDelete(short ScpId, long CardNo)
        {
            CC_CARDDELETEI64 cc = new CC_CARDDELETEI64();
            cc.scp_number = ScpId;
            cc.cardholder_id = CardNo;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcCardDeleteI64, cc);
            return flag;
        }


        #endregion

        #region Time


        public bool TimeSet(short ScpId)
        {
            CC_TIME cc_time = new CC_TIME();
            cc_time.scp_number = ScpId;
            cc_time.custom_time = 0;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcTime, cc_time);
            return flag;
        }

        #endregion

        #region Utility

        //////
        // Method: Turn on debug to file
        //////
        public void TurnOnDebug()
        {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
            if (flag)
            {
                Console.WriteLine("Debug to file on");
            }
            else
            {
                Console.WriteLine("Debug to file off");
            }

        }

        //////
        // Method: Turn off debug to file
        //////
        public void TurnOffDebug()
        {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugOff);
            if (flag)
            {
                Console.WriteLine("Debug to file off");
            }
            else
            {
                Console.WriteLine("Debug to file on");
            }
        }

        //////
        // Method: SendCommand to Driver
        // PreCondition: SCP online
        // PostCondition: Pass/Fail response is sent from the driver
        //////
        public async Task<bool> SendASCIICommand(string command)
        {
            // send the command
            return SCPDLL.scpConfigCommand(command);
        }

        //////
        // Method: SendCommand to Driver
        // PreCondition: SCP online
        // PostCondition: Pass/Fail response is sent from the driver
        //////
        public bool SendCommand(short command, IConfigCommand cfg)
        {
            SCPConfig scp = new SCPConfig();
            bool success = scp.scpCfgCmndEx(command, cfg);
            return success;
        }

        public short CheckSCPStatus(short scpID)
        {
            return SCPDLL.scpCheckOnline(scpID);
        }

        public bool ResetSCP(short ScpId)
        {
            CC_RESET cc_reset = new CC_RESET();
            cc_reset.scp_number = ScpId;
            bool flag = SendCommand((short)enCfgCmnd.enCcReset, cc_reset);
            return flag;
        }

        public bool GetIdReport(short ScpId)
        {
            CC_IDREQUEST cc_idrequest = new CC_IDREQUEST();
            cc_idrequest.scp_number = ScpId;
            
            bool flag = SendCommand((short)enCfgCmnd.enCcIDRequest, cc_idrequest);
            //if (flag)
            //{
            //    return await TrackCommandAsync(tag, hardware_id, Constants.command.C401);
            //}
            return flag;
        }

        public bool GetWebConfigRead(short ScpId,short type)
        {
            CC_WEB_CONFIG_READ cc = new CC_WEB_CONFIG_READ();
            cc.scp_number = ScpId;
            cc.read_type = type;

            bool flag = SendCommand((short)enCfgCmnd.enCcWebConfigRead, cc);
            //if (flag)
            //{
            //    tag_no = SCPDLL.scpGetTagLastPosted(hardware_id);
            //    command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("command Tag : " + tag_no);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }

        public bool GetCpSrq(short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcCpSrq;
            CC_CPSRQ cc_cpsrq = new CC_CPSRQ();
            cc_cpsrq.scp_number = ScpId;
            cc_cpsrq.first = 0;
            cc_cpsrq.count = 1;
            bool flag = SendCommand(_commandValue, cc_cpsrq);
            //if (flag)
            //{
            //    tag_no = SCPDLL.scpGetTagLastPosted(hardware_id);
            //    command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("command Tag : " + tag_no);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }

        public bool GetSioStatus(short ScpId, short SioNo)
        {
            CC_SIOSRQ cc_siosrq = new CC_SIOSRQ();
            cc_siosrq.scp_number = ScpId;
            cc_siosrq.first = SioNo;
            cc_siosrq.count = 1;
            bool flag = SendCommand((short)enCfgCmnd.enCcSioSrq, cc_siosrq);
            return flag;
        }






        #endregion

        #region Trigger

        public bool TriggerSpecification(short ScpId, Trigger dto, short ComponentId)
        {
            CC_TRGR cc = new CC_TRGR();
            cc.scp_number = ScpId;
            cc.trgr_number = ComponentId;
            cc.command = dto.command;
            cc.proc_num = dto.procedure_id;
            cc.src_type = dto.source_type;
            cc.src_number = dto.source_number;
            cc.tran_type = dto.tran_type;
            foreach (var code in dto.code_map)
            {
                cc.code_map += (int)Math.Pow(2, code.value);
            }
            cc.timezone = dto.timezone;
            switch (dto.tran_type)
            {
                case (short)tranType.tranTypeCardFull:
                    break;
                case (short)tranType.tranTypeCardID:
                    break;
                case (short)tranType.tranTypeCoS:
                    break;
                case (short)tranType.tranTypeCoSDoor:
                    break;
                case (short)tranType.tranTypeUserCmnd:
                    break;
                case (short)tranType.tranTypeOperatingMode:
                    break;
                case (short)tranType.tranTypeAcrExtFeatureCoS:
                    break;
                default:
                    break;
            }

            // trigger Variable
            //cc.trig_var[0] = 0;

            // transaction type_desc Arg
            //cc.arg[0] = 0;


            
            bool flag = SendCommand((short)enCfgCmnd.enCcTrgr, cc);
            return flag;

        }

        #endregion

        #region Procedure

        public bool ActionSpecificationAsyncForAllHW(short ComponentId, Entity.Action action, List<short> ScpIds)
        {
            foreach (var id in ScpIds)
            {
                CC_ACTN c = new CC_ACTN(action.action_type);
                c.hdr.lastModified = 0;
                c.hdr.scp_number = id;
                c.hdr.proc_number = ComponentId;
                c.hdr.action_type = action.action_type;

                switch (action.action_type)
                {
                    case 1:
                        c.mp_mask.scp_number = id;
                        c.mp_mask.mp_number = action.arg1;
                        c.mp_mask.set_clear = action.arg2;
                        break;
                    case 2:
                        c.cp_ctl.scp_number = id;
                        c.cp_ctl.cp_number = action.arg1;
                        c.cp_ctl.command = action.arg2;
                        c.cp_ctl.on_time = action.arg3;
                        c.cp_ctl.off_time = action.arg4;
                        c.cp_ctl.repeat = action.arg5;
                        break;
                    case 3:
                        c.acr_mode.scp_number = id;
                        c.acr_mode.acr_number = action.arg1;
                        c.acr_mode.acr_mode = action.arg2;
                        break;
                    case 4:
                        c.fo_mask.scp_number = id;
                        c.fo_mask.acr_number = action.arg1;
                        c.fo_mask.set_clear = action.arg2;
                        break;
                    case 5:
                        c.ho_mask.scp_number = id;
                        c.ho_mask.acr_number = action.arg1;
                        c.ho_mask.set_clear = action.arg2;
                        break;
                    case 6:
                        c.unlock.scp_number = id;
                        c.unlock.acr_number = action.arg1;
                        c.unlock.floor_number = action.arg2;
                        c.unlock.strk_tm = action.arg3;
                        c.unlock.t_held = action.arg4;
                        c.unlock.t_held_pre = action.arg5;
                        break;
                    case 7:
                        c.proc.scp_number = id;
                        c.proc.proc_number = action.arg1;
                        c.proc.command = action.arg2;
                        break;
                    case 8:
                        c.tv_ctl.scp_number = id;
                        c.tv_ctl.tv_number = action.arg1;
                        c.tv_ctl.set_clear = action.arg2;
                        break;
                    case 9:
                        c.tz_ctl.scp_number = id;
                        c.tz_ctl.tz_number = action.arg1;
                        c.tz_ctl.command = action.arg2;
                        break;
                    case 10:
                        c.led_mode.scp_number = id;
                        c.led_mode.acr_number = action.arg1;
                        c.led_mode.led_mode = action.arg2;
                        break;
                    case 14:
                        c.mpg_set.scp_number = id;
                        c.mpg_set.mpg_number = action.arg1;
                        c.mpg_set.command = action.arg2;
                        c.mpg_set.arg1 = action.arg3;
                        break;
                    case 15:
                        c.mpg_test_mask.scp_number = id;
                        c.mpg_test_mask.mpg_number = action.arg1;
                        c.mpg_test_mask.action_prefix_ifz = action.arg2;
                        c.mpg_test_mask.action_prefix_ifnz = action.arg3;
                        break;
                    case 16:
                        c.mpg_test_active.scp_number = id;
                        c.mpg_test_active.mpg_number = action.arg1;
                        c.mpg_test_active.action_prefix_ifnoactive = action.arg2;
                        c.mpg_test_active.action_prefix_ifactive = action.arg3;
                        break;
                    case 17:
                        c.area_set.scp_number = id;
                        c.area_set.area_number = action.arg1;
                        c.area_set.command = action.arg2;
                        c.area_set.occ_set = action.arg3;
                        break;
                    case 18:
                        c.unlock.scp_number = id;
                        c.unlock.acr_number = action.arg1;
                        c.unlock.floor_number = action.arg2;
                        c.unlock.strk_tm = action.arg3;
                        c.unlock.t_held = action.arg4;
                        c.unlock.t_held_pre = action.arg5;
                        break;
                    case 19:
                        c.rled_tmp.scp_number = id;
                        c.rled_tmp.acr_number = action.arg1;
                        c.rled_tmp.color_on = action.arg2;
                        c.rled_tmp.color_off = action.arg3;
                        c.rled_tmp.ticks_on = action.arg4;
                        c.rled_tmp.ticks_off = action.arg5;
                        c.rled_tmp.repeat = action.arg6;
                        c.rled_tmp.beeps = action.arg7;
                        break;
                    case 20:
                        //c.lcd_text.
                        break;
                    case 24:
                        c.temp_acr_mode.scp_number = id;
                        c.temp_acr_mode.acr_number = action.arg1;
                        c.temp_acr_mode.acr_mode = action.arg2;
                        c.temp_acr_mode.time = action.arg3;
                        c.temp_acr_mode.nAuthModFlags = action.arg4;
                        break;
                    case 25:
                        c.card_sim.nScp = id;
                        c.card_sim.nCommand = 1;
                        c.card_sim.nAcr = action.arg1;
                        c.card_sim.e_time = action.arg2;
                        c.card_sim.nFmtNum = action.arg3;
                        c.card_sim.nFacilityCode = action.arg4;
                        c.card_sim.nCardholderId = action.arg5;
                        c.card_sim.nIssueCode = action.arg6;
                        break;
                    case 26:
                        c.use_limit.scp_number = id;
                        c.use_limit.cardholder_id = action.arg1;
                        c.use_limit.new_limit = action.arg2;
                        break;
                    case 27:
                        c.oper_mode.scp_number = id;
                        c.oper_mode.oper_mode = action.arg1;
                        c.oper_mode.enforce_existing = action.arg2;
                        c.oper_mode.existing_mode = action.arg3;
                        break;
                    case 28:
                        c.key_sim.nScp = id;
                        c.key_sim.nAcr = action.arg1;
                        c.key_sim.e_time = action.arg2;
                        for (int i = 0; i < action.str_arg.Length; i++)
                        {
                            c.key_sim.keys[i] = action.str_arg[i];
                        }
                        break;
                    case 30:
                        c.batch_trans.nScpID = id;
                        c.batch_trans.trigNumber = (ushort)action.arg1;
                        c.batch_trans.actType = (ushort)action.arg2;
                        break;
                    case 126:
                    case 127:
                        c.delay.delay_time = action.arg1;
                        break;
                    default:
                        break;
                }
                
                bool flag = SendCommand((short)enCfgCmnd.enCcProc, c);
                return flag;

            }

            return false;
        }

        public bool ActionSpecificationDelayAsync(short ComponentId, Entity.Action action)
        {

            CC_ACTN c = new CC_ACTN(127);
            c.hdr.lastModified = 0;
            c.hdr.scp_number = action.hardware_id;
            c.hdr.proc_number = ComponentId;
            c.hdr.action_type = 127;
            c.delay.delay_time = action.arg1;
            //c.hdr.arg
            
            bool flag = SendCommand((short)enCfgCmnd.enCcProc, c);
            return flag;
        }


        public bool ActionSpecificationAsync(short ComponentId, List<Entity.Action> en)
        {
            foreach (var action in en)
            {
                if (action.action_type == 9 || action.action_type == 127) continue;
                CC_ACTN c = new CC_ACTN(action.action_type);
                c.hdr.lastModified = 0;
                c.hdr.scp_number = action.hardware_id;
                c.hdr.proc_number = ComponentId;
                c.hdr.action_type = action.action_type;
                //c.hdr.arg
                switch (action.action_type)
                {
                    case 1:
                        c.mp_mask.scp_number = action.hardware_id;
                        c.mp_mask.mp_number = action.arg1;
                        c.mp_mask.set_clear = action.arg2;
                        break;
                    case 2:
                        c.cp_ctl.scp_number = action.hardware_id;
                        c.cp_ctl.cp_number = action.arg1;
                        c.cp_ctl.command = action.arg2;
                        c.cp_ctl.on_time = action.arg3;
                        c.cp_ctl.off_time = action.arg4;
                        c.cp_ctl.repeat = action.arg5;
                        break;
                    case 3:
                        c.acr_mode.scp_number = action.hardware_id;
                        c.acr_mode.acr_number = action.arg1;
                        c.acr_mode.acr_mode = action.arg2;
                        break;
                    case 4:
                        c.fo_mask.scp_number = action.hardware_id;
                        c.fo_mask.acr_number = action.arg1;
                        c.fo_mask.set_clear = action.arg2;
                        break;
                    case 5:
                        c.ho_mask.scp_number = action.hardware_id;
                        c.ho_mask.acr_number = action.arg1;
                        c.ho_mask.set_clear = action.arg2;
                        break;
                    case 6:
                        c.unlock.scp_number = action.hardware_id;
                        c.unlock.acr_number = action.arg1;
                        c.unlock.floor_number = action.arg2;
                        c.unlock.strk_tm = action.arg3;
                        c.unlock.t_held = action.arg4;
                        c.unlock.t_held_pre = action.arg5;
                        break;
                    case 7:
                        c.proc.scp_number = action.hardware_id;
                        c.proc.proc_number = action.arg1;
                        c.proc.command = action.arg2;
                        break;
                    case 8:
                        c.tv_ctl.scp_number = action.hardware_id;
                        c.tv_ctl.tv_number = action.arg1;
                        c.tv_ctl.set_clear = action.arg2;
                        break;
                    case 9:
                        c.tz_ctl.scp_number = action.hardware_id;
                        c.tz_ctl.tz_number = action.arg1;
                        c.tz_ctl.command = action.arg2;
                        break;
                    case 10:
                        c.led_mode.scp_number = action.hardware_id;
                        c.led_mode.acr_number = action.arg1;
                        c.led_mode.led_mode = action.arg2;
                        break;
                    case 14:
                        c.mpg_set.scp_number = action.hardware_id;
                        c.mpg_set.mpg_number = action.arg1;
                        c.mpg_set.command = action.arg2;
                        c.mpg_set.arg1 = action.arg3;
                        break;
                    case 15:
                        c.mpg_test_mask.scp_number = action.hardware_id;
                        c.mpg_test_mask.mpg_number = action.arg1;
                        c.mpg_test_mask.action_prefix_ifz = action.arg2;
                        c.mpg_test_mask.action_prefix_ifnz = action.arg3;
                        break;
                    case 16:
                        c.mpg_test_active.scp_number = action.hardware_id;
                        c.mpg_test_active.mpg_number = action.arg1;
                        c.mpg_test_active.action_prefix_ifnoactive = action.arg2;
                        c.mpg_test_active.action_prefix_ifactive = action.arg3;
                        break;
                    case 17:
                        c.area_set.scp_number = action.hardware_id;
                        c.area_set.area_number = action.arg1;
                        c.area_set.command = action.arg2;
                        c.area_set.occ_set = action.arg3;
                        break;
                    case 18:
                        c.unlock.scp_number = action.hardware_id;
                        c.unlock.acr_number = action.arg1;
                        c.unlock.floor_number = action.arg2;
                        c.unlock.strk_tm = action.arg3;
                        c.unlock.t_held = action.arg4;
                        c.unlock.t_held_pre = action.arg5;
                        break;
                    case 19:
                        c.rled_tmp.scp_number = action.hardware_id;
                        c.rled_tmp.acr_number = action.arg1;
                        c.rled_tmp.color_on = action.arg2;
                        c.rled_tmp.color_off = action.arg3;
                        c.rled_tmp.ticks_on = action.arg4;
                        c.rled_tmp.ticks_off = action.arg5;
                        c.rled_tmp.repeat = action.arg6;
                        c.rled_tmp.beeps = action.arg7;
                        break;
                    case 20:
                        //c.lcd_text.
                        break;
                    case 24:
                        c.temp_acr_mode.scp_number = action.hardware_id;
                        c.temp_acr_mode.acr_number = action.arg1;
                        c.temp_acr_mode.acr_mode = action.arg2;
                        c.temp_acr_mode.time = action.arg3;
                        c.temp_acr_mode.nAuthModFlags = action.arg4;
                        break;
                    case 25:
                        c.card_sim.nScp = action.hardware_id;
                        c.card_sim.nCommand = 1;
                        c.card_sim.nAcr = action.arg1;
                        c.card_sim.e_time = action.arg2;
                        c.card_sim.nFmtNum = action.arg3;
                        c.card_sim.nFacilityCode = action.arg4;
                        c.card_sim.nCardholderId = action.arg5;
                        c.card_sim.nIssueCode = action.arg6;
                        break;
                    case 26:
                        c.use_limit.scp_number = action.hardware_id;
                        c.use_limit.cardholder_id = action.arg1;
                        c.use_limit.new_limit = action.arg2;
                        break;
                    case 27:
                        c.oper_mode.scp_number = action.hardware_id;
                        c.oper_mode.oper_mode = action.arg1;
                        c.oper_mode.enforce_existing = action.arg2;
                        c.oper_mode.existing_mode = action.arg3;
                        break;
                    case 28:
                        c.key_sim.nScp = action.hardware_id;
                        c.key_sim.nAcr = action.arg1;
                        c.key_sim.e_time = action.arg2;
                        for (int i = 0; i < action.str_arg.Length; i++)
                        {
                            c.key_sim.keys[i] = action.str_arg[i];
                        }
                        break;
                    case 30:
                        c.batch_trans.nScpID = action.hardware_id;
                        c.batch_trans.trigNumber = (ushort)action.arg1;
                        c.batch_trans.actType = (ushort)action.arg2;
                        break;
                    case 126:
                    case 127:
                        c.delay.delay_time = action.arg1;
                        break;
                    default:
                        break;
                }
                
                bool flag = SendCommand((short)enCfgCmnd.enCcProc, c);
                return flag;

            }
            return false;
        }


        #endregion



    }
}
