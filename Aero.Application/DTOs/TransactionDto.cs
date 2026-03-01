using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record TransactionDto(DateTime DateTime, int SerialNumber, string Actor, double Source, string SourceDesc, string Origin, string SourceModule, double Type, string TypeDesc, double TranCode, string Image, string TranCodeDesc, string ExtendDesc, string Remark, List<TransactionFlagDto> TransactionFlags,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
