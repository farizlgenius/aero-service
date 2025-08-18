
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto.Credential;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace HIDAeroService.Service
{
    public class CredentialService
    {
        private readonly AppConfigData _config;
        private readonly HelperService _helperService;
        private readonly IHubContext<CredentialHub> _hub;
        private readonly AppDbContext _context;
        private readonly ILogger<CredentialService> _logger;
        public CredentialService(AppConfigData config,HelperService helperService,IHubContext<CredentialHub> hub,AppDbContext context,ILogger<CredentialService> logger) 
        {
            _logger = logger;
            _context = context;
            _hub = hub;
            _helperService = helperService;
            _config = config;
        }

        public bool ScanCardTrigger(ScanCardDto dto)
        {
            short ScpId = _helperService.GetScpIdFromIp(dto.ScpIp);
            _config.read.isWaitingCardScan = true;
            _config.read.ScanScpId = ScpId;
            _config.read.ScanAcrNo = dto.AcrNo;
            return true;
        }

        public void TriggerCardScan(int ScpId, short FormatNumber,int FacilityCode,double CardHolderId, int IssueCode,short FloorNumber)
        {
            string ScpIp = _helperService.GetScpIpFromId((short)ScpId);
            _hub.Clients.All.SendAsync("CardScanStatus", ScpIp, FormatNumber, FacilityCode, CardHolderId, IssueCode, FloorNumber);
            _config.read.isWaitingCardScan = false;
        }

        public bool CreateCardHolder(CreateCardHolderDto dto)
        {
            if(!_context.ar_card_holders.Any(p => p.card_holder_refenrence_number == dto.CardHolderReferenceNumber))
            {
                if (!SaveCardHolderToDatabase(dto))
                {
                    return false;
                }
            }

            int? issue = _context.ar_card_holders.Where(p => p.card_holder_refenrence_number == dto.CardHolderReferenceNumber).Select(p => p.issue_code_running_number).FirstOrDefault();
            if (issue == null) issue = 0;
            issue += 1;
            foreach (var card in dto.Cards)
            {
                if(!_context.ar_credentials.Any(p => p.facility_code == card.FacilityCode && p.card_number == card.CardNumber))
                {
                    card.IssueCode = (int)issue;
                    CreateCredential(card);
                }
                issue++;
            } 

            return true;

        }

        public bool SaveCardHolderToDatabase(CreateCardHolderDto dto)
        {
            try
            {
                _context.ar_card_holders.Add(MapperHelper.CreateCardHolderDtoToCardHolder(dto));
                _context.SaveChanges();
                return true;

            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public bool CreateCredential(CreateCredentialDto dto)
        {
            try
            {
                List<int> ErrorId = new List<int>();
                List<short> ScpIdList = _context.ar_scps.Select(p => p.scp_id).ToList();
                foreach (var id in ScpIdList)
                {
                    if (_config.write.CheckSCPStatus(id) == 1)
                    {

                        if (!_config.write.AccessDatabaseCardRecord(id, 0x01, dto.CardNumber, dto.IssueCode, dto.Pin, [dto.AccessLevel], dto.ActiveDate, dto.DeactiveDate))
                        {
                            _logger.LogError("Create Credential Fail");
                            Console.WriteLine("Create Credential Fail");
                            ErrorId.Add(dto.id);
                        }

                    }

                    if (!SaveCredentialToDatabase(dto,ErrorId))
                    {
                        _logger.LogError("Fail Save Create Credential");
                        Console.WriteLine("Fail Save Create Credential");
                        return false;
                    }


                }
                _context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool SaveCredentialToDatabase(CreateCredentialDto dto,List<int> ErrorId) 
        {
            try
            {
                if (!ErrorId.Contains(dto.id))
                {
                    _context.ar_credentials.Add(MapperHelper.CreateCredentialDtoToCreateCredential(dto));
                }
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<CardHolderDto> GetCardHolderList()
        {
            try
            {
                List<CardHolderDto> dto = new List<CardHolderDto>();
                foreach (var d in _context.ar_card_holders.ToList())
                {
                    dto.Add(MapperHelper.CardHolderToCardHolderDto(d));
                }
                return dto;
            } catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                return new List<CardHolderDto>();
            }

        }

        public int GetCredentialRecAlloc()
        {
            return _context.ar_credentials.Count();
        }

        public int GetActiveCredentialRecAlloc()
        {
            return _context.ar_credentials.Where(p => p.is_active == true).Count();
        }
    }
}
