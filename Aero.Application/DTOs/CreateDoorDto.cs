using System;
using Aero.Domain.Enums;

namespace Aero.Application.DTOs;

public sealed record CreateDoorDto(
    int DeviceId,
    string Name,
    short AccessConfig,
    short PairDoorNo,
    int direction,

    List<ReaderDto> Readers,
    short ReaderOutConfiguration,

    StrikeDto? Strk,
    SensorDto? Sensor,
    List<RequestExitDto>? RequestExits,

    short CardFormat,
    short AntiPassbackMode,
    short AntiPassBackIn,
    short AreaInId,
    short AntiPassBackOut,
    short AreaOutId,
    short SpareTags,
    short AccessControlFlags,

    short Mode,
    string ModeDesc,
    short OfflineMode,
    string OfflineModeDesc,
    short DefaultMode,
    string DefaultModeDesc,

    short DefaultLEDMode,
    short PreAlarm,
    short AntiPassbackDelay,
    short StrkT2,
    short DcHeld2,
    short StrkFollowPulse,
    short StrkFollowDelay,

    short nExtFeatureType,
    short IlPBSio,
    short IlPBNumber,
    short IlPBLongPress,
    short IlPBOutSio,
    short IlPBOutNum,
    short DfOfFilterTime,

    bool MaskHeldOpen,
    bool MaskForceOpen,

    int LocationId,
    bool IsActive
) : BaseDto(LocationId,IsActive);
