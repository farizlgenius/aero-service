using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IDepartmentService
{
      Task<ResponseDto<IEnumerable<DepartmentDto>>> GetAsync();
        Task<ResponseDto<Pagination<DepartmentDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        Task<ResponseDto<DepartmentDto>> GetByIdAsync(int id);
        Task<ResponseDto<DepartmentDto>> CreateAsync(DepartmentDto dto);
        Task<ResponseDto<DepartmentDto>> DeleteByIdAsync(int id);
        Task<ResponseDto<IEnumerable<DepartmentDto>>> DeleteRangeAsync(List<int> ids);
        Task<ResponseDto<DepartmentDto>> UpdateAsync(DepartmentDto dto);
}
