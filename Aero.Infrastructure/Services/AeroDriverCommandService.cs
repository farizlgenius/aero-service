using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Settings;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Options;

namespace Aero.Infrastructure.Services;

public sealed class AeroDriverCommandService(IOptions<AppSettings> options) : BaseAeroCommand, IAeroDriverCommand
{

      private readonly AppSettings settings =  options.Value;

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
      public bool SystemLevelSpecification()
      {
            CC_SYS cc_sys = new CC_SYS();
            cc_sys.nPorts = settings.AeroDrivers.nPort;
            cc_sys.nScps = settings.AeroDrivers.nScps;
            cc_sys.nTimezones = 0;
            cc_sys.nHolidays = 0;
            cc_sys.bDirectMode = 1;
            cc_sys.debug_rq = 0;
            for (int i = 0; i < cc_sys.nDebugArg.Length; i++)
            {
                  cc_sys.nDebugArg[i] = 0;
            }
            bool flag = Send((short)enCfgCmnd.enCcSystem, cc_sys);
            
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
      public bool CreateChannel()
      {
            CC_CHANNEL cc_channel = new CC_CHANNEL();
            cc_channel.nChannelId = settings.AeroDrivers.nChannelId;
            cc_channel.cType = settings.AeroDrivers.cType;
            cc_channel.cPort = settings.Ports.Aero;
            cc_channel.baud_rate = 0;
            cc_channel.timer1 = 3000;
            cc_channel.timer2 = 0;
            for (int i = 0; i < cc_channel.cModemId.Length; i++)
            {
                  cc_channel.cModemId[i] = '\0';
            }
            cc_channel.cRTSMode = 0;
            bool flag = Send((short)enCfgCmnd.enCcCreateChannel, cc_channel);
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

















}
