using System;
using Aero.Application.Commands.Interfaces;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Enums;

namespace Aero.Infrastructure.Adapter.Amico;

public sealed class AmicoAdapter : IAeroAdapter
{
      public DeviceType Type => DeviceType.Amico;

      

      #region Aero Command

       public bool AccessControlReaderConfiguration(short ScpId, short AcrNo, Door dto,short a)
      {
            throw new NotImplementedException();
      }

      public bool AccessControlReaderConfiguration(Door door)
      {
            throw new NotImplementedException();
      }

      public bool AccessDatabaseCardRecord(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<short> AccessLevel, int Active, int Deactive = 2085970000)
      {
            throw new NotImplementedException();
      }

      public bool AccessDatabaseSpecification(short ScpId, ScpSetting setting)
      {
            throw new NotImplementedException();
      }

      public Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, short tzAcr)
      {
            throw new NotImplementedException();
      }

      public Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, AccessLevel data)
      {
            throw new NotImplementedException();
      }

      public Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, CreateAccessLevel data)
      {
            throw new NotImplementedException();
      }

      public Task<bool> AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<AccessLevelComponent> component)
      {
            throw new NotImplementedException();
      }

      public bool AcrMode(short ScpId, short AcrNo, short Mode)
      {
            throw new NotImplementedException();
      }

      public bool ActionSpecificationAsync(short ComponentId, List<Domain.Entities.Action> en)
      {
            throw new NotImplementedException();
      }

      public bool ActionSpecificationAsyncForAllHW(short ComponentId, Domain.Entities.Action action, List<short> ScpIds)
      {
            throw new NotImplementedException();
      }

      public bool ActionSpecificationDelayAsync(short ComponentId, Domain.Entities.Action action)
      {
            throw new NotImplementedException();
      }

      public bool CardDelete(short ScpId, long CardNo)
      {
            throw new NotImplementedException();
      }

      public Task<bool> CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
      {
            throw new NotImplementedException();
      }

      public Task<bool> CardFormatterConfiguration(short ScpId, CardFormat dto, short funtionId)
      {
            throw new NotImplementedException();
      }

      public short CheckSCPStatus(short scpID)
      {
            throw new NotImplementedException();
      }

      public bool ClearHolidayConfiguration(short ScpId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> ConfigureAccessArea(short ScpId, short AreaNo, short MultiOccu, short AccControl, short OccControl, short OccSet, short OccMax, short OccUp, short OccDown, short AreaFlag)
      {
            throw new NotImplementedException();
      }

      public bool ConfigureMonitorPointGroup(short ScpId, short ComponentId, short nMonitor, List<MonitorGroupList> list)
      {
            throw new NotImplementedException();
      }

      public bool ControlPointCommand(short ScpId, short cpNo, short command)
      {
            throw new NotImplementedException();
      }

      public bool ControlPointConfiguration(short ScpId, short SioNo, short CpNo, short OutputNo, short DefaultPulseTime)
      {
            throw new NotImplementedException();
      }

      public bool CreateChannel()
      {
            throw new NotImplementedException();
      }

      public bool DeleteHolidayConfiguration(Holiday domain, short ScpId)
      {
            throw new NotImplementedException();
      }

      public bool DetachScp(short component)
      {
            throw new NotImplementedException();
      }

      public bool ExtendedTimeZoneActSpecification(short ScpId, Domain.Entities.TimeZone dto)
      {
            throw new NotImplementedException();
      }

      public Task<bool> GetAccessAreaStatus(short ScpId, short ComponentId, short Number)
      {
            throw new NotImplementedException();
      }

      public bool GetAcrStatus(short ScpId, short AcrNo, short Count)
      {
            throw new NotImplementedException();
      }

      public bool GetCpStatus(short ScpId, int DriverId, short Count)
      {
            throw new NotImplementedException();
      }

      public bool GetIdReport(short ScpId)
      {
            throw new NotImplementedException();
      }

      public bool GetMpStatus(short ScpId, short MpNo, short Count)
      {
            throw new NotImplementedException();
      }

      public bool GetSioStatus(short ScpId, short SioNo)
      {
            throw new NotImplementedException();
      }

      public bool GetTransactionLogStatus(short ScpId)
      {
            throw new NotImplementedException();
      }

      public bool GetWebConfigRead(short ScpId, short type)
      {
            throw new NotImplementedException();
      }

      public bool HolidayConfiguration(Holiday domain, short ScpId)
      {
            throw new NotImplementedException();
      }

      public bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime)
      {
            throw new NotImplementedException();
      }

      public bool MomentaryUnlock(short ScpId, short AcrNo)
      {
            throw new NotImplementedException();
      }

      public bool MonitorPointConfiguration(short ScpId, short SioNo, short InputNo, short LfCode, short Mode, short DelayEntry, short DelayExit, short nMp)
      {
            throw new NotImplementedException();
      }

      public bool MonitorPointGroupArmDisarm(short ScpId, short ComponentId, short Command, short Arg1)
      {
            throw new NotImplementedException();
      }

      public bool MonitorPointMask(short ScpId, short MpNo, int SetClear)
      {
            throw new NotImplementedException();
      }

      public bool OutputPointSpecification(short ScpId, short SioNo, short OutputNo, short OutputMode)
      {
            throw new NotImplementedException();
      }

      public bool ReaderLedBuzzerFunctionSpec(short ScpId, Led data)
      {
            throw new NotImplementedException();
      }

      public bool ReaderSpecification(short ScpId, short SioNo, short ReaderNo, short DataFormat, short KeyPadMode, short LedDriveMode, short OsdpFlag)
      {
            throw new NotImplementedException();
      }

      public bool ReadStructureStatus(short component)
      {
            throw new NotImplementedException();
      }

      public bool ResetScp(short component)
      {
            throw new NotImplementedException();
      }

      public bool ScpDeviceSpecification(short component, ScpSetting setting)
      {
            throw new NotImplementedException();
      }

      public bool SetScpId(short oldId, short newId)
      {
            throw new NotImplementedException();
      }

      public bool SetTransactionLogIndex(short ScpId, bool isEnable)
      {
            throw new NotImplementedException();
      }

      public bool SioDriverConfiguration(short ScpId, short SioDriverNo, short IoModulePort, int BaudRate, short ProtocolType)
      {
            throw new NotImplementedException();
      }

      public bool SioPanelConfiguration(short ScpId, short SioNo, short Model, short nInput, short nOutput, short nReader, short ModuleAddress, short SIODriverPort, bool isEnable)
      {
            throw new NotImplementedException();
      }

      public bool SystemLevelSpecification()
      {
            throw new NotImplementedException();
      }

      public bool TimeSet(short component)
      {
            throw new NotImplementedException();
      }

      public bool TimeZoneControl(short ScpId, short Component, short Command)
      {
            throw new NotImplementedException();
      }

      public bool TriggerSpecification(short ScpId, Trigger data, short ComponentId)
      {
            throw new NotImplementedException();
      }

      #endregion

     
}
