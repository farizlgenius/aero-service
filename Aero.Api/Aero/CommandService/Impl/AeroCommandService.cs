using HID.Aero.ScpdNet.Wrapper;
using AeroService.Constants;
using AeroService.DTO.AccessLevel;
using AeroService.DTO.Door;
using AeroService.DTO.Action;
using AeroService.DTO.CardFormat;
using AeroService.DTO.Interval;
using AeroService.DTO.Procedure;
using AeroService.DTO.Trigger;
using AeroService.Entity;
using AeroService.Model;
using AeroService.Models;
using AeroService.Service;
using AeroService.Service.Impl;
using AeroService.Utility;
using LibNoise.Modifier;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using System;
using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace AeroService.Aero.CommandService
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
