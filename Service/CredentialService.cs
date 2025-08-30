
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
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
        private readonly AeroLibMiddleware _config;
        private readonly HelperService _helperService;
        private readonly IHubContext<CredentialHub> _hub;
        private readonly AppDbContext _context;
        private readonly ILogger<CredentialService> _logger;
        public CredentialService(AeroLibMiddleware config,HelperService helperService,IHubContext<CredentialHub> hub,AppDbContext context,ILogger<CredentialService> logger) 
        {
            _logger = logger;
            _context = context;
            _hub = hub;
            _helperService = helperService;
            _config = config;
        }

        public bool ScanCardTrigger(ScanCardDto dto)
        {
            short ScpId = _helperService.GetScpIdFromMac(dto.ScpMac);
            _config.read.isWaitingCardScan = true;
            _config.read.ScanScpId = ScpId;
            _config.read.ScanAcrNo = dto.AcrNo;
            return true;
        }

        public void TriggerCardScan(int ScpId, short FormatNumber,int FacilityCode,double CardHolderId, int IssueCode,short FloorNumber)
        {
            string ScpMac = _helperService.GetMacFromId((short)ScpId);
            _hub.Clients.All.SendAsync("CardScanStatus", ScpMac, FormatNumber, FacilityCode, CardHolderId, IssueCode, FloorNumber);
            _config.read.isWaitingCardScan = false;
        }

        public bool CreateCardHolder(CreateCardHolderDto dto)
        {
            string _unique;
            if (!_context.ArCardHolders.Any(p => p.CardHolderRefNo.Equals(dto.CardHolderReferenceNumber)))
            {
                _unique = Guid.NewGuid().ToString();
                dto.CardHolderReferenceNumber = _unique;
                if (!SaveCardHolderToDatabase(dto))
                {
                    return false;
                }
            }
            else
            {
                _unique = dto.CardHolderReferenceNumber;
            }

            int? issue = _context.ArCardHolders.Where(p => p.CardHolderRefNo.Equals(dto.CardHolderReferenceNumber)).Select(p => p.IssueCodeRunningNo).FirstOrDefault();
            if (issue == null) issue = 0;
            issue += 1;
            foreach (var card in dto.Cards)
            {
                if(!_context.ArCredentials.Any(p => p.FacilityCode == card.FacilityCode && p.CardNo == card.CardNumber))
                {
                    card.IssueCode = (int)issue;
                    CreateCredential(card,_unique);
                }
                issue++;
            } 

            return true;

        }

        public bool SaveCardHolderToDatabase(CreateCardHolderDto dto)
        {
            try
            {
                _context.ArCardHolders.Add(MapperHelper.CreateCardHolderDtoToCardHolder(dto));
                _context.SaveChanges();
                return true;

            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public bool CreateCredential(CreateCredentialDto dto,string cardHolderReferenceNumber)
        {
            try
            {
                List<int> ErrorId = new List<int>();
                List<short> ScpIdList = _context.ArScps.Select(p => p.ScpId).ToList();
                foreach (var id in ScpIdList)
                {
                    if (_config.write.CheckSCPStatus(id) == 1)
                    {
                        long activeDateInSecond = _helperService.DateTimeToElapeSecond(dto.ActiveDate);
                        long deactiveDateInSecond = _helperService.DateTimeToElapeSecond(dto.DeactiveDate);
                        if (!_config.write.AccessDatabaseCardRecord(id, 0x01, dto.CardNumber, dto.IssueCode, dto.Pin, [dto.AccessLevel],(int)activeDateInSecond,(int)deactiveDateInSecond))
                        {
                            _logger.LogError("Create Credential Fail");
                            Console.WriteLine("Create Credential Fail");
                            ErrorId.Add(dto.Id);
                        }

                    }

                    if (!SaveCredentialToDatabase(dto,ErrorId, cardHolderReferenceNumber))
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

        public bool SaveCredentialToDatabase(CreateCredentialDto dto,List<int> ErrorId, string cardHolderReferenceNumber) 
        {
            try
            {
                if (!ErrorId.Contains(dto.Id))
                {
                    _context.ArCredentials.Add(MapperHelper.CreateCredentialDtoToCreateCredential(dto, cardHolderReferenceNumber));
                    _context.SaveChanges();
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
                int i = 1;
                foreach (var d in _context.ArCardHolders.ToList())
                {
                    dto.Add(MapperHelper.CardHolderToCardHolderDto(d,i));
                    i++;
                }
                return dto;
            } catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                return new List<CardHolderDto>();
            }

        }

        public string RemoveCardHolder(string reference)
        {
            try
            {
                List<ArScp> scps = _context.ArScps.ToList();
                ArCardHolder holder = _context.ArCardHolders.FirstOrDefault(p => p.CardHolderRefNo.Equals(reference));
                List<ArCredential> cards = _context.ArCredentials.Where(p => p.CardHolderRefNo.Equals(reference)).ToList();
                foreach (var s in scps) 
                {
                    foreach(var c in cards)
                    {
                        if (_config.write.CardDelete(s.ScpId,c.CardNo))
                        {
                            _context.ArCredentials.Remove(c);
                        }
                        Console.WriteLine("Command Fail Delete Card");
                    }
                }
                if(holder != null)_context.ArCardHolders.Remove(holder);
                _context.SaveChanges();
                return Constants.ConstantsHelper.COMMAND_SUCCESS;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                return Constants.ConstantsHelper.COMMAND_UNSUCCESS;
            }
        }

        public int GetCredentialRecAlloc()
        {
            return _context.ArCredentials.Count();
        }

        public int GetActiveCredentialRecAlloc()
        {
            return _context.ArCredentials.Where(p => p.IsActive == true).Count();
        }
    }
}
