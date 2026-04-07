using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record MonitorGroupDto(int Id,int ScpId,short MpgId,string Name, short nMpCount, List<MonitorGroupListDto> nMpList,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
