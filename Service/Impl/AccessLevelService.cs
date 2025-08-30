using AutoMapper;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Entity;
using HIDAeroService.Mapper;
using HIDAeroService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HIDAeroService.Service.Impl
{
    public class AccessLevelService : IAccessLevelService
    {
        private IMapper _mapper;   
        private ILogger<AccessLevelService> _logger;
        private readonly AppDbContext _context;
        private readonly AeroLibMiddleware _config;
        private readonly HelperService _helperService;
        public AccessLevelService(AppDbContext context, ILogger<AccessLevelService> logger, AeroLibMiddleware config, HelperService helperService,IMapper mapper)
        {
            _mapper = mapper;
            _helperService = helperService;
            _config = config;
            _logger = logger;
            _context = context;
        }

        public IEnumerable<ArAccessLevel> GetAllSetting()
        {
            _logger.LogInformation("Get Access Level List");
            return _context.ArAcccessLevels.ToArray();
        }

        public async Task<IEnumerable<AccessLevelDto>> GetAll()
        {
            IEnumerable<ArAccessLevel> acs = await _context.ArAcccessLevels.ToArrayAsync();
            List<AccessLevelDto> b = new List<AccessLevelDto>();
            foreach (var a in acs)
            {
                b.Add(_mapper.Map<AccessLevelDto>(a));
            }
            return b;
        }

        public async Task<AccessLevelTimeZoneDto> GetTimeZone(short ElementNo)
        {
            try
            {
                AccessLevelTimeZoneDto dto = new AccessLevelTimeZoneDto();
                ArAccessLevel entity = await _context.ArAcccessLevels.AsNoTracking().Where(p => p.ComponentNo == ElementNo).FirstAsync();
                return _mapper.Map(entity,dto);

            }
            catch (Exception e)
            {
                _logger.LogError("Not Found Access Level");
                return null;
            }

        }

        public async Task<AccessLevelDto> Create(CreateAccessLevelDto dto)
        {
            try
            {
                short AccessLevelNo = await _helperService.GetAvailableComponentNoAsync<ArAccessLevel>(32000);
                List<short> ScpIds = _context.ArScps.Select(p => p.ScpId).ToList();
                foreach (var sp in ScpIds)
                {
                    if (!_config.write.AccessLevelConfigurationExtendedCreate(sp, AccessLevelNo, dto.Doors))
                    {
                        _logger.LogError(ConstantsHelper.COMMAND_UNSUCCESS);
                        return null;
                    }
                }

                return await Save(dto, AccessLevelNo); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _logger.LogError(e.Message);
                return null;
            }



        }

        public async Task<AccessLevelDto> Save(CreateAccessLevelDto dto, short ElementNo)
        {
            dto.ElementNo = ElementNo;
            _context.ArAcccessLevels.Add(_mapper.Map<ArAccessLevel>(dto));
            await _context.SaveChangesAsync();
            return _mapper.Map<AccessLevelDto>(await _context.ArAcccessLevels.AsNoTracking().Where(p => p.ComponentNo == ElementNo).FirstOrDefaultAsync());
        }

        public async Task<AccessLevelDto> Remove(short ElementNo)
        {
            try
            {
                var data = await _context.ArAcccessLevels.Where(p => p.ComponentNo == ElementNo).FirstOrDefaultAsync();
                if (data != null) _context.ArAcccessLevels.Remove(data);
                await _context.SaveChangesAsync();
                return _mapper.Map<AccessLevelDto>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _logger.LogError(e.Message);
                return null;
            }
        }


        public int GetAlvlRecAlloc()
        {
            return _context.ArAcccessLevels.Count();
        }


    }
}
