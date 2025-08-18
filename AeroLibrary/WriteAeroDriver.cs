using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Dto;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Dto.Time;
using HIDAeroService.Entity;
using HIDAeroService.Models;
using HIDAeroService.Service;
using MiNET.Entities.Passive;


namespace HIDAeroService.AeroLibrary
{
    public sealed class WriteAeroDriver
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public int TagNo { get; private set; } = 0;
        public WriteAeroDriver() 
        {
        }



        #region Initial Driver

        //////
        // Method: SendCommand to Driver
        // Target: For Setting Base Driver Specification
        // port: Port number of driver that Controller need to set
        // maxController: maximum controller that can have in system 1-16384
        //////
        public bool SystemLevelSpecification(short nPort,short nScp)
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
            bool flag = SendCommand((Int16)enCfgCmnd.enCcSystem, cc_sys);
            return flag;
        }
      

        //////
        // Method: SendCommand to Driver
        // Target: For Create Channel
        // channelId: Channel Id to create
        // commuType: Communication Type ip-client / ip-server
        // port: Port number of driver that Controller need to set
        // controllerReplyTimeout: maximum controller Reply Waiting Before Timeout Default 3000ms
        //////
        public bool CreateChannel(short cPort)
        {
            CC_CHANNEL cc_channel = new CC_CHANNEL();
            cc_channel.nChannelId = 1;
            cc_channel.cType = 7;
            cc_channel.cPort = cPort;
            cc_channel.baud_rate = 0;
            cc_channel.timer1 = 3000;
            cc_channel.timer2 = 0;
            for (int i = 0; i < cc_channel.cModemId.Length; i++)
            {
                cc_channel.cModemId[i] = '\0';
            }
            cc_channel.cRTSMode = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcCreateChannel, cc_channel);
            return flag;
        }



        #endregion

        #region Validate SCP Connection

        public bool SCPDeviceSpecification(short ScpId)
        {
            CC_SCP_SCP cc_scp_scp = new CC_SCP_SCP();
            cc_scp_scp.lastModified = 0;
            cc_scp_scp.number = ScpId;
            cc_scp_scp.ser_num_low = 0;
            cc_scp_scp.ser_num_high = 0;
            cc_scp_scp.rev_major = 0;
            cc_scp_scp.rev_minor = 0;
            cc_scp_scp.nMsp1Port = 3;
            cc_scp_scp.nTransactions = 60000;
            cc_scp_scp.nSio = 16;
            cc_scp_scp.nMp = 615;
            cc_scp_scp.nCp = 388;
            cc_scp_scp.nAcr = 64;
            cc_scp_scp.nAlvl = 32000;
            cc_scp_scp.nTrgr = 1024;
            cc_scp_scp.nProc = 1024;
            cc_scp_scp.gmt_offset = -25200;
            cc_scp_scp.nDstID = 0;
            cc_scp_scp.nTz = 255;
            cc_scp_scp.nHol = 255;
            cc_scp_scp.nMpg = 128;
            cc_scp_scp.nTranLimit = 60000;
            cc_scp_scp.nAuthModType = 0;
            cc_scp_scp.nOperModes = 0;
            cc_scp_scp.oper_type = 1;
            cc_scp_scp.nLanguages = 0;
            cc_scp_scp.nSrvcType = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcScpScp, cc_scp_scp);
            if(flag) {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool AccessDatabaseSpecification(short ScpID)
        {
            CC_SCP_ADBS cc_scp_adbs = new CC_SCP_ADBS();
            cc_scp_adbs.lastModified = 0;
            cc_scp_adbs.nScpID = ScpID;
            cc_scp_adbs.nCards = 200;
            //cc_scp_adbs.nCards = 100;
            cc_scp_adbs.nAlvl = 32;
            // Pin Constant = 1
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
            bool flag = SendCommand((Int16)enCfgCmnd.enCcScpAdbSpec, cc_scp_adbs);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region SCP

        public bool DeleteScp(short ScpId)
        {
            CC_NEWSCP c = new CC_NEWSCP();
            c.nSCPId = ScpId;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcDeleteScp,c);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool DetachScp(short ScpId)
        {
            CC_ATTACHSCP c = new CC_ATTACHSCP();
            c.nSCPId = ScpId;
            c.nChannelId = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcDetachScp, c);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
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
                        cc_strsq.nStructId[i] = (short)(i +1);
                        break;
                }
            }
            bool flag = SendCommand((Int16)enCfgCmnd.enCcStrSRq, cc_strsq);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Transaction

        public bool SetTransactionLogIndex(short scpID, bool isEnable)
        {
            CC_TRANINDEX cc_tranindex = new CC_TRANINDEX();
            cc_tranindex.scp_number = scpID;
            cc_tranindex.tran_index = isEnable ? -2 : -1;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcTranIndex, cc_tranindex);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(scpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Time Zone

        public bool ExtendedTimeZoneActSpecification(short ScpId, short TzNumber, short mode, short actTime, short deactTime, short intervals, List<TimeZoneInterval> interval)
        {
            CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = TzNumber;
            cc.mode = mode;
            cc.actTime = actTime;
            cc.deactTime = deactTime;
            cc.intervals = intervals;
            if(intervals > 0)
            {
                int i = 0;
                foreach(var dto in interval)
                {
                    cc.i[i].i_days = dto.IDays;
                    cc.i[i].i_start = dto.IStart;
                    cc.i[i].i_end = dto.IEnd;
                    i++;
                }
  
            }

            bool flag = SendCommand((Int16)enCfgCmnd.enCcScpTimezoneExAct, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }

            return flag;
        }

        #endregion

        #region Access Level

        public bool AccessLevelConfigurationExtended(short ScpId, short number)
        {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            for (int i = 0; i < cc.tz.Length; i++)
            {
                cc.tz[i] = 1;

            }
            bool flag = SendCommand((Int16)enCfgCmnd.enCcAlvlEx, cc);
            return flag;
        }

        public bool CreateAccessLevel(short ScpId, short Number,List<CreateAccessLevelDoor> Doors)
        {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = Number;
            for (int i = 0; i < cc.tz.Length; i++)
                cc.tz[i] = Doors.FirstOrDefault(d => d.AcrNumber == i)?.TzNumber ?? cc.tz[i];
            bool flag = SendCommand((Int16)enCfgCmnd.enCcAlvlEx, cc);
            return flag;
        }


        #endregion

        #region SIO

        public bool SIODriverConfiguration(short ScpID, short SIODriverNo, short IOModulePort, int BaudRate, short ProtocolType)
        {
            CC_MSP1 cc_msp1 = new CC_MSP1();
            cc_msp1.lastModified = 0;
            cc_msp1.scp_number = ScpID;
            cc_msp1.msp1_number = SIODriverNo;
            cc_msp1.port_number = IOModulePort;
            cc_msp1.baud_rate = BaudRate;
            cc_msp1.reply_time = 90;
            cc_msp1.nProtocol = ProtocolType;
            cc_msp1.nDialect = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcMsp1, cc_msp1);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool SIOPanelConfiguration(short ScpID, short SioNo, short ModelNo, short ModuleAddress, short SIODriverPort, bool isEnable)
        {
            short nInput, nOutput, nReaders;
            short[] n = Utility.GetSCPComponent(ModelNo);
            nInput = n[0];
            nOutput = n[1];
            nReaders = n[2];

            CC_SIO cc_sio = new CC_SIO();
            cc_sio.lastModified = 0;
            cc_sio.scp_number = ScpID;
            cc_sio.sio_number = SioNo;
            cc_sio.nInputs = nInput;
            cc_sio.nOutputs = nOutput;
            cc_sio.nReaders = nReaders;
            cc_sio.model = ModelNo;
            cc_sio.revision = 0;
            cc_sio.ser_num_low = 0;
            cc_sio.ser_num_high = -1;
            cc_sio.enable = isEnable ? (short)1 : (short)0;
            cc_sio.port = SIODriverPort;
            cc_sio.channel_out = 0;
            cc_sio.channel_in = 0;
            cc_sio.address = ModuleAddress;
            cc_sio.e_max = 3;
            cc_sio.flags = (short)0x20;
            cc_sio.nSioNextIn = -1;
            cc_sio.nSioNextOut = -1;
            cc_sio.nSioNextRdr = -1;
            cc_sio.nSioConnectTest = 0;
            cc_sio.nSioOemCode = 0;
            cc_sio.nSioOemMask = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcSio, cc_sio);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Control Point

        public bool OutputPointSpecification(short ScpID, short SioNo, short OutputNo, short OutputMode)
        {
            CC_OP cc_op = new CC_OP();
            cc_op.lastModified = 0;
            cc_op.scp_number = ScpID;
            cc_op.sio_number = SioNo;
            cc_op.output = OutputNo;
            cc_op.mode = OutputMode;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcOutput, cc_op);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool ControlPointConfiguration(short ScpID, short SioNo, short CpNo, short OutputNo,short DefaultPulseTime)
        {
            CC_CP cc_cp = new CC_CP();
            cc_cp.lastModified = 0;
            cc_cp.scp_number = ScpID;
            cc_cp.sio_number = SioNo;
            cc_cp.cp_number = CpNo;
            cc_cp.op_number = OutputNo;
            cc_cp.dflt_pulse = DefaultPulseTime;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcCP, cc_cp);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool ControlPointCommand(short ScpID, short c, short command)
        {
            CC_CPCTL cc_cpctl = new CC_CPCTL();
            cc_cpctl.scp_number = ScpID;
            cc_cpctl.cp_number = c;
            cc_cpctl.command = command;
            cc_cpctl.on_time = 0;
            cc_cpctl.off_time = 0;
            cc_cpctl.repeat = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcCpCtl, cc_cpctl);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool GetCpStatus(short ScpId,short CpNo,short Count)
        {
            CC_CPSRQ cc = new CC_CPSRQ();
            cc.scp_number = ScpId;
            cc.first = CpNo;
            cc.count = Count;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcCpSrq,cc);
            return flag;
        }

        #endregion

        #region Monitor Point

        public bool InputPointSpecification(short ScpID, short SioNo, short InputNo, short InputMode,short Debounce,short HoldTime)
        {
            CC_IP cc_ip = new CC_IP();
            cc_ip.lastModified = 0;
            cc_ip.scp_number = ScpID;
            cc_ip.sio_number = SioNo;
            cc_ip.input = InputNo;
            cc_ip.icvt_num = InputMode;
            cc_ip.debounce = Debounce;
            cc_ip.hold_time = HoldTime;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcInput, cc_ip);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool MonitorPointConfiguration(short ScpID, short SioNo, short InputNo,short LfCode,short Mode,short DelayEntry,short DelayExit,short nMp)
        {
            CC_MP cc_mp = new CC_MP();
            cc_mp.lastModified = 0;
            cc_mp.scp_number = ScpID;
            cc_mp.sio_number = SioNo;
            cc_mp.mp_number = nMp;
            cc_mp.ip_number = InputNo;
            cc_mp.lf_code = LfCode;
            cc_mp.mode = Mode;
            cc_mp.delay_entry = DelayEntry;
            cc_mp.delay_exit = DelayExit;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcMP, cc_mp);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool GetMpStatus(short ScpId, short MpNo, short Count)
        {
            CC_MPSRQ cc = new CC_MPSRQ();
            cc.scp_number = ScpId;
            cc.first = MpNo;
            cc.count = Count;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcMpSrq, cc);
            return flag;
        }


        #endregion

        #region Reader

        public bool ReaderSpecification(short ScpID, short SioNo, short ReaderNo,short DataFormat,short KeyPadMode,short LedDriveMode,short OsdpFlag)
        {
            CC_RDR cc_rdr = new CC_RDR();
            cc_rdr.lastModified = 0;
            cc_rdr.scp_number = ScpID;
            cc_rdr.sio_number = SioNo;
            cc_rdr.reader = ReaderNo;
            cc_rdr.dt_fmt = DataFormat;
            cc_rdr.keypad_mode = KeyPadMode;
            cc_rdr.led_drive_mode = LedDriveMode;
            cc_rdr.osdp_flags = OsdpFlag;
            //cc_rdr.device_id
            bool flag = SendCommand((Int16)enCfgCmnd.enCcReader, cc_rdr);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Access Control Reader (ACR)

        public bool AccessControlReaderConfiguration(short ScpID, short SIONo, short ReaderNo, short OutputNo, short InputNo, short ReqExNo,short nAcr)
        {
            CC_ACR cc_acr = new CC_ACR();
            cc_acr.lastModified = 0;
            cc_acr.scp_number = ScpID;
            cc_acr.acr_number = nAcr;
            cc_acr.access_cfg = 0;
            cc_acr.pair_acr_number = -1;
            cc_acr.rdr_sio = SIONo;
            cc_acr.rdr_number = ReaderNo;
            cc_acr.strk_sio = SIONo;
            cc_acr.strk_number = OutputNo;
            cc_acr.strike_t_min = 1;
            cc_acr.strike_t_max = 10;
            cc_acr.strike_mode = 2;
            cc_acr.door_sio = SIONo;
            cc_acr.door_number = InputNo;
            cc_acr.dc_held = 10;
            cc_acr.rex0_sio = SIONo;
            cc_acr.rex0_number = ReqExNo;
            cc_acr.rex1_sio = -1;
            cc_acr.rex1_number = -1;
            for (short i = 0; i < cc_acr.rex_tzmask.Length; i++)
            {
                cc_acr.rex_tzmask[i] = 0;
            }
            cc_acr.altrdr_sio = -1;
            cc_acr.altrdr_number = -1;
            cc_acr.altrdr_spec = 0;
            cc_acr.cd_format = 255;
            cc_acr.apb_mode = 0;
            cc_acr.apb_in = -1;
            cc_acr.apb_to = 1;
            //cc_acr.spare
            //cc_acr.actl_flags
            cc_acr.offline_mode = 2;
            cc_acr.default_mode = 5;
            cc_acr.default_led_mode = 0;
            cc_acr.pre_alarm = 0;
            cc_acr.apb_delay = 0;
            //cc_acr.strk_t2
            //cc_acr.dc_held2
            cc_acr.strk_follow_pulse = 0;
            cc_acr.strk_follow_delay = 0;
            cc_acr.nAuthModFlags = 0;
            cc_acr.nExtFeatureType = 0;
            cc_acr.dfofFilterTime = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcACR, cc_acr);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        public bool AccessControlReaderConfigurationForCreate(short ScpId,short AcrNo,AddACRDto dto)
        {
            CC_ACR cc_acr = new CC_ACR();
            cc_acr.lastModified = 0;
            cc_acr.scp_number = ScpId;
            cc_acr.acr_number = AcrNo;
            cc_acr.access_cfg = dto.AccessConfig;
            cc_acr.pair_acr_number = dto.PairACRNo;
            cc_acr.rdr_sio = dto.ReaderSioNumber;
            cc_acr.rdr_number = dto.ReaderNumber;
            cc_acr.strk_sio = dto.StrikeSioNumber;
            cc_acr.strk_number = dto.StrikeNumber;
            cc_acr.strike_t_min = dto.StrikeMinActiveTime;
            cc_acr.strike_t_max = dto.StrikeMaxActiveTime;
            cc_acr.strike_mode = dto.StrikeMode;
            cc_acr.door_sio = dto.SensorSioNumber;
            cc_acr.door_number = dto.SensorNumber;
            cc_acr.dc_held = dto.HeldOpenDelay;
            cc_acr.rex0_sio = dto.REX0SioNumber;
            cc_acr.rex0_number = dto.REX0Number;
            cc_acr.rex1_sio = dto.REX1SioNumber;
            cc_acr.rex1_number = dto.REX1Number;
            cc_acr.rex_tzmask[0] = 0;
            cc_acr.rex_tzmask[1] = 0;
            cc_acr.altrdr_sio = dto.AlternateReaderSioNumber;
            cc_acr.altrdr_number = dto.AlternateReaderNumber;
            cc_acr.altrdr_spec = dto.AlternateReaderConfig;
            cc_acr.cd_format = 255;
            cc_acr.apb_mode = dto.AntiPassbackMode;
            cc_acr.apb_in = dto.AntiPassBackIn;
            cc_acr.apb_to = dto.AntiPassBackOut;
            //cc_acr.spare = dto.SpareTags;
            //cc_acr.actl_flags = dto.AccessControlFlags;
            cc_acr.offline_mode = dto.OfflineMode;
            cc_acr.default_mode = dto.DefaultMode;
            cc_acr.default_led_mode = dto.DefaultLEDMode;
            cc_acr.pre_alarm = dto.PreAlarm;
            cc_acr.apb_delay = dto.AntiPassbackDelay;
            //cc_acr.strk_t2
            //cc_acr.dc_held2
            cc_acr.strk_follow_pulse = 0;
            cc_acr.strk_follow_delay = 0;
            cc_acr.nAuthModFlags = 0;
            cc_acr.nExtFeatureType = 0;
            cc_acr.dfofFilterTime = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcACR, cc_acr);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        public bool MomentaryUnlock(short ScpID, short AcrNo)
        {
            CC_UNLOCK cc_unlock = new CC_UNLOCK();
            cc_unlock.scp_number = ScpID;
            cc_unlock.acr_number = AcrNo;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcUnlock, cc_unlock);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        public bool GetAcrStatus(short ScpId, short AcrNo, short Count)
        {
            CC_ACRSRQ cc = new CC_ACRSRQ();
            cc.scp_number = ScpId;
            cc.first = AcrNo;
            cc.count = Count;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcAcrSrq, cc);
            return flag;
        }

        public bool ACRMode(short ScpId,short AcrNo,short Mode)
        {
            CC_ACRMODE cc = new CC_ACRMODE();
            cc.scp_number= ScpId;
            cc.acr_number= AcrNo;
            cc.acr_mode= Mode;
            cc.nAuthModFlags = 0;
            //cc.nExtFeatureType
            bool flag = SendCommand((Int16)enCfgCmnd.enCcAcrMode,cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Card Formatter

        public bool CardFormatterConfiguration(short scpID, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
        {
            CC_SCP_CFMT cc = new CC_SCP_CFMT();
            cc.lastModified = 0;
            cc.nScpID = scpID;
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
            bool flag = SendCommand((Int16)enCfgCmnd.enCcScpCfmt, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(scpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }


        #endregion

        #region Credentials

        public bool AccessDatabaseCardRecord(short ScpId,short Flags,long CardNumber,int IssueCode,string Pin,List<short> AccessLevel,int Active,int Deactive = 2085970000)
        {
            CC_ADBC_I64DTIC32 cc = new CC_ADBC_I64DTIC32();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.flags = Flags;
            cc.card_number = CardNumber;
            cc.issue_code = IssueCode;
            for(int i = 0;i < Pin.Length; i++)
            {
                cc.pin[i] = Pin[i];
                if(i == Pin.Length-1)
                {
                    cc.pin[i] = '\0';
                }
            }
            for(short i = 0;i < AccessLevel.Count; i++)
            {
                cc.alvl[i] = AccessLevel[i];
            }
            cc.act_time = Active;
            cc.dact_time = Deactive;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcAdbCardI64DTic32, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        #endregion

        #region Time

        public bool TimeSet(short ScpID)
        {
            CC_TIME cc_time = new CC_TIME();
            cc_time.scp_number = ScpID;
            cc_time.custom_time = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcTime, cc_time);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
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
        public bool SendASCIICommand(string command)
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

        public bool ResetSCP(short scpID)
        {
            CC_RESET cc_reset = new CC_RESET();
            cc_reset.scp_number = scpID;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcReset, cc_reset);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(scpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool GetIDReport(short ScpID)
        {
            CC_IDREQUEST cc_idrequest = new CC_IDREQUEST();
            cc_idrequest.scp_number = ScpID;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcIDRequest, cc_idrequest);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool GetWebConfig(short ScpID)
        {
            CC_WEB_CONFIG_READ cc = new CC_WEB_CONFIG_READ();
            cc.scp_number = ScpID;
            cc.read_type = 2;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcWebConfigRead, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool GetCpSrq(short scpID)
        {
            CC_CPSRQ cc_cpsrq = new CC_CPSRQ();
            cc_cpsrq.scp_number = scpID;
            cc_cpsrq.first = 0;
            cc_cpsrq.count = 1;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcCpSrq, cc_cpsrq);
            return flag;
        }

        public bool GetSioStatus(short ScpId, short SioNo)
        {
            CC_SIOSRQ cc_siosrq = new CC_SIOSRQ();
            cc_siosrq.scp_number = ScpId;
            cc_siosrq.first = SioNo;
            cc_siosrq.count = 1;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcSioSrq, cc_siosrq);
            return flag;
        }



        public bool ReadMemoryStorage(short ScpID)
        {
            CC_READ cc_read = new CC_READ();
            cc_read.nScpID = ScpID;
            cc_read.nFirst = 0;
            cc_read.nCount = 1;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcMemRead, cc_read);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool UploadScpConfig(short scpId)
        {
            throw new NotImplementedException();
        }



        #endregion












    }
}
