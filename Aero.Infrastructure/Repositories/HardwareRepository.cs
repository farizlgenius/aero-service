using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class HardwareRepository(AppDbContext context) : IHardwareRepository
{
    public async Task<IEnumerable<Hardware>> GetAsync()
    {
        var response = await context.hardware
        .AsNoTracking()
        .Select(x => new Hardware
        {
            
        })
        .ToArrayAsync();

        return response;
    }
}
