using System;
using System.Diagnostics;
using System.IO.Compression;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class DoorMapper
{
      public static Door ToDomain(DoorDto dto)
      {
            var res = new Door();
            // Base 
            MacBaseMapper.ToDomain(dto,res);

            res.AcrId = dto.AcrId;
            res.Name = dto.Name;
            res.AccessConfig = dto.AccessConfig;
            res.PairDoorNo = dto.PairDoorNo;
            res.Readers = dto.Readers.Count() == 0 ? new List<Reader>() : dto.Readers.Select(x => new Aero.Domain.Entities.Reader
            {
                  // Base 
                  HardwareName = x.HardwareName,
                  Mac = x.Mac,
                  ComponentId = x.ComponentId,
                  LocationId = x.LocationId,
                  IsActive = true,

                  ModuleId = x.ModuleId,
                  ReaderNo = x.ReaderNo,
                  DataFormat = x.DataFormat,
                  KeypadMode = x.KeypadMode,
                  LedDriveMode = x.LedDriveMode,
                  OsdpFlag = x.OsdpFlag,
                  OsdpBaudrate = x.OsdpBaudrate,
                  OsdpDiscover = x.OsdpDiscover,
                  OsdpTracing = x.OsdpTracing,
                  OsdpAddress = x.OsdpAddress,
                  OsdpSecureChannel = x.OsdpSecureChannel
            } ).ToList();
            res.ReaderOutConfiguration = dto.ReaderOutConfiguration;
            res.Strk = dto.Strk is null ? null : new Aero.Domain.Entities.Strike
            {
                  // Base 
                  HardwareName = dto.Strk.HardwareName,
                  Mac = dto.Strk.Mac,
                  ComponentId = dto.Strk.ComponentId,
                  LocationId = dto.Strk.LocationId,
                  IsActive = true,

                  ModuleId = dto.Strk.ModuleId,
                  OutputNo = dto.Strk.OutputNo,
                  RelayMode = dto.Strk.RelayMode,
                  OfflineMode = dto.Strk.OfflineMode,
                  StrkMax = dto.Strk.StrkMax,
                  StrkMin = dto.Strk.StrkMin,
                  StrkMode = dto.Strk.StrkMode
            };
            res.Sensor = dto.Sensor is null ? null : new Aero.Domain.Entities.Sensor
            {
                  // Base 
                  HardwareName = dto.Sensor.HardwareName,
                  Mac = dto.Sensor.Mac,
                  ComponentId = dto.Sensor.ComponentId,
                  LocationId = dto.Sensor.LocationId,
                  IsActive = true,

                  ModuleId = dto.Sensor.ModuleId,
                  InputNo = dto.Sensor.InputNo,
                  InputMode = dto.Sensor.InputMode,
                  Debounce = dto.Sensor.Debounce,
                  HoldTime = dto.Sensor.HoldTime,
                  DcHeld = dto.Sensor.DcHeld
            };
            res.RequestExits = dto.RequestExits is null || dto.RequestExits.Count == 0 ? new List<RequestExit>() : dto.RequestExits.Select(x => new Aero.Domain.Entities.RequestExit{
                  // Base 
                  HardwareName = x.HardwareName,
                  Mac = x.Mac,
                  ComponentId = x.ComponentId,
                  LocationId = x.LocationId,
                  IsActive = true,

                  ModuleId = x.ModuleId,
                  InputNo = x.InputNo,
                  InputMode = x.InputMode,
                  Debounce = x.Debounce,
                  HoldTime = x.HoldTime,
                  MaskTimeZone = x.MaskTimeZone
            }).ToList();
            res.CardFormat = dto.CardFormat;
            res.AntiPassbackMode = dto.AntiPassbackMode;
            res.AntiPassBackIn = dto.AntiPassBackIn;
            res.AntiPassBackOut = dto.AntiPassBackOut;
            res.SpareTags = dto.SpareTags;
            res.AccessControlFlags = dto.AccessControlFlags;
            res.Mode = dto.Mode;
            res.ModeDesc = dto.ModeDesc;
            res.OfflineMode = dto.OfflineMode;
            res.OfflineModeDesc = dto.OfflineModeDesc;
            res.DefaultMode = dto.DefaultMode;
            res.DefaultModeDesc = dto.DefaultModeDesc;
            res.DefaultLEDMode =  dto.DefaultLEDMode;
            res.PreAlarm = dto.PreAlarm;
            res.AntiPassbackDelay = dto.AntiPassbackDelay;
            res.StrkT2 = dto.StrkT2;
            res.DcHeld2 = dto.DcHeld2;
            res.StrkFollowPulse = dto.StrkFollowPulse;
            res.StrkFollowDelay = dto.StrkFollowDelay;
            res.nExtFeatureType = dto.nExtFeatureType;
            res.IlPBSio = dto.IlPBSio;
            res.IlPBNumber = dto.IlPBNumber;
            res.IlPBLongPress = dto.IlPBLongPress;
            res.IlPBOutSio = dto.IlPBOutSio;
            res.IlPBOutNum = dto.IlPBOutNum;
            res.DfOfFilterTime = dto.DfOfFilterTime;
            res.MaskForceOpen = dto.MaskForceOpen;
            res.MaskHeldOpen = dto.MaskHeldOpen;

            return res;
      }

    public static Door ToDeleteDomain(DoorDto dto)
    {
        var res = new Door();
        // Base 
        MacBaseMapper.ToDomain(dto, res);

        res.AcrId = dto.AcrId;
        res.Name = dto.Name;
        res.AccessConfig = dto.AccessConfig;
        res.PairDoorNo = dto.PairDoorNo;
        res.Readers = dto.Readers.Count() == 0 ? new List<Reader>() : dto.Readers.Select(x => new Aero.Domain.Entities.Reader
        {
            // Base 
            HardwareName = x.HardwareName,
            Mac = x.Mac,
            ComponentId = x.ComponentId,
            LocationId = x.LocationId,
            IsActive = true,

            ModuleId = -1,
            ReaderNo = -1,
            DataFormat = x.DataFormat,
            KeypadMode = x.KeypadMode,
            LedDriveMode = x.LedDriveMode,
            OsdpFlag = x.OsdpFlag,
            OsdpBaudrate = x.OsdpBaudrate,
            OsdpDiscover = x.OsdpDiscover,
            OsdpTracing = x.OsdpTracing,
            OsdpAddress = x.OsdpAddress,
            OsdpSecureChannel = x.OsdpSecureChannel
        }).ToList();
        res.ReaderOutConfiguration = dto.ReaderOutConfiguration;
        res.Strk = dto.Strk is null ? null : new Aero.Domain.Entities.Strike
        {
            // Base 
            HardwareName = dto.Strk.HardwareName,
            Mac = dto.Strk.Mac,
            ComponentId = -1,
            LocationId = dto.Strk.LocationId,
            IsActive = true,

            ModuleId = -1,
            OutputNo = dto.Strk.OutputNo,
            RelayMode = dto.Strk.RelayMode,
            OfflineMode = dto.Strk.OfflineMode,
            StrkMax = dto.Strk.StrkMax,
            StrkMin = dto.Strk.StrkMin,
            StrkMode = dto.Strk.StrkMode
        };
        res.Sensor = dto.Sensor is null ? null : new Aero.Domain.Entities.Sensor
        {
            // Base 
            HardwareName = dto.Sensor.HardwareName,
            Mac = dto.Sensor.Mac,
            ComponentId = -1,
            LocationId = dto.Sensor.LocationId,
            IsActive = true,

            ModuleId = -1,
            InputNo = dto.Sensor.InputNo,
            InputMode = dto.Sensor.InputMode,
            Debounce = dto.Sensor.Debounce,
            HoldTime = dto.Sensor.HoldTime,
            DcHeld = dto.Sensor.DcHeld
        };
        res.RequestExits = dto.RequestExits is null || dto.RequestExits.Count == 0 ? new List<RequestExit>() : dto.RequestExits.Select(x => new Aero.Domain.Entities.RequestExit
        {
            // Base 
            HardwareName = x.HardwareName,
            Mac = x.Mac,
            ComponentId = -1,
            LocationId = x.LocationId,
            IsActive = true,

            ModuleId = -1,
            InputNo = x.InputNo,
            InputMode = x.InputMode,
            Debounce = x.Debounce,
            HoldTime = x.HoldTime,
            MaskTimeZone = x.MaskTimeZone
        }).ToList();
        res.CardFormat = dto.CardFormat;
        res.AntiPassbackMode = dto.AntiPassbackMode;
        res.AntiPassBackIn = dto.AntiPassBackIn;
        res.AntiPassBackOut = dto.AntiPassBackOut;
        res.SpareTags = dto.SpareTags;
        res.AccessControlFlags = dto.AccessControlFlags;
        res.Mode = dto.Mode;
        res.ModeDesc = dto.ModeDesc;
        res.OfflineMode = dto.OfflineMode;
        res.OfflineModeDesc = dto.OfflineModeDesc;
        res.DefaultMode = dto.DefaultMode;
        res.DefaultModeDesc = dto.DefaultModeDesc;
        res.DefaultLEDMode = dto.DefaultLEDMode;
        res.PreAlarm = dto.PreAlarm;
        res.AntiPassbackDelay = dto.AntiPassbackDelay;
        res.StrkT2 = dto.StrkT2;
        res.DcHeld2 = dto.DcHeld2;
        res.StrkFollowPulse = dto.StrkFollowPulse;
        res.StrkFollowDelay = dto.StrkFollowDelay;
        res.nExtFeatureType = dto.nExtFeatureType;
        res.IlPBSio = dto.IlPBSio;
        res.IlPBNumber = dto.IlPBNumber;
        res.IlPBLongPress = dto.IlPBLongPress;
        res.IlPBOutSio = dto.IlPBOutSio;
        res.IlPBOutNum = dto.IlPBOutNum;
        res.DfOfFilterTime = dto.DfOfFilterTime;
        res.MaskForceOpen = dto.MaskForceOpen;
        res.MaskHeldOpen = dto.MaskHeldOpen;

        return res;
    }

}
