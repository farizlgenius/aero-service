using AutoMapper;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Interval;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Models;
using HIDAeroService.Service.Interface;
using LibNoise.Primitive;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HIDAeroService.Service.Impl
{
    public class IntervalService : IIntervalService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly HelperService _helperService;
        private readonly ILogger<IIntervalService> _logger;

        public IntervalService(IMapper mapper, AppDbContext context,HelperService helperService,ILogger<IIntervalService> logger) 
        {
            _helperService = helperService;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<IntervalDto>>> GetAsync()
        {
            List<string> errors = new List<string>();
            List<IntervalDto> dtos = new List<IntervalDto>();
            var intervals = await _context.ArIntervals.AsNoTracking().ToArrayAsync();
            if (intervals.Count() == 0) return Helper.ResponseBuilder<IEnumerable<IntervalDto>>(HttpStatusCode.OK,Enumerable.Empty<IntervalDto>(),ConstantsHelper.NOT_FOUND_RECORD,errors);
            foreach (var interval in intervals) 
            {
                dtos.Add(_mapper.Map<IntervalDto>(interval));
            }
            return Helper.ResponseBuilder<IEnumerable<IntervalDto>>(HttpStatusCode.OK, dtos, ConstantsHelper.SUCCESS, errors); 
        }

        public async Task<Response<IntervalDto>> CreateAsync(IntervalDto dto)
        {
            List<string> errors = new List<string>();
            try
            {   
                var componentNo = await _helperService.GetAvailableComponentNoAsync<ArInterval>(0);
                var entity = _mapper.Map<ArInterval>(dto);
                entity.ComponentNo = componentNo;
                entity.IsActive = true;
                _context.ArIntervals.Add(entity);
                await _context.SaveChangesAsync();
                var data = await _context.ArIntervals.AsNoTracking().Where(p => p.ComponentNo == componentNo).FirstOrDefaultAsync();
                return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.OK, _mapper.Map<IntervalDto>(data), ConstantsHelper.SUCCESS, errors);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors); ;
            }
            
        }

        public async Task<Response<IntervalDto>> UpdateAsync(IntervalDto dto)
        {
            List<string> errors = new List<string>();
            try
            {
                var entity = await _context.ArIntervals.FirstOrDefaultAsync(p => p.ComponentNo == dto.ComponentNo);
                if (entity == null) return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.OK,null, ConstantsHelper.SUCCESS, errors);
                _mapper.Map(dto,entity);
                _context.ArIntervals.Update(entity);
                await _context.SaveChangesAsync();


                return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.OK, dto, ConstantsHelper.SUCCESS, errors);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors);
            }
        }

        public async Task<Response<IntervalDto>> RemoveAsync(short componentNo)
        {
            List<string> errors = new List<string>();
            try
            {
                var entity = await _context.ArIntervals.FirstOrDefaultAsync(p => p.ComponentNo == componentNo);
                if (entity == null) return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.OK, null, ConstantsHelper.SUCCESS, errors);
                _context.ArIntervals.Remove(entity);
                await _context.SaveChangesAsync();
                return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.OK, _mapper.Map<IntervalDto>(entity), ConstantsHelper.SUCCESS, errors);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errors.Add(ex.Message);
                return Helper.ResponseBuilder<IntervalDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors);
            }

        }





    }
}
