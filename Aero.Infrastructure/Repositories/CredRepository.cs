using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Listenser;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Infrastructure.Repositories;

public class CredRepository(AppDbContext context,IQHwRepository qHw,IServiceScopeFactory factory) : ICredRepository
{
      public async Task<int> AddAsync(Credential data)
      {
            throw new NotImplementedException();
      }

    public async Task ToggleScanCardAsync(ScanCardDto dto)
    {
        using var scope = factory.CreateScope();
        var m = scope.ServiceProvider.GetRequiredService<AeroWorker>();
        m.isWaitingCardScan = true;
        m.ScanScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);
        m.ScanAcrNo = dto.DoorId;

    }

      public async Task<int> DeleteByCardNoAsync(long Cardno)
      {
            var en = await context.credential
            .Where(x => x.card_no == Cardno)
            .OrderBy(x => x.card_no)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.credential.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateAsync(Credential newData)
      {
            throw new NotImplementedException();
      }
}
