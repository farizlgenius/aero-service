using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface ICfmtRepository : IBaseRepository<CardFormatDto,CardFormat>
{
    Task<int> CountByUpdateTimeAsync(DateTime sync);
    Task<short> GetLowestUnassignedNumberAsync(int max);
}
