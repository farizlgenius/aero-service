using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface ICompanyService
{
      Task<ResponseDto<IEnumerable<CompanyDto>>> GetAsync();
        Task<ResponseDto<Pagination<CompanyDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        Task<ResponseDto<CompanyDto>> GetByIdAsync(int id);
        Task<ResponseDto<CompanyDto>> CreateAsync(CompanyDto dto);
        Task<ResponseDto<CompanyDto>> DeleteByIdAsync(int id);
        Task<ResponseDto<IEnumerable<CompanyDto>>> DeleteRangeAsync(List<int> ids);
        Task<ResponseDto<CompanyDto>> UpdateAsync(CompanyDto dto);
}
