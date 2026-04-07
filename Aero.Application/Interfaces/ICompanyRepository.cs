using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface ICompanyRepository : IBaseRepository<CompanyDto,Company>
{
      Task<bool> IsAnyReferenceByIdAsync(int id);
}
