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
using HIDAeroService.Service;
using HIDAeroService.Utility;
using LibNoise.Modifier;
using MiNET.Entities.Passive;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using static MiNET.Net.McpeUpdateBlock;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HIDAeroService.AeroLibrary
{
    public sealed class AeroCommand
    {
        private readonly ConcurrentDictionary<int, TaskCompletionSource<bool>> _pendingCommands = new ConcurrentDictionary<int, TaskCompletionSource<bool>>();
        public int TagNo { get; private set; } = 0;
        public List<int> UploadCommandTags { get; private set; } = new List<int>();
        public string Command { get; private set; } = "";
        public int CommandTimeOut
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            } 
        }

        private int _commandTimeout = 10;

        



        private Task<bool> SendCommandAsync(int tag,int timeout)
        { 
            TimeSpan span = TimeSpan.FromSeconds(timeout);
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!_pendingCommands.TryAdd(tag, tcs))
                throw new InvalidOperationException($"Command with tag {tag} already exists");

            // Timeout
            var cts = new CancellationTokenSource(span);
            cts.Token.Register(() => {
                if (_pendingCommands.TryRemove(tag, out var source))
                    source.TrySetException(new TimeoutException("Command time out"));
            });

            return tcs.Task;

        }

        public void CommandResultTrigger(int tag,bool result)
        {
            if(_pendingCommands.TryRemove(tag,out var tcs))
            {
                tcs.TrySetResult(result);
            }
        }


        #region Configure the driver

        //////
        // Method: SendCommand to Driver
        // Target: For Setting Base Driver Specification
        // port: Port number of driver that Controller need to set
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
        // channelId: Channel ComponentId to create
        // commuType: Communication TypeDesc ip-client / ip-server
        // port: Port number of driver that Controller need to set
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

        public async Task<bool> SCPDeviceSpecification(short ScpId, SystemSetting setting)
        {
            var _commandValue = (short)enCfgCmnd.enCcScpScp;
            CC_SCP_SCP cc_scp_scp = new CC_SCP_SCP();
            cc_scp_scp.lastModified = 0;
            cc_scp_scp.number = ScpId;
            cc_scp_scp.ser_num_low = 0;
            cc_scp_scp.ser_num_high = 0;
            cc_scp_scp.rev_major = 0;
            cc_scp_scp.rev_minor = 0;
            cc_scp_scp.nMsp1Port = setting.nMsp1Port;
            cc_scp_scp.nTransactions = setting.nTransaction;
            cc_scp_scp.nSio = setting.nSio;
            cc_scp_scp.nMp = setting.nMp;
            cc_scp_scp.nCp = setting.nCp;
            cc_scp_scp.nAcr = setting.nAcr;
            cc_scp_scp.nAlvl = setting.nAlvl;
            cc_scp_scp.nTrgr = setting.nTrgr;
            cc_scp_scp.nProc = setting.nProc;
            cc_scp_scp.gmt_offset = setting.GmtOffset;
            cc_scp_scp.nDstID = 0;
            cc_scp_scp.nTz = setting.nTz;
            cc_scp_scp.nHol = setting.nHol;
            cc_scp_scp.nMpg = setting.nMpg;
            cc_scp_scp.nTranLimit = 60000;
            cc_scp_scp.nAuthModType = 0;
            cc_scp_scp.nOperModes = 0;
            cc_scp_scp.oper_type = 1;
            cc_scp_scp.nLanguages = 0;
            cc_scp_scp.nSrvcType = 0;
            bool flag = SendCommand(_commandValue, cc_scp_scp);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return flag;
        }


        public async Task<bool> AccessDatabaseSpecificationAsync(short ScpID, SystemSetting setting)
        {
            CC_SCP_ADBS cc_scp_adbs = new CC_SCP_ADBS();
            cc_scp_adbs.lastModified = 0;
            cc_scp_adbs.nScpID = ScpID;
            cc_scp_adbs.nCards = setting.nCard;
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
            bool flag = SendCommand((short)enCfgCmnd.enCcScpAdbSpec, cc_scp_adbs);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpID), _commandTimeout);
            }
            return false;
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

        public async Task<bool> DeleteScpAsync(short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcDeleteScp;
            CC_NEWSCP c = new CC_NEWSCP();
            c.nSCPId = ScpId;
            bool flag = SendCommand(_commandValue, c);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                Console.WriteLine("DeleteScpAsync");
                Console.WriteLine(TagNo);
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandValue);
            }
            else
            {
                return false;
            }
       
        }

        public async Task<bool> DetachScpAsync(short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcDetachScp;
            CC_ATTACHSCP c = new CC_ATTACHSCP();
            c.nSCPId = ScpId;
            c.nChannelId = 0;
            bool flag = SendCommand(_commandValue, c);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                Console.WriteLine("DetachScpAsync");
                Console.WriteLine(TagNo);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ReadStructureStatusAsync(short ScpId)
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
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return false;
        }

        public bool SetScpId(short oldId,short newId)
        {
            CC_SCPID cc = new CC_SCPID();
            cc.scp_number = oldId;
            cc.scp_id = newId;
            bool flag = SendCommand((short)enCfgCmnd.enCcScpID, cc);
            return flag;
        }



        #endregion

        #region Transaction

        public async Task<bool> SetTransactionLogIndexAsync(short scpID, bool isEnable)
        {
            var _commandValue = (short)enCfgCmnd.enCcTranIndex;
            CC_TRANINDEX cc_tranindex = new CC_TRANINDEX();
            cc_tranindex.scp_number = scpID;
            cc_tranindex.tran_index = isEnable ? -2 : -1;
            bool flag = SendCommand(_commandValue, cc_tranindex);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(scpID);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(scpID), _commandTimeout);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Time Zone

        public async Task<bool> ExtendedTimeZoneActSpecificationAsync(short scpId, Entity.TimeZone dto,List<Interval> intervals, int activeTime, int deactiveTime)
        {
            CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
            cc.lastModified = 0;
            cc.nScpID = scpId;
            cc.number = dto.ComponentId;
            cc.mode = dto.Mode;
            cc.actTime = activeTime;
            cc.deactTime = deactiveTime;
            cc.intervals = (short)intervals.Count;
            if (intervals.Count > 0)
            {
                int i = 0;
                foreach (var interval in intervals)
                {
                    cc.i[i].i_days = (short)ConvertDayToBinary(interval.Days);
                    cc.i[i].i_start = (short)ConvertTimeToEndMinute(interval.StartTime);
                    cc.i[i].i_end =  (short)ConvertTimeToEndMinute(interval.StartTime);
                    i++;
                }

            }

            bool flag = SendCommand((short)enCfgCmnd.enCcScpTimezoneExAct, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(scpId),_commandTimeout);
            }

            return false;
        }

        private string DaysInWeekToString(DaysInWeekDto days)
        {
            var map = new Dictionary<string, bool>{
                {"Sun",days.Sunday },
                {
                    "Mon",days.Monday
                },
                {
                    "Tue",days.Tuesday
                },
                {
                    "Wed",days.Wednesday
                },
                {
                    "Thu",days.Thursday
                },
                {
                    "Fri",days.Friday
                },
                {
                    "Sat",days.Saturday
                }
            };

            return string.Join(",", map.Where(x => x.Value).Select(x => x.Key));
        }

        private DaysInWeekDto StringToDaysInWeek(string daysString)
        {
            var dto = new DaysInWeekDto();
            if (string.IsNullOrWhiteSpace(daysString)) return dto;

            var parts = daysString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(p => p.Trim());

            foreach (var day in parts)
            {
                switch (day)
                {
                    case "Sun": dto.Sunday = true; break;
                    case "Mon": dto.Monday = true; break;
                    case "Tue": dto.Tuesday = true; break;
                    case "Wed": dto.Wednesday = true; break;
                    case "Thu": dto.Thursday = true; break;
                    case "Fri": dto.Friday = true; break;
                    case "Sat": dto.Saturday = true; break;
                }
            }

            return dto;
        }

        private int ConvertDayToBinary(DaysInWeek days)
        {
            int result = 0;
            result |= (days.Sunday ? 1 : 0) << 0;
            result |= (days.Monday ? 1 : 0) << 1;
            result |= (days.Tuesday ? 1 : 0) << 2;
            result |= (days.Wednesday ? 1 : 0) << 3;
            result |= (days.Thursday ? 1 : 0) << 4;
            result |= (days.Friday ? 1 : 0) << 5;
            result |= (days.Saturday ? 1 : 0) << 6;

            // Holiday
            //result |= 0 << 8;
            //result |= 0 << 9;
            //result |= 0 << 10;
            //result |= 0 << 11;
            //result |= 0 << 12;
            //result |= 0 << 13;
            //result |= 0 << 14;
            //result |= 0 << 15;
            return result;
        }

        private int ConvertTimeToEndMinute(string timeString)
        {
            // Parse "HH:mm"
            var time = TimeSpan.Parse(timeString);

            // Convert hours/minutes to minutes since 12:00 AM
            int startMinutes = time.Hours * 60 + time.Minutes;

            // Return the minute number at the *end* of this minute
            return startMinutes;
        }

        #endregion

        #region Holiday

        public async Task<bool> HolidayConfigurationAsync(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.Year,
                month = dto.Month,
                day = dto.Day,
                //extend = dto.Extend,
                type_mask = 1
            };
            bool flag = SendCommand((short)enCfgCmnd.enCcScpHoliday, cc);
            if (flag)
            {
                return await  SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }

            return false;
        }

        public async Task<bool> DeleteHolidayConfigurationAsync(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.Year,
                month = dto.Month,
                day = dto.Day,
                //extend = dto.Extend,
                type_mask = 0
            };
            bool flag = SendCommand((short)enCfgCmnd.enCcScpHoliday, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }

            return false;
        }


        public async Task<bool> ClearHolidayConfigurationAsync( short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcScpHoliday;
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = 0,
                month = 1,
                day = 1,
                //extend = dto.Extend,
                type_mask = 0
            };
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }

            return false;
        }
        #endregion

        #region Access Level

        public async Task<bool> AccessLevelConfigurationExtendedAsync(short ScpId, short number, short TzAcr)
        {
            var _commandValue = (short)enCfgCmnd.enCcAlvlEx;
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
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;
        }


        public async Task<bool> AccessLevelConfigurationExtendedCreateAsync(short ScpId, short Number, List<CreateUpdateAccessLevelDoorTimeZoneDto> AccessLevelDoorTimeZoneDto)
        {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = Number;
            foreach(var d in AccessLevelDoorTimeZoneDto)
            {
                cc.tz[d.DoorId] = d.TimeZoneId;
            }
            bool flag = SendCommand((short)enCfgCmnd.enCcAlvlEx, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return false;
        }


        #endregion

        #region SIO

        public bool SioDriverConfiguration(short ScpId, short SIODriverNo, short IOModulePort, int BaudRate, short ProtocolType)
        {
            var _commandValue = (short)enCfgCmnd.enCcMsp1;
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
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public async Task<bool> SioDriverConfigurationAsync(short ScpId, short SIODriverNo, short IOModulePort, int BaudRate, short ProtocolType)
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
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;
        }


        public bool SioPanelConfiguration(short ScpId, short SioNo, short ModelNo, short ModuleAddress, short SIODriverPort, bool isEnable)
        {
            var _commandValue = (short)enCfgCmnd.enCcSio;
            short nInput, nOutput, nReaders;
            short[] n = UtilityHelper.GetSCPComponent(ModelNo);
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
            cc_sio.flags = 0x20;
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
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;
        }

        public async Task<bool> SioPanelConfigurationAsync(short ScpId, short SioNo, short Model,short nInput,short nOutput,short nReader, short ModuleAddress, short SIODriverPort, bool isEnable)
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
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId) ,_commandTimeout);
            }
            else
            {
                return false;
            }
        }


        #endregion

        #region Control Point

        public int OutputPointSpecification(short ScpId, short SioNo, short OutputNo, short OutputMode)
        {
            var _commandValue = (short)enCfgCmnd.enCcOutput;
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

        public async Task<bool> OutputPointSpecificationAsync(short ScpId, short SioNo, short OutputNo, short OutputMode)
        {
            CC_OP cc_op = new CC_OP();
            cc_op.lastModified = 0;
            cc_op.scp_number = ScpId;
            cc_op.sio_number = SioNo;
            cc_op.output = OutputNo;
            cc_op.mode = OutputMode;
            bool flag = SendCommand((short)enCfgCmnd.enCcOutput, cc_op);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> ControlPointConfigurationAsync(short ScpId, short SioNo, short CpNo, short OutputNo, short DefaultPulseTime)
        {
            CC_CP cc_cp = new CC_CP();
            cc_cp.lastModified = 0;
            cc_cp.scp_number = ScpId;
            cc_cp.sio_number = SioNo;
            cc_cp.cp_number = CpNo;
            cc_cp.op_number = OutputNo;
            cc_cp.dflt_pulse = DefaultPulseTime;
            bool flag = SendCommand((short)enCfgCmnd.enCcCP, cc_cp);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            else
            {
                return false;
            }

        }

        public int ControlPointCommand(short ScpId, short cpNo, short command)
        {
            var _commandValue = (short)enCfgCmnd.enCcCpCtl;
            CC_CPCTL cc_cpctl = new CC_CPCTL();
            cc_cpctl.scp_number = ScpId;
            cc_cpctl.cp_number = cpNo;
            cc_cpctl.command = command;
            cc_cpctl.on_time = 0;
            cc_cpctl.off_time = 0;
            cc_cpctl.repeat = 0;
            bool flag = SendCommand(_commandValue, cc_cpctl);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return SCPDLL.scpGetTagLastPosted(ScpId);
            }
            else
            {
                return -1;
            }
        }

        public async Task<bool> ControlPointCommandAsync(short ScpId, short cpNo, short command)
        {
            CC_CPCTL cc_cpctl = new CC_CPCTL();
            cc_cpctl.scp_number = ScpId;
            cc_cpctl.cp_number = cpNo;
            cc_cpctl.command = command;
            cc_cpctl.on_time = 0;
            cc_cpctl.off_time = 0;
            cc_cpctl.repeat = 0;
            bool flag = SendCommand((short)enCfgCmnd.enCcCpCtl, cc_cpctl);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> GetCpStatus(short ScpId, short CpNo, short Count)
        {
            CC_CPSRQ cc = new CC_CPSRQ();
            cc.scp_number = ScpId;
            cc.first = (short)CpNo;
            cc.count = Count;
            bool flag = SendCommand((short)enCfgCmnd.enCcCpSrq, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Monitor Point


        public async Task<bool> InputPointSpecificationAsync(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime)
        {
            var _commandValue = (short)enCfgCmnd.enCcInput;
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
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId) ,_commandTimeout);
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> MonitorPointConfigurationAsync(short ScpId, short SioNo, short InputNo, short LfCode, short Mode, short DelayEntry, short DelayExit, short nMp)
        {
            var _commandValue = (short)enCfgCmnd.enCcMP;
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
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> MonitorPointMaskAsync(short ScpId,short MpNo,int SetClear)
        {
            var _commandValue = (short)enCfgCmnd.enCcMpMask;
            CC_MPMASK cc = new CC_MPMASK();
            cc.scp_number = ScpId;
            cc.mp_number = MpNo;
            cc.set_clear = (short)SetClear;
             bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetMpStatusAsync(short ScpId, short MpNo, short Count)
        {
            var _commandValue = (short)enCfgCmnd.enCcMpSrq;
            CC_MPSRQ cc = new CC_MPSRQ();
            cc.scp_number = ScpId;
            cc.first = MpNo;
            cc.count = Count;
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> MonitorPointGroup(short ScpId,short MpgNo,MonitorGroupListDto list)
        {
            CC_MPG c = new CC_MPG();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.mpg_number = MpgNo;
            //c.nMpList
            bool flag = SendCommand((short)enCfgCmnd.enCcMpg,c);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return false;
        }


        #endregion

        #region Monitor Point Group

        public async Task<bool> ConfigureMonitorPointGroup(short ScpId,short ComponentId,short nMonitor,List<MonitorGroupList> list)
        {
            CC_MPG c = new CC_MPG();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.mpg_number = ComponentId;
            c.nMpCount = nMonitor;
            int i = 0;
            foreach(var l in list)
            {
                c.nMpList[i] = l.PointType;
                i += 1;
                c.nMpList[i] = l.PointNumber;
            }
            bool flag = SendCommand((short)enCfgCmnd.enCcMpg, c);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;
        }

        public async Task<bool> MonitorPointGroupArmDisarmAsync(short ScpId,short ComponentId,short Command,short Arg1)
        {
            CC_MPGSET c = new CC_MPGSET();
            c.scp_number = ScpId;
            c.mpg_number = ComponentId;
            c.command = Command;
            c.arg1 = Arg1;
            bool flag = SendCommand((short)enCfgCmnd.enCcMpgSet, c);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return false;
        }

        #endregion

        #region Reader

        public async Task<bool> ReaderSpecificationAsync(short ScpId, short SioNo, short ReaderNo, short DataFormat, short KeyPadMode, short LedDriveMode, short OsdpFlag)
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
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;
        }

        #endregion

        #region Access Control Reader (ACR)

        public async Task<bool> AccessControlReaderConfigurationAsync(short ScpId, short AcrNo, Door dto)
        {
            CC_ACR cc_acr = new CC_ACR();
            cc_acr.lastModified = 0;
            cc_acr.scp_number = ScpId;
            cc_acr.acr_number = AcrNo;
            cc_acr.access_cfg = dto.AccessConfig;
            cc_acr.pair_acr_number = dto.PairDoorNo;
            cc_acr.rdr_sio = dto.Readers.ElementAt(0).ModuleId;
            cc_acr.rdr_number = dto.Readers.ElementAt(0).ModuleId;
            cc_acr.strk_sio = dto.Strk.ModuleId;
            cc_acr.strk_number = dto.Strk.OutputNo;
            cc_acr.strike_t_min = dto.Strk.StrkMin;
            cc_acr.strike_t_max = dto.Strk.StrkMax;
            cc_acr.strike_mode = dto.Strk.StrkMode;
            cc_acr.door_sio = dto.Sensor.ModuleId;
            cc_acr.door_number = dto.Sensor.InputNo;
            cc_acr.dc_held = dto.Sensor.DcHeld;
            if(dto.RequestExits is not null)
            {
                cc_acr.rex0_sio = dto.RequestExits.ElementAt(0).ModuleId;
                cc_acr.rex0_number = dto.RequestExits.ElementAt(0).InputNo;
                cc_acr.rex_tzmask[0] = dto.RequestExits.ElementAt(0).MaskTimeZone;
                if(dto.RequestExits.Count > 1)
                {
                    cc_acr.rex1_sio = dto.RequestExits.ElementAt(1).ModuleId;
                    cc_acr.rex1_number = dto.RequestExits.ElementAt(1).InputNo;
                    cc_acr.rex_tzmask[1] = dto.RequestExits.ElementAt(1).MaskTimeZone;
                }
            }
            if(dto.Readers.Count > 1)
            {
                cc_acr.altrdr_sio = dto.Readers.ElementAt(1).ModuleId;
                cc_acr.altrdr_number = dto.Readers.ElementAt(1).ReaderNo;
                cc_acr.altrdr_spec = dto.ReaderOutConfiguration;
            }
            cc_acr.cd_format = dto.CardFormat;
            cc_acr.apb_mode = dto.AntiPassbackMode;
            cc_acr.apb_in = dto.AntiPassBackIn;
            cc_acr.apb_to = dto.AntiPassBackOut;
            if(dto.SpareTags != -1) cc_acr.spare = dto.SpareTags;
            if(dto.AccessControlFlags != -1) cc_acr.actl_flags = dto.AccessControlFlags;
            cc_acr.offline_mode = dto.OfflineMode;
            cc_acr.default_mode = dto.DefaultMode;
            cc_acr.default_led_mode = dto.DefaultLEDMode;
            cc_acr.pre_alarm = dto.PreAlarm;
            cc_acr.apb_delay = dto.AntiPassbackDelay;
            cc_acr.strk_t2 = dto.StrkT2;
            cc_acr.dc_held2 = dto.DcHeld2;
            cc_acr.strk_follow_pulse = 0;
            cc_acr.strk_follow_delay = 0;
            cc_acr.nAuthModFlags = 0;
            cc_acr.nExtFeatureType = 0;
            cc_acr.dfofFilterTime = 0;
            bool flag = SendCommand((short)enCfgCmnd.enCcACR, cc_acr);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;

        }

     
        public async Task<bool> MomentaryUnlockAsync(short ScpId, short AcrNo)
        {
            CC_UNLOCK cc_unlock = new CC_UNLOCK();
            cc_unlock.scp_number = ScpId;
            cc_unlock.acr_number = AcrNo;
            bool flag = SendCommand((short)enCfgCmnd.enCcUnlock, cc_unlock);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;

        }

        public async Task<bool> GetAcrStatusAsync(short ScpId, short AcrNo, short Count)
        {
            var _commandValue = (short)enCfgCmnd.enCcAcrSrq;
            CC_ACRSRQ cc = new CC_ACRSRQ();
            cc.scp_number = ScpId;
            cc.first = AcrNo;
            cc.count = Count;
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return false;
        }

        public async Task<bool> ACRModeAsync(short ScpId, short AcrNo, short Mode)
        {
            var _commandValue = (short)enCfgCmnd.enCcAcrMode;
            CC_ACRMODE cc = new CC_ACRMODE();
            cc.scp_number = ScpId;
            cc.acr_number = AcrNo;
            cc.acr_mode = Mode;
            cc.nAuthModFlags = 0;
            //cc.nExtFeatureType
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return flag;
        }

        #endregion

        #region Access Area

        public async Task<bool> ConfigureAccessAreaAsync(short ScpId, short AreaNo, short MultiOccu,short AccControl,short OccControl,short OccSet,short OccMax,short OccUp,short OccDown,short AreaFlag)
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
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;
        }

        public async Task<bool> GetAccessAreaStatusAsync(short ScpId,short ComponentId,short Number) 
        {
            CC_AREASRQ cc = new CC_AREASRQ();
            cc.scp_number = ScpId;
            cc.first = ComponentId;
            cc.count = Number;
            bool flag = SendCommand((short)enCfgCmnd.enCcAreaSrq, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;
        }

        #endregion

        #region Card Formatter

        public bool CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
        {
            var _commandValue = (short)enCfgCmnd.enCcScpCfmt;
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
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return flag;

        }

        public async Task<bool> CardFormatterConfigurationAsync(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
        {
            var _commandValue = (short)enCfgCmnd.enCcScpCfmt;
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
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            else
            {
                return false;
            }
            

        }

        public async Task<bool> CardFormatterConfigurationAsync(short ScpId, CardFormatDto dto, short funtionId)
        {
            var _commandValue = (short)enCfgCmnd.enCcScpCfmt;
            CC_SCP_CFMT cc = new CC_SCP_CFMT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = (short)dto.ComponentId;
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
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;

        }


        #endregion

        #region Credentials

        public async Task<bool> AccessDatabaseCardRecordAsync(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<AccessLevel> AccessLevel, int Active, int Deactive = 2085970000)
        {
            var _commandValue = (short)enCfgCmnd.enCcAdbCardI64DTic32;
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
            for(int i = 0;i < AccessLevel.Count; i++)
            {
                cc.alvl[i] = AccessLevel[i].ComponentId;
            }
            cc.act_time = Active;
            cc.dact_time = Deactive;
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;

        }

        public async Task<bool> CardDeleteAsync(short ScpId, long CardNo)
        {
            var _commandValue = (short)enCfgCmnd.enCcCardDeleteI64;
            CC_CARDDELETEI64 cc = new CC_CARDDELETEI64();
            cc.scp_number = ScpId;
            cc.cardholder_id = CardNo;
            bool flag = SendCommand(_commandValue, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return false;
        }


        #endregion

        #region Time


        public async Task<bool> TimeSetAsync(short ScpId)
        {
            CC_TIME cc_time = new CC_TIME();
            cc_time.scp_number = ScpId;
            cc_time.custom_time = 0;
            bool flag = SendCommand((short)enCfgCmnd.enCcTime, cc_time);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            else
            {
                return false;
            }
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

        public async Task<bool> ResetSCP(short ScpId)
        {
            CC_RESET cc_reset = new CC_RESET();
            cc_reset.scp_number = ScpId;
            bool flag = SendCommand((short)enCfgCmnd.enCcReset, cc_reset);
            //if (flag)
            //{
            //    return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            //}
            //return false;
            return flag;
        }

        public async Task<bool> GetIdReportAsync(short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcIDRequest;
            CC_IDREQUEST cc_idrequest = new CC_IDREQUEST();
            cc_idrequest.scp_number = ScpId;
            bool flag = SendCommand(_commandValue, cc_idrequest);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            return flag;
        }

        public bool GetWebConfig(short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcWebConfigRead;
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
            var _commandValue = (short)enCfgCmnd.enCcCpSrq;
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
            var _commandValue = (short)enCfgCmnd.enCcSioSrq;
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

        public async Task<bool> GetSioStatusAsync(short ScpId, short SioNo)
        {
            var _commandValue = (short)enCfgCmnd.enCcSioSrq;
            CC_SIOSRQ cc_siosrq = new CC_SIOSRQ();
            cc_siosrq.scp_number = ScpId;
            cc_siosrq.first = SioNo;
            cc_siosrq.count = 1;
            bool flag = SendCommand(_commandValue, cc_siosrq);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId),_commandTimeout);
            }
            else
            {
                return false;
            }
        }



        public bool ReadMemoryStorage(short ScpId)
        {
            var _commandValue = (short)enCfgCmnd.enCcMemRead;
            CC_READ cc_read = new CC_READ();
            cc_read.nScpID = ScpId;
            cc_read.nFirst = 0;
            cc_read.nCount = 1;
            bool flag = SendCommand(_commandValue, cc_read);
            if (flag)
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId);
                Console.WriteLine("Command Tag : " + TagNo);
                //insert code to store the command tag and associated cmnd struct.
                //cmnd struct and tag can be deleted upon receipt of
                //successful command delivery notification
            }
            return false;
        }

        //public bool UploadScpConfig(short scpId)
        //{

        //}



        #endregion

        #region Trigger

        public async Task<bool> TriggerSpecification(short scpId, Trigger dto,short ComponentId)
        {
            CC_TRGR cc = new CC_TRGR();
            cc.scp_number = scpId;
            cc.trgr_number = ComponentId;
            cc.command = dto.Command;
            cc.proc_num = dto.ProcedureId;
            cc.src_type = dto.SourceType;
            cc.src_number = dto.SourceNumber;
            cc.tran_type = dto.TranType;
            cc.code_map = dto.CodeMap;
            cc.timezone = dto.TimeZone;
            switch (dto.TranType)
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

            // Trigger Variable
            //cc.trig_var[0] = 0;

            // Transaction TypeDesc Arg
            //cc.arg[0] = 0;

            

            bool flag = SendCommand((short)enCfgCmnd.enCcTrgr, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(scpId), _commandTimeout);
            }

            return false;
        }

        #endregion

        #region Procedure

        public async Task<bool> ActionSpecificationAsyncForAllHW(short ComponentId,Entity.Action action,List<short> ScpIds)
        {
            foreach(var id in ScpIds)
            {
                CC_ACTN c = new CC_ACTN(action.ActionType);
                c.hdr.lastModified = 0;
                c.hdr.scp_number = id;
                c.hdr.proc_number = ComponentId;
                c.hdr.action_type = action.ActionType;

                switch (action.ActionType)
                {
                    case 1:
                        c.mp_mask.scp_number = id;
                        c.mp_mask.mp_number = action.Arg1;
                        c.mp_mask.set_clear = action.Arg2;
                        break;
                    case 2:
                        c.cp_ctl.scp_number = id;
                        c.cp_ctl.cp_number = action.Arg1;
                        c.cp_ctl.command = action.Arg2;
                        c.cp_ctl.on_time = action.Arg3;
                        c.cp_ctl.off_time = action.Arg4;
                        c.cp_ctl.repeat = action.Arg5;
                        break;
                    case 3:
                        c.acr_mode.scp_number = id;
                        c.acr_mode.acr_number = action.Arg1;
                        c.acr_mode.acr_mode = action.Arg2;
                        break;
                    case 4:
                        c.fo_mask.scp_number = id;
                        c.fo_mask.acr_number = action.Arg1;
                        c.fo_mask.set_clear = action.Arg2;
                        break;
                    case 5:
                        c.ho_mask.scp_number = id;
                        c.ho_mask.acr_number = action.Arg1;
                        c.ho_mask.set_clear = action.Arg2;
                        break;
                    case 6:
                        c.unlock.scp_number = id;
                        c.unlock.acr_number = action.Arg1;
                        c.unlock.floor_number = action.Arg2;
                        c.unlock.strk_tm = action.Arg3;
                        c.unlock.t_held = action.Arg4;
                        c.unlock.t_held_pre = action.Arg5;
                        break;
                    case 7:
                        c.proc.scp_number = id;
                        c.proc.proc_number = action.Arg1;
                        c.proc.command = action.Arg2;
                        break;
                    case 8:
                        c.tv_ctl.scp_number = id;
                        c.tv_ctl.tv_number = action.Arg1;
                        c.tv_ctl.set_clear = action.Arg2;
                        break;
                    case 9:
                        c.tz_ctl.scp_number = id;
                        c.tz_ctl.tz_number = action.Arg1;
                        c.tz_ctl.command = action.Arg2;
                        break;
                    case 10:
                        c.led_mode.scp_number = id;
                        c.led_mode.acr_number = action.Arg1;
                        c.led_mode.led_mode = action.Arg2;
                        break;
                    case 14:
                        c.mpg_set.scp_number = id;
                        c.mpg_set.mpg_number = action.Arg1;
                        c.mpg_set.command = action.Arg2;
                        c.mpg_set.arg1 = action.Arg3;
                        break;
                    case 15:
                        c.mpg_test_mask.scp_number = id;
                        c.mpg_test_mask.mpg_number = action.Arg1;
                        c.mpg_test_mask.action_prefix_ifz = action.Arg2;
                        c.mpg_test_mask.action_prefix_ifnz = action.Arg3;
                        break;
                    case 16:
                        c.mpg_test_active.scp_number = id;
                        c.mpg_test_active.mpg_number = action.Arg1;
                        c.mpg_test_active.action_prefix_ifnoactive = action.Arg2;
                        c.mpg_test_active.action_prefix_ifactive = action.Arg3;
                        break;
                    case 17:
                        c.area_set.scp_number = id;
                        c.area_set.area_number = action.Arg1;
                        c.area_set.command = action.Arg2;
                        c.area_set.occ_set = action.Arg3;
                        break;
                    case 18:
                        c.unlock.scp_number = id;
                        c.unlock.acr_number = action.Arg1;
                        c.unlock.floor_number = action.Arg2;
                        c.unlock.strk_tm = action.Arg3;
                        c.unlock.t_held = action.Arg4;
                        c.unlock.t_held_pre = action.Arg5;
                        break;
                    case 19:
                        c.rled_tmp.scp_number = id;
                        c.rled_tmp.acr_number = action.Arg1;
                        c.rled_tmp.color_on = action.Arg2;
                        c.rled_tmp.color_off = action.Arg3;
                        c.rled_tmp.ticks_on = action.Arg4;
                        c.rled_tmp.ticks_off = action.Arg5;
                        c.rled_tmp.repeat = action.Arg6;
                        c.rled_tmp.beeps = action.Arg7;
                        break;
                    case 20:
                        //c.lcd_text.
                        break;
                    case 24:
                        c.temp_acr_mode.scp_number = id;
                        c.temp_acr_mode.acr_number = action.Arg1;
                        c.temp_acr_mode.acr_mode = action.Arg2;
                        c.temp_acr_mode.time = action.Arg3;
                        c.temp_acr_mode.nAuthModFlags = action.Arg4;
                        break;
                    case 25:
                        c.card_sim.nScp = id;
                        c.card_sim.nCommand = 1;
                        c.card_sim.nAcr = action.Arg1;
                        c.card_sim.e_time = action.Arg2;
                        c.card_sim.nFmtNum = action.Arg3;
                        c.card_sim.nFacilityCode = action.Arg4;
                        c.card_sim.nCardholderId = action.Arg5;
                        c.card_sim.nIssueCode = action.Arg6;
                        break;
                    case 26:
                        c.use_limit.scp_number = id;
                        c.use_limit.cardholder_id = action.Arg1;
                        c.use_limit.new_limit = action.Arg2;
                        break;
                    case 27:
                        c.oper_mode.scp_number = id;
                        c.oper_mode.oper_mode = action.Arg1;
                        c.oper_mode.enforce_existing = action.Arg2;
                        c.oper_mode.existing_mode = action.Arg3;
                        break;
                    case 28:
                        c.key_sim.nScp = id;
                        c.key_sim.nAcr = action.Arg1;
                        c.key_sim.e_time = action.Arg2;
                        for (int i = 0; i < action.StrArg.Length; i++)
                        {
                            c.key_sim.keys[i] = action.StrArg[i];
                        }
                        break;
                    case 30:
                        c.batch_trans.nScpID = id;
                        c.batch_trans.trigNumber = (ushort)action.Arg1;
                        c.batch_trans.actType = (ushort)action.Arg2;
                        break;
                    case 126:
                    case 127:
                        c.delay.delay_time = action.Arg1;
                        break;
                    default:
                        break;
                }

                bool flag = SendCommand((short)enCfgCmnd.enCcProc, c);
                if (flag)
                {
                    return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(id), _commandTimeout);
                }
                return false;

            }

            return false;
        }


        

        public async Task<bool> ActionSpecificationAsync(short ComponentId,List<Entity.Action> en)
        {
            foreach(var action in en)
            {
                if (action.ActionType == 9) continue;
                CC_ACTN c = new CC_ACTN(action.ActionType);
                c.hdr.lastModified = 0;
                c.hdr.scp_number = action.ScpId;
                c.hdr.proc_number = ComponentId;
                c.hdr.action_type = action.ActionType;
                //c.hdr.arg
                switch (action.ActionType)
                {
                    case 1:
                        c.mp_mask.scp_number = action.ScpId;
                        c.mp_mask.mp_number = action.Arg1;
                        c.mp_mask.set_clear = action.Arg2;
                        break;
                    case 2:
                        c.cp_ctl.scp_number = action.ScpId;
                        c.cp_ctl.cp_number = action.Arg1;
                        c.cp_ctl.command = action.Arg2;
                        c.cp_ctl.on_time = action.Arg3;
                        c.cp_ctl.off_time = action.Arg4;
                        c.cp_ctl.repeat = action.Arg5;
                        break;
                    case 3:
                        c.acr_mode.scp_number = action.ScpId;
                        c.acr_mode.acr_number = action.Arg1;
                        c.acr_mode.acr_mode = action.Arg2;
                        break;
                    case 4:
                        c.fo_mask.scp_number = action.ScpId;
                        c.fo_mask.acr_number = action.Arg1;
                        c.fo_mask .set_clear = action.Arg2;
                        break;
                    case 5:
                        c.ho_mask.scp_number = action.ScpId;
                        c.ho_mask.acr_number = action.Arg1;
                        c.ho_mask.set_clear = action.Arg2;
                        break;
                    case 6:
                        c.unlock.scp_number = action.ScpId;
                        c.unlock.acr_number = action.Arg1;
                        c.unlock.floor_number = action.Arg2;
                        c.unlock.strk_tm = action.Arg3;
                        c.unlock.t_held = action.Arg4;
                        c.unlock.t_held_pre = action.Arg5; 
                        break;
                    case 7:
                        c.proc.scp_number = action.ScpId;
                        c.proc.proc_number = action.Arg1;
                        c.proc.command = action.Arg2;
                        break;
                    case 8:
                        c.tv_ctl.scp_number = action.ScpId;
                        c.tv_ctl.tv_number = action.Arg1;
                        c.tv_ctl.set_clear = action.Arg2;
                        break;
                    case 9:
                        c.tz_ctl.scp_number = action.ScpId;
                        c.tz_ctl.tz_number = action.Arg1;
                        c.tz_ctl.command = action.Arg2;
                        break;
                    case 10:
                        c.led_mode.scp_number = action.ScpId;
                        c.led_mode.acr_number = action.Arg1;
                        c.led_mode.led_mode = action.Arg2;
                        break;
                    case 14:
                        c.mpg_set.scp_number = action.ScpId;
                        c.mpg_set.mpg_number = action.Arg1;
                        c.mpg_set.command = action.Arg2;
                        c.mpg_set.arg1 = action.Arg3;
                        break;
                    case 15:
                        c.mpg_test_mask.scp_number = action.ScpId;
                        c.mpg_test_mask.mpg_number = action.Arg1;
                        c.mpg_test_mask.action_prefix_ifz = action.Arg2;
                        c.mpg_test_mask.action_prefix_ifnz = action.Arg3;
                        break;
                    case 16:
                        c.mpg_test_active.scp_number = action.ScpId;
                        c.mpg_test_active.mpg_number = action.Arg1;
                        c.mpg_test_active.action_prefix_ifnoactive = action.Arg2;
                        c.mpg_test_active.action_prefix_ifactive = action.Arg3;
                        break;
                    case 17:
                        c.area_set.scp_number = action.ScpId;
                        c.area_set.area_number = action.Arg1;
                        c.area_set.command = action.Arg2;
                        c.area_set.occ_set = action.Arg3;
                        break;
                    case 18:
                        c.unlock.scp_number = action.ScpId;
                        c.unlock.acr_number = action.Arg1;
                        c.unlock.floor_number = action.Arg2;
                        c.unlock.strk_tm = action.Arg3;
                        c.unlock.t_held = action.Arg4;
                        c.unlock.t_held_pre = action.Arg5;
                        break;
                    case 19:
                        c.rled_tmp.scp_number = action.ScpId;
                        c.rled_tmp.acr_number = action.Arg1;
                        c.rled_tmp.color_on = action.Arg2;
                        c.rled_tmp.color_off = action.Arg3;
                        c.rled_tmp.ticks_on = action.Arg4;
                        c.rled_tmp.ticks_off = action.Arg5;
                        c.rled_tmp.repeat = action.Arg6;
                        c.rled_tmp.beeps = action.Arg7;
                        break;
                    case 20:
                        //c.lcd_text.
                        break;
                    case 24:
                        c.temp_acr_mode.scp_number = action.ScpId;
                        c.temp_acr_mode.acr_number = action.Arg1;
                        c.temp_acr_mode.acr_mode = action.Arg2;
                        c.temp_acr_mode.time = action.Arg3;
                        c.temp_acr_mode.nAuthModFlags = action.Arg4;
                        break;
                    case 25:
                        c.card_sim.nScp = action.ScpId;
                        c.card_sim.nCommand = 1;
                        c.card_sim.nAcr = action.Arg1;
                        c.card_sim.e_time = action.Arg2;
                        c.card_sim.nFmtNum = action.Arg3;
                        c.card_sim.nFacilityCode = action.Arg4;
                        c.card_sim.nCardholderId = action.Arg5;
                        c.card_sim.nIssueCode = action.Arg6;
                        break;
                    case 26:
                        c.use_limit.scp_number = action.ScpId;
                        c.use_limit.cardholder_id = action.Arg1;
                        c.use_limit.new_limit = action.Arg2;
                        break;
                    case 27:
                        c.oper_mode.scp_number = action.ScpId;
                        c.oper_mode.oper_mode = action.Arg1;
                        c.oper_mode.enforce_existing = action.Arg2;
                        c.oper_mode.existing_mode = action.Arg3;
                        break;
                    case 28:
                        c.key_sim.nScp = action.ScpId;
                        c.key_sim.nAcr = action.Arg1;
                        c.key_sim.e_time = action.Arg2;
                        for(int i = 0;i < action.StrArg.Length; i++)
                        {
                            c.key_sim.keys[i] = action.StrArg[i];
                        }
                        break;
                    case 30:
                        c.batch_trans.nScpID = action.ScpId;
                        c.batch_trans.trigNumber = (ushort)action.Arg1;
                        c.batch_trans.actType = (ushort)action.Arg2;
                        break;
                    case 126:
                    case 127:
                        c.delay.delay_time = action.Arg1;
                        break;
                    default:
                        break;
                }

                bool flag = SendCommand((short)enCfgCmnd.enCcProc,c);
                if (flag)
                {
                    return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(action.ScpId), _commandTimeout);
                }
                return false;

            }
            return false;
        }


        #endregion



    }
}
