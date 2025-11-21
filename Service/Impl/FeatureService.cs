using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Feature;
using HIDAeroService.Helpers;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class FeatureService(AppDbContext context) : IFeatureService
    {
        public async Task<ResponseDto<IEnumerable<FeatureDto>>> GetFeatureListAsync()
        {
            var dtos = await context.FeatureLists
                .Select(x => new FeatureDto
                {
                    Name = x.Name,
                    ComponentId = x.ComponentId,
                    IsWritable = false,

                }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<FeatureDto>>(dtos);
        }
    }
}
