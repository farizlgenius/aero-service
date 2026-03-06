using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record ProcedureDto(int Id,short DeviceId,short DriverId, string Name,int TriggerId, List<ActionDto> Actions,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
