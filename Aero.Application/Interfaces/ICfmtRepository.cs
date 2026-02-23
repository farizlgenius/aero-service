using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface ICfmtRepository : IBaseRepository<CardFormatDto,CardFormat, CardFormat>
{
    Task<int> CountByUpdateTimeAsync(DateTime sync);
}
