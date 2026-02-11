using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using System;

namespace Aero.Application.Interfaces;

public interface ICredRepository : IBaseRepository<Credential>
{
      Task<int> DeleteByCardNoAsync(long Cardno);
    Task ToggleScanCardAsync(ScanCardDto dto);
}
