using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IPositionService
{
      Task<ResponseDto<IEnumerable<PositionDto>>> GetAsync();
        Task<ResponseDto<Pagination<PositionDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        Task<ResponseDto<PositionDto>> GetByIdAsync(int id);
        Task<ResponseDto<PositionDto>> CreateAsync(PositionDto dto);
        Task<ResponseDto<PositionDto>> DeleteByIdAsync(int id);
        Task<ResponseDto<IEnumerable<PositionDto>>> DeleteRangeAsync(List<int> ids);
        Task<ResponseDto<PositionDto>> UpdateAsync(PositionDto dto);
}
