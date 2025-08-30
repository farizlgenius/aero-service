using AutoMapper;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto.CardFormat;
using HIDAeroService.Entity;
using HIDAeroService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MiNET.Entities;

namespace HIDAeroService.Service.Impl
{

    public class CardFormatService : ICardFormatService
    {
        private readonly ILogger<CardFormatService> _logger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly AeroLibMiddleware _config;
        private readonly AppConfigSettings _configSettings;

        public CardFormatService(AppDbContext context, IMapper mapper, ILogger<CardFormatService> logger,AeroLibMiddleware config,IOptions<AppConfigSettings> configSettings)
        {
            _configSettings = configSettings.Value;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CardFormatDto> Add(CreateCardFormatDto dto)
        {
            try
            {
                var _maxCardFormat = _configSettings.MaxCardFormat;
                if (_maxCardFormat == 0) _maxCardFormat = 7;
                List<short> _errorScpList = new List<short>();
                List<short> ScpIds = await _context.ArScps.Select(p => p.ScpId).ToListAsync();
                short _cardFormatNo = 0;


                // Check that there're available 0-7 or not
                if (_context.ArCardFormats.Any(p => p.IsActive == false))
                {
                    _cardFormatNo = await _context.ArCardFormats.Where(p => p.IsActive == false).Select(p => p.ComponentNo).FirstOrDefaultAsync();
                }
                else 
                {
                    _cardFormatNo = _context.ArCardFormats.Max(p => p.ComponentNo);
                    if (_cardFormatNo > _maxCardFormat) return null;
                    _cardFormatNo++;
                }


                foreach (var id in ScpIds)
                {
                    if (!_config.write.CardFormatterConfiguration(id, _cardFormatNo, dto.Facility, 0, 1, 0, dto.Bits, dto.PeLn, dto.PeLoc, dto.PoLn, dto.PoLoc, dto.FcLn, dto.FcLoc, dto.ChLn, dto.ChLoc, dto.IcLn, dto.IcLoc))
                    {
                        _errorScpList.Add(id);
                    }

                }

                if(_errorScpList.Count > 0) return null;

                if (!await Save(dto,_cardFormatNo))
                {
                    _logger.LogError("Can't Save to DB");
                    return null;
                }

                var data = _context.ArCardFormats.FirstOrDefaultAsync(p => p.ComponentNo == _cardFormatNo);
                if (data == null) return null;
                return _mapper.Map<CardFormatDto>(data);

            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<CardFormatDto> Delete(short cardFormatNo)
        {
            try
            {
                List<short> _errorScpList = new List<short>();
                List<short> ScpIds = await _context.ArScps.Select(p => p.ScpId).ToListAsync();
                var entity = await _context.ArCardFormats.Where(p => p.ComponentNo == cardFormatNo).FirstOrDefaultAsync();
                entity.FunctionId = 0;

                foreach (var id in ScpIds)
                {
                    if (!_config.write.CardFormatterConfiguration(id, entity.ComponentNo, entity.Facility, 0, entity.FunctionId, 0, entity.Bits, entity.PeLn, entity.PeLoc, entity.PoLn, entity.PoLoc, entity.FcLn, entity.FcLoc, entity.ChLn, entity.ChLoc, entity.IcLn, entity.IcLoc))
                    {
                        _errorScpList.Add(id);
                    }

                }

                if (_errorScpList.Count > 0) return null;

                entity.IsActive = false;
                await _context.SaveChangesAsync();
                return _mapper.Map<CardFormatDto>(entity);
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<CardFormatDto>> GetAll()
        {
            try
            {
                List<CardFormatDto> data = new List<CardFormatDto>();
                var entity = await _context.ArCardFormats.Where(p => p.IsActive == true).ToArrayAsync();
                foreach (var e in entity)
                {
                    data.Add(_mapper.Map<CardFormatDto>(e));
                }
                return data;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.Message);
                return Enumerable.Empty<CardFormatDto>();
            }

        }

        public IEnumerable<ArCardFormat> GetAllSetting()
        {
            try
            {
                return _context.ArCardFormats.Where(p => p.IsActive == true).ToArray();
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.Message);
                return Enumerable.Empty<ArCardFormat>();
            }
        }


        public async Task<bool> Save(CreateCardFormatDto dto,short ElementNo)
        {
            try
            {
                if(_context.ArCardFormats.Any(p => p.ComponentNo == ElementNo))
                {
                    var entity = await _context.ArCardFormats.FirstOrDefaultAsync(p => p.ComponentNo == ElementNo);
                    entity = _mapper.Map(dto,entity);
                    entity.IsActive = true;
                    entity.FunctionId = 1;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var entity = _mapper.Map<ArCardFormat>(dto);
                    entity.ComponentNo = ElementNo;
                    entity.IsActive = true;
                    entity.FunctionId = 1;
                    _context.ArCardFormats.Add(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

    }
}
