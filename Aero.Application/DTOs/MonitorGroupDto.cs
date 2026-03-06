using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record MonitorGroupDto(int Id,short DeviceId,short DriverId,string Name, short nMpCount, List<MonitorGroupListDto> nMpList,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
