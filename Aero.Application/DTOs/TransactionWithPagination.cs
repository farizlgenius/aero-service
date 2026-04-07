namespace Aero.Application.DTOs;

public sealed record TransactionWithPagination(int Count, List<TransactionDto> Transactions);
