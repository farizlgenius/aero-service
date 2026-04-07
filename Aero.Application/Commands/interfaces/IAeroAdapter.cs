using System;
using Aero.Domain.Entities;
using Aero.Domain.Enums;

namespace Aero.Application.Commands.Interfaces;

public interface IAeroAdapter
{
      // List of command here
      DeviceType Type {get;}


      #region Configure the driver

      bool SystemLevelSpecification();
      bool CreateChannel();

      #endregion

      #region Access Level

      Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, short tzAcr);
    Task<bool> AccessLevelConfigurationExtended(short ScpId,short number, AccessLevel data);
    Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, CreateAccessLevel data);
    Task<bool> AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<AccessLevelComponent> component);
      

      #endregion

      #region Access Area

      Task<bool> ConfigureAccessArea(short ScpId, short AreaNo, short MultiOccu, short AccControl, short OccControl, short OccSet, short OccMax, short OccUp, short OccDown, short AreaFlag);
    Task<bool> GetAccessAreaStatus(short ScpId, short ComponentId, short Number);

      #endregion

      #region Card format

      Task<bool> CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc);
    Task<bool> CardFormatterConfiguration(short ScpId, CardFormat dto, short funtionId);

      #endregion

      #region  Control Point

      bool OutputPointSpecification(short ScpId, short SioNo, short OutputNo, short OutputMode);
      bool ControlPointConfiguration(short ScpId, short SioNo, short CpNo, short OutputNo, short DefaultPulseTime);
      bool ControlPointCommand(short ScpId, short cpNo, short command);
      bool GetCpStatus(short ScpId, int DriverId, short Count);

      #endregion

      #region  Door

      bool ReaderSpecification(short ScpId, short SioNo, short ReaderNo, short DataFormat, short KeyPadMode, short LedDriveMode, short OsdpFlag);
      bool AccessControlReaderConfiguration(short ScpId, short AcrNo, Door dto,short AccessConfig);

      bool AccessControlReaderConfiguration(Door door);
      bool MomentaryUnlock(short ScpId, short AcrNo);
      bool GetAcrStatus(short ScpId, short AcrNo, short Count);
      bool AcrMode(short ScpId, short AcrNo, short Mode);

      #endregion

      #region Holiday

       bool HolidayConfiguration(Holiday domain, short ScpId);
      bool DeleteHolidayConfiguration(Holiday domain, short ScpId);
     bool ClearHolidayConfiguration(short ScpId);

      #endregion

      #region User

      bool AccessDatabaseCardRecord(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<short> AccessLevel, int Active, int Deactive = 2085970000);
      bool CardDelete(short ScpId, long CardNo);


      #endregion

      #region  Monitor 

      bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime);
  bool MonitorPointConfiguration(short ScpId, short SioNo, short InputNo, short LfCode, short Mode, short DelayEntry, short DelayExit, short nMp);
  bool MonitorPointMask(short ScpId, short MpNo, int SetClear);
  bool GetMpStatus(short ScpId, short MpNo, short Count);

      #endregion

      #region Monitor Group

      bool ConfigureMonitorPointGroup(short ScpId, short ComponentId, short nMonitor, List<MonitorGroupList> list);
      bool MonitorPointGroupArmDisarm(short ScpId, short ComponentId, short Command, short Arg1);

      #endregion

      #region Procedure

      bool ActionSpecificationAsyncForAllHW(short ComponentId, Aero.Domain.Entities.Action action, List<short> ScpIds);
      bool ActionSpecificationDelayAsync(short ComponentId, Aero.Domain.Entities.Action action);
      bool ActionSpecificationAsync(short ComponentId, List<Aero.Domain.Entities.Action> en);


      #endregion

      #region SCP

       bool DetachScp(short component);
      bool ResetScp(short component);
      bool ScpDeviceSpecification(short component, ScpSetting setting);
      bool AccessDatabaseSpecification(short ScpId, ScpSetting setting);
      bool TimeSet(short component);
      bool ReadStructureStatus(short component);
      bool SetScpId(short oldId, short newId);
      bool GetTransactionLogStatus(short ScpId);
      bool SetTransactionLogIndex(short ScpId, bool isEnable);
      bool GetWebConfigRead(short ScpId, short type);
      short CheckSCPStatus(short scpID);

      bool GetIdReport(short ScpId);

      #endregion

      #region SIO

      bool SioDriverConfiguration(short ScpId, short SioDriverNo, short IoModulePort, int BaudRate, short ProtocolType);
      bool SioPanelConfiguration(short ScpId, short SioNo, short Model, short nInput, short nOutput, short nReader, short ModuleAddress, short SIODriverPort, bool isEnable);
      bool GetSioStatus(short ScpId, short SioNo);


      #endregion

      #region Trigger

      bool TriggerSpecification(short ScpId, Trigger data, short ComponentId);

      #endregion

      #region Timezone

      bool ExtendedTimeZoneActSpecification(short ScpId, Domain.Entities.TimeZone dto);
      bool TimeZoneControl(short ScpId,short Component,short Command);

      #endregion

      #region Setting
      // Command 122: Reader LED/Buzzer Function Specs
      bool ReaderLedBuzzerFunctionSpec(short ScpId,Aero.Domain.Entities.Led data);


      #endregion
}
