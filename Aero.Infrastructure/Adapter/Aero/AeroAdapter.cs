using System;
using Aero.Application.Commands.Interfaces;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Aero.Infrastructure.Services;
using Aero.Infrastructure.Settings;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Options;

namespace Aero.Infrastructure.Adapter.Aero;

public sealed class AeroAdapter(IOptions<AppSettings> options,IDeviceRepository hw,ICmndRepository cmnd) : BaseAeroCommand, IAeroAdapter
{
      private readonly AppSettings settings = options.Value;
      public DeviceType Type => DeviceType.Aero;

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

      #region Access Level

      public async Task<bool> AccessLevelConfigurationExtended(short ScpId, short component, short tzAcr)
      {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = component;
            for (int i = 0; i < cc.tz.Length; i++)
            {

                  cc.tz[i] = tzAcr;

            }

            bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
            if (flag)
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = true,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });
            }
            else
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = -1,
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = false,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });

            }
            return flag;
      }

      public async Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, AccessLevel data)
      {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            foreach (var d in data.Components)
            {
                  cc.tz[d.AcrId] = d.TimezoneId;
            }

            bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
            if (flag)
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = true,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });
            }
            else
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = -1,
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = false,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });

            }
            return flag;
      }

      public async Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, CreateAccessLevel data)
      {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            foreach (var d in data.Components)
            {
                  cc.tz[d.AcrId] = d.TimezoneId;
            }

            bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
            if (flag)
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = true,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });
            }
            else
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = -1,
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = false,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });

            }
            return flag;
      }

      public async Task<bool> AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<AccessLevelComponent> component)
      {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            foreach (var d in component)
            {
                  cc.tz[d.AcrId] = d.TimezoneId;
            }

            bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
            if (flag)
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = true,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });
            }
            else
            {
                  await cmnd.AddAsync(new CommandAudit
                  {
                        TagNo = -1,
                        ScpId = ScpId,
                        Mac = await hw.GetMacFromComponentAsync(ScpId),
                        Command = enCfgCmnd.enCcAlvlEx.ToString(),
                        IsPending = false,
                        IsSuccess = false,
                        NakReason = "",
                        NakDescCode = 0,
                        LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
                  });

            }
            return flag;
      }

      #endregion

      #region Access Area

      public async Task<bool> ConfigureAccessArea(short ScpId, short AreaNo, short MultiOccu, short AccControl, short OccControl, short OccSet, short OccMax, short OccUp, short OccDown, short AreaFlag)
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

            bool flag = Send((short)enCfgCmnd.enCcAreaSpc, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
                IsPending = true,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });
        }
        else
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = -1,
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });

        }
        return flag;
      }

      public async Task<bool> GetAccessAreaStatus(short ScpId, short ComponentId, short Number)
      {
            CC_AREASRQ cc = new CC_AREASRQ();
            cc.scp_number = ScpId;
            cc.first = ComponentId;
            cc.count = Number;

            bool flag = Send((short)enCfgCmnd.enCcAreaSrq, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
                IsPending = true,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });
        }
        else
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = -1,
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });

        }
        return flag;
      }

      #endregion

      #region Card format

      public async Task<bool> CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
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

            bool flag = Send((short)enCfgCmnd.enCcScpCfmt, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
                IsPending = true,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });
        }
        else
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = -1,
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });

        }
        return flag;
      }



      public async Task<bool> CardFormatterConfiguration(short ScpId, CardFormat dto, short funtionId)
      {
            CC_SCP_CFMT cc = new CC_SCP_CFMT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = dto.CfmtId;
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

            bool flag = Send((short)enCfgCmnd.enCcScpCfmt, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
                IsPending = true,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });
        }
        else
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = -1,
                ScpId = ScpId,
                Mac = await hw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await hw.GetLocationIdFromDriverIdAsync(ScpId)
            });

        }
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

            bool flag = Send((short)enCfgCmnd.enCcOutput, cc_op);
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

            bool flag = Send((short)enCfgCmnd.enCcCP, cc_cp);
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

            bool flag = Send((short)enCfgCmnd.enCcCpCtl, cc_cpctl);
            return flag;

      }


      public bool GetCpStatus(short ScpId, int DriverId, short Count)
      {
            CC_CPSRQ cc = new CC_CPSRQ();
            cc.scp_number = ScpId;
            cc.first = (short)DriverId;
            cc.count = Count;

            bool flag = Send((short)enCfgCmnd.enCcCpSrq, cc);
            return flag;
      }

      #endregion

      #region Door

      public bool ReaderSpecification(short ScpId, short SioNo, short ReaderNo, short DataFormat, short KeyPadMode, short LedDriveMode, short OsdpFlag)
      {
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

            bool flag = Send((short)enCfgCmnd.enCcReader, cc_rdr);
            return flag;
      }

      public bool AccessControlReaderConfiguration(Door door)
      {
            CC_ACR cc_acr = new CC_ACR();
            cc_acr.lastModified = 0;
            cc_acr.scp_number = (short)door.ScpId;
            cc_acr.acr_number = door.AcrId;
            cc_acr.access_cfg = door.AccessConfig;
            cc_acr.pair_acr_number = door.PairDoorNo;
            cc_acr.rdr_sio = door.Readers == null || door.Readers.Count == 0 ? (short)-1 : door.Readers.ElementAt(0).ModuleDriverId;
            cc_acr.rdr_number = door.Readers == null || door.Readers.Count == 0 ? (short)-1 : door.Readers.ElementAt(0).ReaderNo;
            cc_acr.strk_sio = door.Strk == null ? (short)-1 : door.Strk.ModuleDriverId;
            cc_acr.strk_number = door.Strk == null ? (short)-1 : door.Strk.OutputNo;
            cc_acr.strike_t_min = door.Strk == null ? (short)0 : door.Strk.StrkMin;
            if(door.Strk != null)
            {
                  cc_acr.strike_t_max = door.Strk.StrkMax;
                  cc_acr.strike_mode = door.Strk.StrkMode;
            }
            cc_acr.door_sio = door.Sensor == null ? (short)-1 : door.Sensor.ModuleDriverId;
            cc_acr.door_number = door.Sensor == null ? (short)-1 : door.Sensor.InputNo;
            if(door.Sensor != null)
            {
                 cc_acr.dc_held = door.Sensor.DcHeld; 
            }
            if (door.RequestExits is not null && door.RequestExits.Count > 0)
            {
                  cc_acr.rex0_sio = door.RequestExits.ElementAt(0).ModuleDriverId;
                  cc_acr.rex0_number = door.RequestExits.ElementAt(0).InputNo;
                  cc_acr.rex_tzmask[0] = door.RequestExits.ElementAt(0).MaskTimeZone;
                  if (door.RequestExits.Count > 1)
                  {
                        cc_acr.rex1_sio = door.RequestExits.ElementAt(1).ModuleDriverId;
                        cc_acr.rex1_number = door.RequestExits.ElementAt(1).InputNo;
                        cc_acr.rex_tzmask[1] = door.RequestExits.ElementAt(1).MaskTimeZone;
                  }
            }
            // if (door.Readers != null && door.Readers.Count > 1 && door.Readers.ElementAt(1).OsdpFlag == true)
            // {
            //       cc_acr.altrdr_sio = door.Readers.ElementAt(1).ModuleDriverId;
            //       cc_acr.altrdr_number = door.Readers.ElementAt(1).ReaderNo;
            //       cc_acr.altrdr_spec = door.ReaderOutConfiguration;
            // }
            cc_acr.cd_format = door.CardFormat;
            cc_acr.apb_mode = door.AntiPassbackMode;
            if (door.AreaInId > 0) cc_acr.apb_in = (short)door.AreaInId;
            if (door.AreaOutId > 0) cc_acr.apb_to = (short)door.AreaOutId!;
            if (door.SpareTags != -1) cc_acr.spare = door.SpareTags;
            if (door.AccessControlFlags != -1) cc_acr.actl_flags = door.AccessControlFlags;
            cc_acr.offline_mode = door.OfflineMode;
            cc_acr.default_mode = door.DefaultMode;
            cc_acr.default_led_mode = door.DefaultLEDMode;
            cc_acr.pre_alarm = door.PreAlarm;
            cc_acr.apb_delay = door.AntiPassbackDelay;
            cc_acr.strk_t2 = door.StrkT2;
            cc_acr.dc_held2 = door.DcHeld2;
            cc_acr.strk_follow_pulse = 0;
            cc_acr.strk_follow_delay = 0;
            cc_acr.nAuthModFlags = 0;
            cc_acr.nExtFeatureType = 0;
            cc_acr.dfofFilterTime = 0;

            bool flag = Send((short)enCfgCmnd.enCcACR, cc_acr);
            return flag;

      }

      public bool AccessControlReaderConfiguration(short ScpId, short AcrNo, Door dto,short AccessConfig)
      {
            CC_ACR cc_acr = new CC_ACR();
            cc_acr.lastModified = 0;
            cc_acr.scp_number = ScpId;
            cc_acr.acr_number = AcrNo;
            cc_acr.access_cfg = AccessConfig;
            cc_acr.pair_acr_number = dto.PairDoorNo;
            cc_acr.rdr_sio = dto.Readers == null || dto.Readers.Count == 0 ? (short)-1 : dto.Readers.ElementAt(0).ModuleDriverId;
            cc_acr.rdr_number = dto.Readers == null || dto.Readers.Count == 0 ? (short)-1 : dto.Readers.ElementAt(0).ReaderNo;
            cc_acr.strk_sio = dto.Strk == null ? (short)-1 : dto.Strk.ModuleDriverId;
            cc_acr.strk_number = dto.Strk == null ? (short)-1 : dto.Strk.OutputNo;
            cc_acr.strike_t_min = dto.Strk == null ? (short)-1 : dto.Strk.StrkMin;
            if(dto.Strk != null)
            {
                  cc_acr.strike_t_max = dto.Strk.StrkMax;
                  cc_acr.strike_mode = dto.Strk.StrkMode;
            }
            cc_acr.door_sio = dto.Sensor == null ? (short)-1 : dto.Sensor.ModuleDriverId;
            cc_acr.door_number = dto.Sensor == null ? (short)-1 : dto.Sensor.InputNo;
            if(dto.Sensor != null)
            {
                 cc_acr.dc_held = dto.Sensor.DcHeld; 
            }
            if (dto.RequestExits is not null && dto.RequestExits.Count > 0)
            {
                  cc_acr.rex0_sio = dto.RequestExits.ElementAt(0).ModuleDriverId;
                  cc_acr.rex0_number = dto.RequestExits.ElementAt(0).InputNo;
                  cc_acr.rex_tzmask[0] = dto.RequestExits.ElementAt(0).MaskTimeZone;
                  if (dto.RequestExits.Count > 1)
                  {
                        cc_acr.rex1_sio = dto.RequestExits.ElementAt(1).ModuleDriverId;
                        cc_acr.rex1_number = dto.RequestExits.ElementAt(1).InputNo;
                        cc_acr.rex_tzmask[1] = dto.RequestExits.ElementAt(1).MaskTimeZone;
                  }
            }
            if (dto.AccessConfig == 2 && dto.Readers != null && dto.Readers.Count > 1 && dto.Readers.ElementAt(1).OsdpFlag == true)
            {
                  cc_acr.altrdr_sio = dto.Readers.ElementAt(1).ModuleDriverId;
                  cc_acr.altrdr_number = dto.Readers.ElementAt(1).ReaderNo;
                  cc_acr.altrdr_spec = dto.ReaderOutConfiguration;
            }
            cc_acr.cd_format = dto.CardFormat;
            cc_acr.apb_mode = dto.AntiPassbackMode;
            if (dto.AreaInId > 0) cc_acr.apb_in = (short)dto.AreaInId;
            if (dto.AreaOutId > 0) cc_acr.apb_to = (short)dto.AreaOutId!;
            if (dto.SpareTags != -1) cc_acr.spare = dto.SpareTags;
            if (dto.AccessControlFlags != -1) cc_acr.actl_flags = dto.AccessControlFlags;
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

            bool flag = Send((short)enCfgCmnd.enCcACR, cc_acr);
            return flag;

      }

      public bool MomentaryUnlock(short ScpId, short AcrNo)
        {
            CC_UNLOCK cc_unlock = new CC_UNLOCK();
            cc_unlock.scp_number = ScpId;
            cc_unlock.acr_number = AcrNo;

            bool flag = Send((short)enCfgCmnd.enCcUnlock, cc_unlock);
            return flag;

        }

        public bool GetAcrStatus(short ScpId, short AcrNo, short Count)
        {
            CC_ACRSRQ cc = new CC_ACRSRQ();
            cc.scp_number = ScpId;
            cc.first = AcrNo;
            cc.count = Count;

            bool flag = Send((short)enCfgCmnd.enCcAcrSrq, cc);
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

            bool flag = Send((short)enCfgCmnd.enCcAcrMode, cc);
            return flag;
        }


      #endregion

      #region Holiday

      public bool HolidayConfiguration(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.Year,
                month = dto.Month,
                day = dto.Day,
                //extend = dto.extend,
                type_mask = 1
            };
            bool flag = Send((short)enCfgCmnd.enCcScpHoliday, cc);
            return flag;
        }

        public bool DeleteHolidayConfiguration(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.Year,
                month = dto.Month,
                day = dto.Day,
                //extend = dto.extend,
                type_mask = 0
            };
            bool flag = Send((short)enCfgCmnd.enCcScpHoliday, cc);
            return flag;
        }


        public bool ClearHolidayConfiguration(short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = 0,
                month = 1,
                day = 1,
                //extend = dto.extend,
                type_mask = 0
            };
            bool flag = Send((short)enCfgCmnd.enCcScpHoliday, cc);
            return flag;

        }

      #endregion

      #region User

      public bool AccessDatabaseCardRecord(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<short> AccessLevel, int Active, int Deactive = 2085970000)
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
                  cc.alvl[i] = AccessLevel[i];
            }
            cc.act_time = Active;
            cc.dact_time = Deactive;

            bool flag = Send((short)enCfgCmnd.enCcAdbCardI64DTic32, cc);
            return flag;

      }



      public bool CardDelete(short ScpId, long CardNo)
      {
            CC_CARDDELETEI64 cc = new CC_CARDDELETEI64();
            cc.scp_number = ScpId;
            cc.cardholder_id = CardNo;

            bool flag = Send((short)enCfgCmnd.enCcCardDeleteI64, cc);
            return flag;
      }


      #endregion

      #region Monitor Point

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

            bool flag = Send((short)enCfgCmnd.enCcMP, cc_mp);
            return flag;
        }

        public bool MonitorPointMask(short ScpId, short MpNo, int SetClear)
        {
            CC_MPMASK cc = new CC_MPMASK();
            cc.scp_number = ScpId;
            cc.mp_number = MpNo;
            cc.set_clear = (short)SetClear;

            bool flag = Send((short)enCfgCmnd.enCcMpMask, cc);
            return flag;
        }

        public bool GetMpStatus(short ScpId, short MpNo, short Count)
        {
            CC_MPSRQ cc = new CC_MPSRQ();
            cc.scp_number = ScpId;
            cc.first = MpNo;
            cc.count = Count;

            bool flag = Send((short)enCfgCmnd.enCcMpSrq, cc);
            return flag;
        }

      public bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime)
      {
        CC_IP cc = new CC_IP();
        cc.scp_number = ScpId;
        cc.lastModified = 0;
        cc.sio_number = SioNo;
        cc.input = InputNo;
        cc.icvt_num = InputMode;
        cc.debounce = Debounce;
        cc.hold_time = HoldTime;
        bool flag = Send((short)enCfgCmnd.enCcInput, cc);
        return flag;
      }

      #endregion

      #region Monitor Group

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
                c.nMpList[i] = l.PointType;
                i += 1;
                c.nMpList[i] = l.PointNumber;
            }

            bool flag = Send((short)enCfgCmnd.enCcMpg, c);
            return flag;
        }

        public bool MonitorPointGroupArmDisarm(short ScpId, short ComponentId, short Command, short Arg1)
        {
            CC_MPGSET c = new CC_MPGSET();
            c.scp_number = ScpId;
            c.mpg_number = ComponentId;
            c.command = Command;
            c.arg1 = Arg1;

            bool flag = Send((short)enCfgCmnd.enCcMpgSet, c);
            //if (flag)
            //{
            //    return await TrackCommandAsync(tag, hardware_id, Constants.command.C321);
            //}
            return flag;
        }

      #endregion

      #region Procedure

      public bool ActionSpecificationAsyncForAllHW(short ComponentId, Domain.Entities.Action action, List<short> ScpIds)
      {
            foreach (var id in ScpIds)
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

                  bool flag = Send((short)enCfgCmnd.enCcProc, c);
                  return flag;

            }

            return false;
      }

      public bool ActionSpecificationDelayAsync(short ComponentId, Domain.Entities.Action action)
      {

            CC_ACTN c = new CC_ACTN(127);
            c.hdr.lastModified = 0;
            c.hdr.scp_number = action.DeviceId;
            c.hdr.proc_number = ComponentId;
            c.hdr.action_type = 127;
            c.delay.delay_time = action.Arg1;
            //c.hdr.arg

            bool flag = Send((short)enCfgCmnd.enCcProc, c);
            return flag;
      }


      public bool ActionSpecificationAsync(short ComponentId, List<Domain.Entities.Action> en)
      {
            foreach (var action in en)
            {
                  if (action.ActionType == 9 || action.ActionType == 127) continue;
                  CC_ACTN c = new CC_ACTN(action.ActionType);
                  c.hdr.lastModified = 0;
                  c.hdr.scp_number = action.DeviceId;
                  c.hdr.proc_number = ComponentId;
                  c.hdr.action_type = action.ActionType;
                  //c.hdr.arg
                  switch (action.ActionType)
                  {
                        case 1:
                              c.mp_mask.scp_number = action.DeviceId;
                              c.mp_mask.mp_number = action.Arg1;
                              c.mp_mask.set_clear = action.Arg2;
                              break;
                        case 2:
                              c.cp_ctl.scp_number = action.DeviceId;
                              c.cp_ctl.cp_number = action.Arg1;
                              c.cp_ctl.command = action.Arg2;
                              c.cp_ctl.on_time = action.Arg3;
                              c.cp_ctl.off_time = action.Arg4;
                              c.cp_ctl.repeat = action.Arg5;
                              break;
                        case 3:
                              c.acr_mode.scp_number = action.DeviceId;
                              c.acr_mode.acr_number = action.Arg1;
                              c.acr_mode.acr_mode = action.Arg2;
                              break;
                        case 4:
                              c.fo_mask.scp_number = action.DeviceId;
                              c.fo_mask.acr_number = action.Arg1;
                              c.fo_mask.set_clear = action.Arg2;
                              break;
                        case 5:
                              c.ho_mask.scp_number = action.DeviceId;
                              c.ho_mask.acr_number = action.Arg1;
                              c.ho_mask.set_clear = action.Arg2;
                              break;
                        case 6:
                              c.unlock.scp_number = action.DeviceId;
                              c.unlock.acr_number = action.Arg1;
                              c.unlock.floor_number = action.Arg2;
                              c.unlock.strk_tm = action.Arg3;
                              c.unlock.t_held = action.Arg4;
                              c.unlock.t_held_pre = action.Arg5;
                              break;
                        case 7:
                              c.proc.scp_number = action.DeviceId;
                              c.proc.proc_number = action.Arg1;
                              c.proc.command = action.Arg2;
                              break;
                        case 8:
                              c.tv_ctl.scp_number = action.DeviceId;
                              c.tv_ctl.tv_number = action.Arg1;
                              c.tv_ctl.set_clear = action.Arg2;
                              break;
                        case 9:
                              c.tz_ctl.scp_number = action.DeviceId;
                              c.tz_ctl.tz_number = action.Arg1;
                              c.tz_ctl.command = action.Arg2;
                              break;
                        case 10:
                              c.led_mode.scp_number = action.DeviceId;
                              c.led_mode.acr_number = action.Arg1;
                              c.led_mode.led_mode = action.Arg2;
                              break;
                        case 14:
                              c.mpg_set.scp_number = action.DeviceId;
                              c.mpg_set.mpg_number = action.Arg1;
                              c.mpg_set.command = action.Arg2;
                              c.mpg_set.arg1 = action.Arg3;
                              break;
                        case 15:
                              c.mpg_test_mask.scp_number = action.DeviceId;
                              c.mpg_test_mask.mpg_number = action.Arg1;
                              c.mpg_test_mask.action_prefix_ifz = action.Arg2;
                              c.mpg_test_mask.action_prefix_ifnz = action.Arg3;
                              break;
                        case 16:
                              c.mpg_test_active.scp_number = action.DeviceId;
                              c.mpg_test_active.mpg_number = action.Arg1;
                              c.mpg_test_active.action_prefix_ifnoactive = action.Arg2;
                              c.mpg_test_active.action_prefix_ifactive = action.Arg3;
                              break;
                        case 17:
                              c.area_set.scp_number = action.DeviceId;
                              c.area_set.area_number = action.Arg1;
                              c.area_set.command = action.Arg2;
                              c.area_set.occ_set = action.Arg3;
                              break;
                        case 18:
                              c.unlock.scp_number = action.DeviceId;
                              c.unlock.acr_number = action.Arg1;
                              c.unlock.floor_number = action.Arg2;
                              c.unlock.strk_tm = action.Arg3;
                              c.unlock.t_held = action.Arg4;
                              c.unlock.t_held_pre = action.Arg5;
                              break;
                        case 19:
                              c.rled_tmp.scp_number = action.DeviceId;
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
                              c.temp_acr_mode.scp_number = action.DeviceId;
                              c.temp_acr_mode.acr_number = action.Arg1;
                              c.temp_acr_mode.acr_mode = action.Arg2;
                              c.temp_acr_mode.time = action.Arg3;
                              c.temp_acr_mode.nAuthModFlags = action.Arg4;
                              break;
                        case 25:
                              c.card_sim.nScp = action.DeviceId;
                              c.card_sim.nCommand = 1;
                              c.card_sim.nAcr = action.Arg1;
                              c.card_sim.e_time = action.Arg2;
                              c.card_sim.nFmtNum = action.Arg3;
                              c.card_sim.nFacilityCode = action.Arg4;
                              c.card_sim.nCardholderId = action.Arg5;
                              c.card_sim.nIssueCode = action.Arg6;
                              break;
                        case 26:
                              c.use_limit.scp_number = action.DeviceId;
                              c.use_limit.cardholder_id = action.Arg1;
                              c.use_limit.new_limit = action.Arg2;
                              break;
                        case 27:
                              c.oper_mode.scp_number = action.DeviceId;
                              c.oper_mode.oper_mode = action.Arg1;
                              c.oper_mode.enforce_existing = action.Arg2;
                              c.oper_mode.existing_mode = action.Arg3;
                              break;
                        case 28:
                              c.key_sim.nScp = action.DeviceId;
                              c.key_sim.nAcr = action.Arg1;
                              c.key_sim.e_time = action.Arg2;
                              for (int i = 0; i < action.StrArg.Length; i++)
                              {
                                    c.key_sim.keys[i] = action.StrArg[i];
                              }
                              break;
                        case 30:
                              c.batch_trans.nScpID = action.DeviceId;
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

                  bool flag = Send((short)enCfgCmnd.enCcProc, c);
                  return flag;

            }
            return false;
      }



      #endregion

      #region SCP

      /// <summary>
      /// Command used for detach controller
      /// </summary>
      /// <param name="component">SCP Id</param>
      /// <returns>Command status</returns>
      public bool DetachScp(short component)
      {
            CC_ATTACHSCP c = new CC_ATTACHSCP();
            c.nSCPId = component;
            c.nChannelId = 0;
            bool flag = Send((short)enCfgCmnd.enCcDetachScp, c);
            return flag;
      }

      public bool ResetScp(short component)
      {
            CC_RESET cc_reset = new CC_RESET();
            cc_reset.scp_number = component;
            bool flag = Send((short)enCfgCmnd.enCcReset, cc_reset);
            return flag;
      }

      public bool ScpDeviceSpecification(short component, ScpSetting setting)
      {
            CC_SCP_SCP cc_scp_scp = new CC_SCP_SCP();
            cc_scp_scp.lastModified = 0;
            cc_scp_scp.number = component;
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
            cc_scp_scp.gmt_offset = setting.gmtOffset;
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

            bool flag = Send((short)enCfgCmnd.enCcScpScp, cc_scp_scp);
            return flag;
      }

      public bool AccessDatabaseSpecification(short ScpId, ScpSetting setting)
      {
            CC_SCP_ADBS cc_scp_adbs = new CC_SCP_ADBS();
            cc_scp_adbs.lastModified = 0;
            cc_scp_adbs.nScpID = ScpId;
            cc_scp_adbs.nCards = setting.nCard;
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

            bool flag = Send((short)enCfgCmnd.enCcScpAdbSpec, cc_scp_adbs);
            return flag;
      }

      public bool TimeSet(short component)
      {
            CC_TIME cc_time = new CC_TIME();
            cc_time.scp_number = component;
            cc_time.custom_time = 0;

            bool flag = Send((short)enCfgCmnd.enCcTime, cc_time);
            return flag;
      }

      public bool ReadStructureStatus(short component)
      {
            CC_STRSRQ cc_strsq = new CC_STRSRQ();
            cc_strsq.nScpID = component;
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

            bool flag = Send((short)enCfgCmnd.enCcStrSRq, cc_strsq);
            return flag;
      }

      public bool DeleteScp(short ScpId)
      {
            CC_NEWSCP c = new CC_NEWSCP();
            c.nSCPId = ScpId;

            bool flag = Send((short)enCfgCmnd.enCcDeleteScp, c);
            return flag;

      }



      public bool SetScpId(short oldId, short newId)
      {
            CC_SCPID cc = new CC_SCPID();
            cc.scp_number = oldId;
            cc.scp_id = newId;
            bool flag = Send((short)enCfgCmnd.enCcScpID, cc);
            return flag;
      }

      public bool GetTransactionLogStatus(short ScpId)
      {
            CC_TRANSRQ c = new CC_TRANSRQ();
            c.scp_number = ScpId;

            bool flag = Send((short)enCfgCmnd.enCcTranSrq, c);
            return flag;
      }

      public bool SetTransactionLogIndex(short ScpId, bool isEnable)
      {
            var _commandValue = (short)enCfgCmnd.enCcTranIndex;
            CC_TRANINDEX cc_tranindex = new CC_TRANINDEX();
            cc_tranindex.scp_number = ScpId;
            cc_tranindex.tran_index = isEnable ? -2 : -1;

            bool flag = Send(_commandValue, cc_tranindex);
            return flag;
      }

      public bool GetWebConfigRead(short ScpId, short type)
        {
            CC_WEB_CONFIG_READ cc = new CC_WEB_CONFIG_READ();
            cc.scp_number = ScpId;
            cc.read_type = type;

            bool flag = Send((short)enCfgCmnd.enCcWebConfigRead, cc);
            return flag;
        }

        public short CheckSCPStatus(short scpID)
        {
            return SCPDLL.scpCheckOnline(scpID);
        }

      public bool GetIdReport(short ScpId)
      {
            CC_IDREQUEST cc_idrequest = new CC_IDREQUEST();
            cc_idrequest.scp_number = ScpId;

            bool flag = Send((short)enCfgCmnd.enCcIDRequest, cc_idrequest);
            return flag;
      }

      #endregion

      #region SIO

      public bool GetSioStatus(short ScpId, short SioNo)
  {
    CC_SIOSRQ cc_siosrq = new CC_SIOSRQ();
    cc_siosrq.scp_number = ScpId;
    cc_siosrq.first = SioNo;
    cc_siosrq.count = 1;
    bool flag = Send((short)enCfgCmnd.enCcSioSrq, cc_siosrq);
    return flag;
  }

  public bool SioDriverConfiguration(short ScpId, short SioDriverNo, short IoModulePort, int BaudRate, short ProtocolType)
  {
    CC_MSP1 cc_msp1 = new CC_MSP1();
    cc_msp1.lastModified = 0;
    cc_msp1.scp_number = ScpId;
    cc_msp1.msp1_number = SioDriverNo;
    cc_msp1.port_number = IoModulePort;
    cc_msp1.baud_rate = BaudRate;
    cc_msp1.reply_time = 90;
    cc_msp1.nProtocol = ProtocolType;
    cc_msp1.nDialect = 0;
    bool flag = Send((short)enCfgCmnd.enCcMsp1, cc_msp1);
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

    bool flag = Send((short)enCfgCmnd.enCcSio, cc_sio);
    return flag;
  }


      #endregion

      #region Trigger

        public bool TriggerSpecification(short ScpId, Trigger data, short ComponentId)
        {
            CC_TRGR cc = new CC_TRGR();
            cc.scp_number = ScpId;
            cc.trgr_number = ComponentId;
            cc.command = data.Command;
            cc.proc_num = data.ProcedureId;
            cc.src_type = data.SourceType;
            cc.src_number = data.SourceNumber;
            cc.tran_type = data.TranType;
            foreach (var code in data.CodeMap)
            {
                cc.code_map += (int)Math.Pow(2, code.Value);
            }
            cc.timezone = data.TimeZone;
            switch (data.TranType)
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



            bool flag = Send((short)enCfgCmnd.enCcTrgr, cc);
            return flag;

        }

        #endregion

      #region TimeZone

       public bool ExtendedTimeZoneActSpecification(short ScpId, Domain.Entities.TimeZone domain)
  {
        long active = UtilitiesHelper.DateTimeToElapeSecond(domain.ActiveTime);
        long deactive = UtilitiesHelper.DateTimeToElapeSecond(domain.DeactiveTime);
        CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
    cc.lastModified = 0;
    cc.nScpID = ScpId;
    cc.number = domain.TzId;
    cc.mode = domain.Mode;
    cc.actTime = (int)active;
    cc.deactTime = (int)deactive;
    cc.intervals = (short)domain.Intervals.Count;
    if (domain.Intervals.Count > 0)
    {
      int i = 0;
      foreach (var interval in domain.Intervals)
      {
        cc.i[i].i_days = (short)UtilitiesHelper.ConvertDayToBinary(interval.Days);
        cc.i[i].i_start = (short)UtilitiesHelper.ConvertTimeToEndMinute(interval.StartTime);
        cc.i[i].i_end = (short)UtilitiesHelper.ConvertTimeToEndMinute(interval.EndTime);
        i++;
      }

    }
    bool flag = Send((short)enCfgCmnd.enCcScpTimezoneExAct, cc);
    return flag;
  }

  public bool TimeZoneControl(short ScpId, short TzNo, short Command)
  {
    CC_TZCOMMAND cc = new CC_TZCOMMAND();
    cc.scp_number = ScpId;
    cc.tz_number = TzNo;
    cc.command = Command;
    bool flag = Send((short)enCfgCmnd.enCcTzCommand, cc);
    return flag;

  }


      #endregion

      #region LED

      public bool ReaderLedBuzzerFunctionSpec(short ScpId, Led data)
      {
            bool isSuccess = true;
            foreach (var config in data.Config)
            {
                  CC_RLEDSPC c = new CC_RLEDSPC();
                  c.lastModified = 0;
                  c.scp_number = ScpId;
                  c.led_mode = data.LedMode;
                  c.rled_id = config.RLedId;
                  c.on_color = config.OnColor;
                  c.off_color = config.OffColor;
                  c.on_time = config.OnTime;
                  c.off_time = config.OffTime;
                  c.repeat_count = config.RepeatCount;
                  c.beep_count = config.BeepCount;
                  bool flag = Send((short)enCfgCmnd.enCcRledSpc, c);
                  isSuccess = flag;
            }

            return isSuccess;

      }

      #endregion
}
