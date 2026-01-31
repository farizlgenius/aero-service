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

      

       



    }
}
