using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record ProcedureDto(short ProcId, string Name, List<ActionDto> Actions,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
