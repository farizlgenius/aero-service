using AutoMapper;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Models;
using HIDAeroService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using HIDAeroService.Constants;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Runtime.CompilerServices;
using HIDAeroService.Logging;

namespace HIDAeroService.Service.Impl
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly AppDbContext _context;
        private readonly HelperService _helperService;
        private readonly IMapper _mapper;
        private readonly ILogger<TimeZoneService> _logger;
        private readonly AeroLibMiddleware _middleware;
        public TimeZoneService(AppDbContext context, HelperService helperService, AeroLibMiddleware config,IMapper mapper, ILogger<TimeZoneService> logger,AeroLibMiddleware middleware)
        {
            _middleware = middleware;
            _mapper = mapper;
            _helperService = helperService;
            _context = context;
            _logger = logger;
        }

        public async Task<Response<TimeZoneDto>> CreateAsync(TimeZoneDto dto)
        {
            List<string> errors = new List<string>();
            try
            {
                var componentNo = await _helperService.GetAvailableComponentNoAsync<ArTimeZone>(255);  
                List<short> Ids = await _context.ArScps.AsNoTracking().Select(x => x.ScpId).ToListAsync();
                foreach(var Id in Ids)
                {
                    List<short> intervalIds = dto.IntervalsNoList.Split(",",StringSplitOptions.RemoveEmptyEntries).Select(x => short.Parse(x.Trim())).ToList();
                    long active = _helperService.DateTimeToElapeSecond(dto.ActiveTime);
                    long deactive = _helperService.DateTimeToElapeSecond(dto.DeactiveTime);
                    List<ArInterval> intervals = await _context.ArIntervals.AsNoTracking().Where(x => intervalIds.Contains(x.ComponentNo)).ToListAsync();
                    if (!_middleware.write.ExtendedTimeZoneActSpecification(Id, dto, intervals, (int)active, (int)deactive))
                    {
                        CustomLogging.LogErr<TimeZoneService>(_logger, Helper.ResponseCommandUnsuccessMessageBuilder(Id), "ExtendedTimeZoneActSpecification");
                        errors.Add(Helper.ResponseCommandUnsuccessMessageBuilder(Id));
                    }
                }

                dto.ComponentNo = componentNo;
                await _context.ArTimeZones.AddAsync(_mapper.Map<ArTimeZone>(dto));
                await _context.SaveChangesAsync();

                CustomLogging.LogInfo<TimeZoneService>(_logger, "Create TimeZone", "CreateAsync");
                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.Created,dto,ConstantsHelper.CREATED,errors);

            } catch (Exception e) 
            {
                errors.Add(e.Message);
                _logger.LogError(e.Message);
                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.InternalServerError,null,ConstantsHelper.INTERNAL_ERROR,errors);
            }

        }

        public async Task<Response<TimeZoneDto>> DeleteAsync(short id)
        {
            CustomLogging.LogInfo<TimeZoneService>(_logger, $"DELETE TimeZone with Component no: {id}", "DeleteAsync");
            List<string> errors = new List<string>();
            try
            {
                var entity = await _context.ArTimeZones.FirstOrDefaultAsync(x => x.ComponentNo == id);
                if (entity == null) return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.OK,null, ConstantsHelper.NOT_FOUND_RECORD, errors);

                _context.ArTimeZones.Remove(entity);
                await _context.SaveChangesAsync();
                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.OK,_mapper.Map<TimeZoneDto>(entity), ConstantsHelper.REMOVE_SUCCESS, errors);

            }
            catch(Exception e)
            {
                errors.Add(e.Message);
                _logger.LogError(e.Message);
                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.InternalServerError,null, ConstantsHelper.INTERNAL_ERROR, errors);
            }
        }

        public IEnumerable<ArTimeZone> GetAllSetting()
        {
            try
            {

                return _context.ArTimeZones.ToArray();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                return Enumerable.Empty<ArTimeZone>();
            }
        }

        public async Task<Response<IEnumerable<TimeZoneDto>>> GetAsync()
        {
            List<string> errors = new List<string>(); 
            try
            {
                CustomLogging.LogInfo<TimeZoneService>(_logger, $"GET TimeZones", "GetAsync");

                var entities = await _context.ArTimeZones.ToArrayAsync();
                List<TimeZoneDto> dtos = new List<TimeZoneDto>();
                if(entities.Count() == 0) return Helper.ResponseBuilder<IEnumerable<TimeZoneDto>>(HttpStatusCode.OK,dtos, ConstantsHelper.NOT_FOUND_RECORD, errors);
                foreach (var entity in entities) 
                {
                    dtos.Add(_mapper.Map<TimeZoneDto>(entity));
                }
                return Helper.ResponseBuilder<IEnumerable<TimeZoneDto>>(HttpStatusCode.OK,dtos,ConstantsHelper.SUCCESS , errors);

            }
            catch(Exception e)
            {
                errors.Add(e.Message);
                _logger.LogError(e.Message);
                return Helper.ResponseBuilder<IEnumerable<TimeZoneDto>>(HttpStatusCode.InternalServerError, Enumerable.Empty<TimeZoneDto>(),ConstantsHelper.INTERNAL_ERROR,errors );

            }
        }

        public async Task<Response<TimeZoneDto>> GetByIdAsync(short id)
        {
            List<string> errors = new List<string>();
            try
            {
                CustomLogging.LogInfo<TimeZoneService>(_logger, $"GET TimeZone By Component No: {id}", "GetByIdAsync");

                var entity = await _context.ArTimeZones.AsNoTracking().FirstOrDefaultAsync(p => p.ComponentNo == id);
                if (entity == null) return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.OK, null,  ConstantsHelper.NOT_FOUND_RECORD,errors);
                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.OK,_mapper.Map<TimeZoneDto>(entity), ConstantsHelper.SUCCESS, errors);

            }
            catch (Exception e)
            {
                errors.Add(e.Message);
                _logger.LogError(e.Message);
                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.InternalServerError, null, ConstantsHelper.INTERNAL_ERROR, errors);

            }
        }
        public async Task<Response<TimeZoneDto>> UpdateAsync(TimeZoneDto dto)
        {
            List<string> errors = new List<string>();
            try
            {
                CustomLogging.LogInfo<TimeZoneService>(_logger, $"UPDATE TimeZone Component no: {dto.ComponentNo}", "UpdateAsync");

                var entity = await _context.ArTimeZones.FirstOrDefaultAsync(p => p.ComponentNo == dto.ComponentNo);
                if (entity == null) return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.OK, null, ConstantsHelper.NOT_FOUND_RECORD, errors);
                List<short> Ids = await _context.ArScps.AsNoTracking().Select(x => x.ScpId).ToListAsync();
                foreach (var Id in Ids)
                {
                    List<short> intervalIds = dto.IntervalsNoList.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => short.Parse(x.Trim())).ToList();
                    long active = _helperService.DateTimeToElapeSecond(dto.ActiveTime);
                    long deactive = _helperService.DateTimeToElapeSecond(dto.DeactiveTime);
                    List<ArInterval> intervals = await _context.ArIntervals.AsNoTracking().Where(x => intervalIds.Contains(x.ComponentNo)).ToListAsync();
                    if (!_middleware.write.ExtendedTimeZoneActSpecification(Id, dto, intervals, (int)active, (int)deactive))
                    {
                        _logger.LogError($"SCP {Id} : " + ConstantsHelper.COMMAND_UNSUCCESS);
                        errors.Add($"SCP {Id} : " + ConstantsHelper.COMMAND_UNSUCCESS);
                    }
                }
                _mapper.Map(dto,entity);
                _context.ArTimeZones.Update(entity);
                await _context.SaveChangesAsync();

                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.OK, _mapper.Map<TimeZoneDto>(entity), ConstantsHelper.SUCCESS, errors);

            }
            catch (Exception e)
            {
                errors.Add(e.Message);
                _logger.LogError(e.Message);
                return Helper.ResponseBuilder<TimeZoneDto>(HttpStatusCode.InternalServerError, null,ConstantsHelper.INTERNAL_ERROR,errors);

            }
        }



    }
}
