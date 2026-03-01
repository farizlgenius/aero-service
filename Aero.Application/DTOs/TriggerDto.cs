using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record TriggerDto(short TrigId, string Name, short Command, short ProcedureId, short SourceType, short SourceNumber, short TranType, List<TransactionCodeDto> CodeMap, short TimeZone,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
