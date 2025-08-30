using AutoMapper;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Holiday;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HIDAeroService.Service.Impl
{
    public class HolidayService : IHolidayService
    {
        private readonly AppDbContext _context;
        private readonly AeroLibMiddleware _middleware;
        private readonly IMapper _mapper;
        private readonly ILogger<IHolidayService> _logger;
        private readonly HelperService _helperService;
        public HolidayService(AeroLibMiddleware middleware,AppDbContext context,IMapper mapper,ILogger<IHolidayService> logger,HelperService helperService) 
        {
            _helperService = helperService;
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _middleware = middleware;
        }

        // New

        public async Task<Response<IEnumerable<HolidayDto>>> GetAsync()
        {
            _logger.LogInformation("Hello From Hol");
            List<string> errors = new List<string>();
            try
            {
                List<HolidayDto> dtos = new List<HolidayDto>(); 
                var entities = await _context.ArHolidays.AsNoTracking().ToArrayAsync();
                if (entities.Length == 0) return Helper.ResponseBuilder<IEnumerable<HolidayDto>>(HttpStatusCode.OK, Enumerable.Empty<HolidayDto>(),ConstantsHelper.NOT_FOUND_RECORD, errors);
                foreach (var entity in entities) 
                {
                    dtos.Add(_mapper.Map<HolidayDto>(entity));
                }
                return Helper.ResponseBuilder<IEnumerable<HolidayDto>>(HttpStatusCode.OK, dtos, ConstantsHelper.SUCCESS, errors);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<IEnumerable<HolidayDto>>(HttpStatusCode.InternalServerError,Enumerable.Empty<HolidayDto>(),ConstantsHelper.INTERNAL_ERROR, errors);
            }
        }

        public async Task<Response<HolidayDto>> CreateAsync(HolidayDto dto)
        {
            List<string> errors = new List<string>();
            try
            {
                // Send command 
                List<short> scpIds = await _context.ArScps.Select(p => p.ScpId).ToListAsync();
                foreach (var id in scpIds) 
                {
                    if (!_middleware.write.HolidayConfiguration(dto,id))
                    {
                        _logger.LogError(Helper.ResponseCommandUnsuccessMessageBuilder(id));
                        errors.Add(Helper.ResponseCommandUnsuccessMessageBuilder(id));
                    }
                }

                var componentNo = await _helperService.GetAvailableComponentNoAsync<ArHoliday>(255);
                var entity = _mapper.Map<ArHoliday>(dto);
                entity.ComponentNo = componentNo;
                entity.IsActive = true;
                await _context.ArHolidays.AddAsync(entity);
                await _context.SaveChangesAsync();
                var ent = await _context.ArHolidays.FirstOrDefaultAsync(p => p.ComponentNo == componentNo);
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.InternalServerError, _mapper.Map<HolidayDto>(ent), ConstantsHelper.INTERNAL_ERROR, errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors);
            }
        }

        public async Task<Response<HolidayDto>> UpdateAsync(HolidayDto dto)
        {
            List<string> errors = new List<string>();
            try
            {
                var entity = await _context.ArHolidays.FirstOrDefaultAsync(p => p.ComponentNo == dto.ComponentNo);
                if (entity == null) return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.OK, null, ConstantsHelper.NOT_FOUND_RECORD, errors);
                _mapper.Map(dto,entity);
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.OK, null, ConstantsHelper.NOT_FOUND_RECORD, errors);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors);
            }
        }

        public async Task<Response<HolidayDto>> DeleteAsync(short id)
        {
            List<string> errors = new List<string>();
            try
            {
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.OK, null, ConstantsHelper.NOT_FOUND_RECORD, errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors);
            }
        }

        public async Task<Response<HolidayDto>> GetByIdAsync(short id)
        {
            List<string> errors = new List<string>();
            try
            {
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.OK, null, ConstantsHelper.NOT_FOUND_RECORD, errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<HolidayDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors);
            }
        }
    }
}
