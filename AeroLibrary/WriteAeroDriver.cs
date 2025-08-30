using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Dto.Acr;
using HIDAeroService.Dto.Holiday;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;
using HIDAeroService.Models;
using HIDAeroService.Service;
using MiNET.Entities.Passive;
using Newtonsoft.Json.Linq;


namespace HIDAeroService.AeroLibrary
{
    public sealed class WriteAeroDriver
    {
        public int TagNo { get; private set; } = 0;
        public List<int> UploadCommandTags { get; private set; } = new List<int>();
        public string UploadMessage { get; set; } = "";
        public string Command { get; private set; } = "";

        #region Configure the driver

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
        public bool CreateChannel(short cPort,short nChannelId,short cType)
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
            bool flag = SendCommand((Int16)enCfgCmnd.enCcCreateChannel, cc_channel);
            return flag;
        }


        #endregion

        #region Configuring the intelligent controller: pre-connection

        public bool SCPDeviceSpecification(short ScpId,ArScpSetting setting)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcScpScp;
            CC_SCP_SCP cc_scp_scp = new CC_SCP_SCP();
            cc_scp_scp.lastModified = 0;
            cc_scp_scp.number = ScpId;
            cc_scp_scp.ser_num_low = 0;
            cc_scp_scp.ser_num_high = 0;
            cc_scp_scp.rev_major = 0;
            cc_scp_scp.rev_minor = 0;
            cc_scp_scp.nMsp1Port = setting.NMsp1Port;
            cc_scp_scp.nTransactions = setting.NTransaction;
            cc_scp_scp.nSio = setting.NSio;
            cc_scp_scp.nMp = setting.NMp;
            cc_scp_scp.nCp = setting.NCp;
            cc_scp_scp.nAcr = setting.NAcr;
            cc_scp_scp.nAlvl = setting.NAlvl;
            cc_scp_scp.nTrgr = setting.NTrgr;
            cc_scp_scp.nProc = setting.NProc;
            cc_scp_scp.gmt_offset = setting.GmtOffset;
            cc_scp_scp.nDstID = 0;
            cc_scp_scp.nTz = setting.NTz;
            cc_scp_scp.nHol = setting.NHol;
            cc_scp_scp.nMpg = setting.NMpg;
            cc_scp_scp.nTranLimit = 60000;
            cc_scp_scp.nAuthModType = 0;
            cc_scp_scp.nOperModes = 0;
            cc_scp_scp.oper_type = 1;
            cc_scp_scp.nLanguages = 0;
            cc_scp_scp.nSrvcType = 0;
            bool flag = SendCommand(_commandValue, cc_scp_scp);
            if(flag) {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool AccessDatabaseSpecification(short ScpID,ArScpSetting setting)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcScpAdbSpec;
            CC_SCP_ADBS cc_scp_adbs = new CC_SCP_ADBS();
            cc_scp_adbs.lastModified = 0;
            cc_scp_adbs.nScpID = ScpID;
            cc_scp_adbs.nCards = setting.NCard;
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
            bool flag = SendCommand(_commandValue, cc_scp_adbs);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpID);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        //public bool ElevatorAccessLevelSpecification(short ScpId,short MaxElvAvl,short MaxFloor)
        //{
        //    CC_ELALVLSPC cc = new CC_ELALVLSPC();
        //    cc.scp_number = ScpId;
        //    cc.max_elalvl = MaxElvAvl;
        //    cc.max_floors = MaxFloor;
        //    bool flage = SendCommand((Int16)enCfgCmnd.enCcElAlvlSpc, cc);
        //    if (flag)
        //    {
        //        TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
        //        Console.WriteLine("Command Tag : " + TagNo);
        //        //insert code to store the command tag and associated cmnd struct.
        //        //cmnd struct and tag can be deleted upon receipt of
        //        //successful command delivery notification
        //    }
        //    return flag;
        //}

        #endregion

        #region SCP

        public bool DeleteScp(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcDeleteScp;
            CC_NEWSCP c = new CC_NEWSCP();
            c.nSCPId = ScpId;
            bool flag = SendCommand(_commandValue, c);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool DetachScp(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcDetachScp;
            CC_ATTACHSCP c = new CC_ATTACHSCP();
            c.nSCPId = ScpId;
            c.nChannelId = 0;
            bool flag = SendCommand(_commandValue, c);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool ReadStructureStatus(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcStrSRq;
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
            bool flag = SendCommand(_commandValue, cc_strsq);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
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
            var _commandValue = (Int16)enCfgCmnd.enCcTranIndex;
            CC_TRANINDEX cc_tranindex = new CC_TRANINDEX();
            cc_tranindex.scp_number = scpID;
            cc_tranindex.tran_index = isEnable ? -2 : -1;
            bool flag = SendCommand(_commandValue, cc_tranindex);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(scpID);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Time Zone

        public bool ExtendedTimeZoneActSpecification(short scpId,TimeZoneDto dto, List<ArInterval> intervals,int activeTime,int deactiveTime)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcScpTimezoneExAct;
            CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
            cc.lastModified = 0;
            cc.nScpID = scpId;
            cc.number = dto.ComponentNo;
            cc.mode = dto.Mode;
            cc.actTime = activeTime;
            cc.deactTime = deactiveTime;
            cc.intervals = dto.Intervals;
            if(intervals.Count > 0)
            {
                int i = 0;
                foreach(var interval in intervals)
                {
                    cc.i[i].i_days = interval.IDays;
                    cc.i[i].i_start = interval.IStart;
                    cc.i[i].i_end = interval.IEnd;
                    i++;
                }
  
            }

            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(scpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }

            return flag;
        }

        #endregion

        #region Holiday

        public bool HolidayConfiguration(HolidayDto dto,short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcScpHoliday;
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.Year,
                month = dto.Month,
                day = dto.Day,
                //extend = dto.Extend,
                type_mask = dto.TypeMask
            };
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }

            return flag;
        }

        #endregion

        #region Access Level

        public bool AccessLevelConfigurationExtended(short ScpId, short number,short TzAcr)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcAlvlEx;
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            for (int i = 0; i < cc.tz.Length; i++)
            {
                
                cc.tz[i] = TzAcr;

            }
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool AccessLevelConfigurationExtendedCreate(short ScpId,short Number, List<CreateAccessLevelDoor> Doors)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcAlvlEx;
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = Number;
            for (int i = 0; i < cc.tz.Length; i++)
            {
                if(i < Doors.Count)
                {
                    cc.tz[Doors[i].AcrNo] = Doors[i].TzNo;
                }
                else
                {
                    cc.tz[i] = 0;
                }   

            }
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }


        #endregion

        #region SIO

        public bool SioDriverConfiguration(short ScpId, short SIODriverNo, short IOModulePort, int BaudRate, short ProtocolType)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcMsp1;
            CC_MSP1 cc_msp1 = new CC_MSP1();
            cc_msp1.lastModified = 0;
            cc_msp1.scp_number = ScpId;
            cc_msp1.msp1_number = SIODriverNo;
            cc_msp1.port_number = IOModulePort;
            cc_msp1.baud_rate = BaudRate;
            cc_msp1.reply_time = 90;
            cc_msp1.nProtocol = ProtocolType;
            cc_msp1.nDialect = 0;
            bool flag = SendCommand(_commandValue, cc_msp1);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool SioPanelConfiguration(short ScpId, short SioNo, short ModelNo, short ModuleAddress, short SIODriverPort, bool isEnable)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcSio;
            short nInput, nOutput, nReaders;
            short[] n = Utility.GetSCPComponent(ModelNo);
            nInput = n[0];
            nOutput = n[1];
            nReaders = n[2];

            CC_SIO cc_sio = new CC_SIO();
            cc_sio.lastModified = 0;
            cc_sio.scp_number = ScpId;
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
            bool flag = SendCommand(_commandValue, cc_sio);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Control Point

        public int OutputPointSpecification(short ScpId, short SioNo, short OutputNo, short OutputMode)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcOutput;
            CC_OP cc_op = new CC_OP();
            cc_op.lastModified = 0;
            cc_op.scp_number = ScpId;
            cc_op.sio_number = SioNo;
            cc_op.output = OutputNo;
            cc_op.mode = OutputMode;
            bool flag = SendCommand(_commandValue, cc_op);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return TagNo;
            }
            else
            {
                return -1;
            }
        }

        public int ControlPointConfiguration(short ScpId, short SioNo, short CpNo, short OutputNo,short DefaultPulseTime)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcCP;
            CC_CP cc_cp = new CC_CP();
            cc_cp.lastModified = 0;
            cc_cp.scp_number = ScpId;
            cc_cp.sio_number = SioNo;
            cc_cp.cp_number = CpNo;
            cc_cp.op_number = OutputNo;
            cc_cp.dflt_pulse = DefaultPulseTime;
            bool flag = SendCommand(_commandValue, cc_cp);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return TagNo;
            }
            else
            {
                return -1;
            }
            
        }

        public bool ControlPointCommand(short ScpId, short c, short command)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcCpCtl;
            CC_CPCTL cc_cpctl = new CC_CPCTL();
            cc_cpctl.scp_number = ScpId;
            cc_cpctl.cp_number = c;
            cc_cpctl.command = command;
            cc_cpctl.on_time = 0;
            cc_cpctl.off_time = 0;
            cc_cpctl.repeat = 0;
            bool flag = SendCommand(_commandValue, cc_cpctl);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
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
            bool flag = SendCommand((Int16)enCfgCmnd.enCcCpSrq, cc);
            //if (flag)
            //{
            //    TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
            //    Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("Command Tag : " + TagNo);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }

        #endregion

        #region Monitor Point

        public bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode,short Debounce,short HoldTime)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcInput;
            CC_IP cc_ip = new CC_IP();
            cc_ip.lastModified = 0;
            cc_ip.scp_number = ScpId;
            cc_ip.sio_number = SioNo;
            cc_ip.input = InputNo;
            cc_ip.icvt_num = InputMode;
            cc_ip.debounce = Debounce;
            cc_ip.hold_time = HoldTime;
            bool flag = SendCommand(_commandValue, cc_ip);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool MonitorPointConfiguration(short ScpId, short SioNo, short InputNo,short LfCode,short Mode,short DelayEntry,short DelayExit,short nMp)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcMP;
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
            bool flag = SendCommand(_commandValue, cc_mp);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool GetMpStatus(short ScpId, short MpNo, short Count)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcMpSrq;
            CC_MPSRQ cc = new CC_MPSRQ();
            cc.scp_number = ScpId;
            cc.first = MpNo;
            cc.count = Count;
            bool flag = SendCommand(_commandValue, cc);
            //if (flag)
            //{
            //    TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
            //    Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("Command Tag : " + TagNo);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }


        #endregion

        #region Reader

        public bool ReaderSpecification(short ScpId, short SioNo, short ReaderNo,short DataFormat,short KeyPadMode,short LedDriveMode,short OsdpFlag)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcReader;
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
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Access Control Reader (ACR)

        public bool AccessControlReaderConfigurationForCreate(short ScpId,short AcrNo,AddAcrDto dto)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcACR;
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
            bool flag = SendCommand(_commandValue, cc_acr);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        public bool MomentaryUnlock(short ScpId, short AcrNo)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcUnlock;
            CC_UNLOCK cc_unlock = new CC_UNLOCK();
            cc_unlock.scp_number = ScpId;
            cc_unlock.acr_number = AcrNo;
            bool flag = SendCommand(_commandValue, cc_unlock);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        public bool GetAcrStatus(short ScpId, short AcrNo, short Count)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcAcrSrq;
            CC_ACRSRQ cc = new CC_ACRSRQ();
            cc.scp_number = ScpId;
            cc.first = AcrNo;
            cc.count = Count;
            bool flag = SendCommand(_commandValue, cc);
            //if (flag)
            //{
            //    TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
            //    Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("Command Tag : " + TagNo);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }

        public bool ACRMode(short ScpId,short AcrNo,short Mode)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcAcrMode;
            CC_ACRMODE cc = new CC_ACRMODE();
            cc.scp_number= ScpId;
            cc.acr_number= AcrNo;
            cc.acr_mode= Mode;
            cc.nAuthModFlags = 0;
            //cc.nExtFeatureType
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        #endregion

        #region Card Formatter

        public bool CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcScpCfmt;
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
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
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
            var _commandValue = (Int16)enCfgCmnd.enCcAdbCardI64DTic32;
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
            cc.act_time = (int)Active;
            cc.dact_time = (int)Deactive;
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        public bool CardDelete(short ScpId,long CardNo)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcCardDeleteI64;
            CC_CARDDELETEI64 cc = new CC_CARDDELETEI64();
            cc.scp_number = ScpId;
            cc.cardholder_id = CardNo;
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }


        #endregion

        #region Time

        public bool TimeSet(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcTime;
            CC_TIME cc_time = new CC_TIME();
            cc_time.scp_number = ScpId;
            cc_time.custom_time = 0;
            bool flag = SendCommand((Int16)enCfgCmnd.enCcTime, cc_time);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
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

        public bool ResetSCP(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcReset;
            CC_RESET cc_reset = new CC_RESET();
            cc_reset.scp_number = ScpId;
            bool flag = SendCommand(_commandValue, cc_reset);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public bool GetIDReport(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcIDRequest;
            CC_IDREQUEST cc_idrequest = new CC_IDREQUEST();
            cc_idrequest.scp_number = ScpId;
            bool flag = SendCommand(_commandValue, cc_idrequest);
            //if (flag)
            //{
            //    TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
            //    Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("Command Tag : " + TagNo);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }

        public bool GetWebConfig(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcWebConfigRead;
            CC_WEB_CONFIG_READ cc = new CC_WEB_CONFIG_READ();
            cc.scp_number = ScpId;
            cc.read_type = 2;
            bool flag = SendCommand(_commandValue, cc);
            //if (flag)
            //{
            //    TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
            //    Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("Command Tag : " + TagNo);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }

        public bool GetCpSrq(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcCpSrq;
            CC_CPSRQ cc_cpsrq = new CC_CPSRQ();
            cc_cpsrq.scp_number = ScpId;
            cc_cpsrq.first = 0;
            cc_cpsrq.count = 1;
            bool flag = SendCommand(_commandValue, cc_cpsrq);
            //if (flag)
            //{
            //    TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
            //    Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("Command Tag : " + TagNo);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }

        public bool GetSioStatus(short ScpId, short SioNo)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcSioSrq;
            CC_SIOSRQ cc_siosrq = new CC_SIOSRQ();
            cc_siosrq.scp_number = ScpId;
            cc_siosrq.first = SioNo;
            cc_siosrq.count = 1;
            bool flag = SendCommand(_commandValue, cc_siosrq);
            //if (flag)
            //{
            //    TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
            //    Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
            //    Console.WriteLine("Command Tag : " + TagNo);
            //    //insert code to store the command tag and associated cmnd struct.
            //    //cmnd struct and tag can be deleted upon receipt of
            //    //successful command delivery notification
            //}
            return flag;
        }



        public bool ReadMemoryStorage(short ScpId)
        {
            var _commandValue = (Int16)enCfgCmnd.enCcMemRead;
            CC_READ cc_read = new CC_READ();
            cc_read.nScpID = ScpId;
            cc_read.nFirst = 0;
            cc_read.nCount = 1;
            bool flag = SendCommand(_commandValue, cc_read);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Command = Enum.GetName(typeof(enCfgCmnd), _commandValue) ?? "";
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        //public bool UploadScpConfig(short scpId)
        //{
            
        //}



        #endregion



    }
}
