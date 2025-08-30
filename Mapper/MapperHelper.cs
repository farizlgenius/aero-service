using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Dto;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Dto.Acr;
using HIDAeroService.Dto.Cp;
using HIDAeroService.Dto.Credential;
using HIDAeroService.Dto.Mp;
using HIDAeroService.Dto.Scp;
using HIDAeroService.Dto.Sio;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;
using HIDAeroService.Models;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Identity;
using MiNET.Entities;
using Newtonsoft.Json.Linq;
using System.Transactions;

namespace HIDAeroService.Mapper
{
    public class MapperHelper
    {

        public static IDReportDto IDReportToIDReportDto(IDReport idReport)
        {
            IDReportDto dto = new IDReportDto();
            dto.ConfigFlag = idReport.ConfigFlag;
            dto.DeviceID = idReport.DeviceID;
            dto.MacAddress = idReport.MacAddress;
            dto.SerialNumber = idReport.SerialNumber;
            dto.ScpID = idReport.ScpID;
            dto.Model = "HID Aero x1100";
            dto.Ip = Utility.IntegerToIp(idReport.Ip);
            dto.Port = (short)idReport.Port;
            return dto;
        }

        public static ArScp ScpRegisDtoToSCP(ScpRegisDto dto)
        {
            ArScp scp = new ArScp();
            scp.ScpId = dto.ScpId;
            scp.Ip = dto.Ip;
            scp.SerialNumber = dto.SerialNumber;
            scp.Name = dto.Name;
            scp.Mac = dto.Mac;
            scp.Model = dto.Model;
            scp.NSio = 16;
            scp.NMp = 615;
            scp.NCp = 388;
            scp.NAcr = 64;
            scp.NAlvl = 32000;
            scp.Ntrgr = 1024;
            scp.Nproc = 1024;
            scp.NTz = 255;
            scp.NHol = 255;
            scp.NMpg = 128;
            scp.Port = dto.Port;
            scp.LastSync = DateTime.Now;
            scp.CreatedDate = DateTime.Now;
            scp.UpdatedDate = DateTime.Now;
            return scp;

        }

        public static ScpDto SCPToScpDto(int No, ArScp scp,short status)
        {
            ScpDto dto = new ScpDto();
            dto.No = No;
            dto.ScpId = scp.ScpId;
            dto.IpAddress = scp.Ip;
            dto.SerialNumber = scp.SerialNumber;
            dto.Mac = scp.Mac;
            dto.Name = scp.Name;
            dto.Model = scp.Model;
            dto.Status = status;
            dto.IsReset = scp.IsReset;
            dto.IsUpload = scp.IsUpload;
            return dto;

        }

        public static EventDto EventToEventDto(ArEvent even) 
        {
            EventDto dto = new EventDto();
            dto.Date = even.Date;
            dto.Time = even.Time;
            //dto.SerialNumber = even.serial_number;
            dto.Source = even.Source;
            dto.SourceNumber = even.SourceNo;
            //dto.Type = even.type;
            dto.Description = even.Description;
            dto.Additional = even.Additional;
            return dto;

        }

        public static ArControlPoint AddCpDtoToCp(AddCpDto cpDto,short cp_number) 
        {
            ArControlPoint cp = new ArControlPoint();
            cp.Name = cpDto.Name;
            cp.SioNo = cpDto.SioNumber;
            cp.CpNo = cp_number;
            cp.OpNo = cpDto.OpNumber;
            cp.Mode = cpDto.Mode;
            cp.ScpMac = cpDto.ScpMac;
            cp.DefaultPulseTime = cpDto.DefaultPulseTime;
            cp.CreatedDate = DateTime.Now;
            cp.UpdatedDate = DateTime.Now;
            return cp;
        }

        public static CpDto CpToCpDto(int No,ArControlPoint cp,string model_desc,string sio_Name)  
        {

            CpDto dto = new CpDto();
            dto.No = No;
            dto.Name = cp.Name;
            dto.SioName = sio_Name;
            dto.SioModel = model_desc;
            dto.SioNumber = cp.SioNo;
            dto.CpNumber = cp.CpNo;
            dto.OpNumber = cp.OpNo;
            dto.Mode = cp.Mode == 0 || cp.Mode == 16 || cp.Mode == 32 ? "Normal" : "Inverted";
            dto.ScpMac = cp.ScpMac;
            return dto;
        }

        public static SioDto SioToSioDto(int No,ArSio s)
        {
            SioDto dto = new SioDto();
            dto.No = No;
            dto.Name = s.Name;
            dto.ScpName = s.ScpName;
            dto.ScpMac = s.ScpMac;
            dto.SioNumber = s.SioNumber;
            dto.Model = s.ModeDescription;
            dto.Address = s.Address;
            dto.BaudRate = s.BaudRate;
            dto.ProtoCol = s.NProtocol;
            
            return dto;
        }

        public static OpModeDto OpModeToOpModeDto(ArOpMode op) 
        {
            OpModeDto dtos = new OpModeDto();
            dtos.Description = op.Description;
            dtos.Value = op.Value;
            return dtos;
        }

        public static ArCpNo ACRDtoToACR(AddCpDto dto,short cp_number)
        {
            ArCpNo cp = new ArCpNo();
            cp.ScpMac = dto.ScpMac;
            cp.CpNo = cp_number;
            cp.SioNo = dto.SioNumber;
            cp.IsAvailable = false;
            return cp;
        }

        public static AccessLevelDto AccessLevelToAccessLevelDto(ArAccessLevel a)
        {
            AccessLevelDto dtos = new AccessLevelDto();
            dtos.Name = a.Name;
            dtos.ElementNo = a.ComponentNo;
            return dtos;
        }

        public static MpDto MpToMpDto(int No,ArMonitorPoint mp,string model_desc, string sioName)
        {

            MpDto dtos = new MpDto();
            dtos.No = No;
            dtos.Name = mp.Name;
            dtos.SioNumber = mp.SioNo;
            dtos.SioName = sioName;
            dtos.SioModel = model_desc;
            dtos.MpNumber = mp.MpNo;
            dtos.IpNumber = mp.IpNo;
            dtos.Mode = mp.IcvtNo == 0 ? "Normally closed" : "Normally open";
            dtos.DelayEntry = mp.DelayEntry;
            dtos.DelayExit = mp.DelayExit;
            dtos.ScpMac = mp.ScpMac;
            return dtos;
            
        }



        public static AcrDto ACRToACRDto(ArAcr data,int no,string sio_Name)
        {
            AcrDto dto = new AcrDto();
            dto.No = no;
            dto.Name = data.Name;
            dto.AcrNo = data.AcrNo;
            dto.SioNumber = data.RdrSio;
            dto.SioName = sio_Name;
            dto.ScpMac = data.ScpMac;
            dto.AcrMode = data.DoorMode;
            dto.AcrModeDesc = Description.GetACRModeForStatus(data.DoorMode);
            dto.AcrDefaultMode = data.DefaultMode;
            dto.AcrDefaultModeDesc = Description.GetACRModeForStatus(data.DefaultMode);
            return dto;
        }

        public static ArAcr ACRDtoToACR(AddAcrDto dto,short AcrNo)
        {
           ArAcr data = new ArAcr();
            data.Name = dto.Name;
            data.ScpMac = dto.ScpMac;
            data.AcrNo = AcrNo;
            data.AccessCfg = dto.AccessConfig;
            data.RdrSio = dto.ReaderSioNumber;
            data.ReaderNo = dto.ReaderNumber;
            data.StrkSio = dto.StrikeSioNumber;
            data.StrkNo = dto.StrikeNumber;
            data.StrkMin = dto.StrikeMinActiveTime;
            data.StrkMax = dto.StrikeMaxActiveTime;
            data.StrkMode = dto.StrikeMode;
            data.SensorSio = dto.SensorSioNumber;
            data.SensorNo = dto.SensorNumber;
            data.DcHeld = dto.HeldOpenDelay;
            data.Rex1Sio = dto.REX0SioNumber;
            data.Rex1No = dto.REX0Number;
            data.Rex2Sio = dto.REX1SioNumber;
            data.Rex2No = dto.REX1Number;
            data.Rex1TzMask = dto.REX0TimeZone;
            data.Rex2TzMask = dto.REX1TimeZone;
            data.AlternateReaderSio = dto.AlternateReaderSioNumber;
            data.AlternateReaderNo = dto.AlternateReaderNumber;
            data.AlternateReaderSpec = dto.AlternateReaderConfig;
            data.CdFormat = dto.CardFormat;
            data.ApbMode = dto.AntiPassbackMode;
            data.OfflineMode = dto.OfflineMode;
            data.DefaultMode = dto.DefaultMode;
            data.DoorMode = dto.DefaultMode;
            data.DefaultLEDMode = dto.DefaultLEDMode;
            data.PreAlarm = dto.PreAlarm;
            data.ApbDelay = dto.AntiPassbackDelay;
            data.UpdatedDate = DateTime.Now;
            data.CreatedDate = DateTime.Now;    
            return data;

        }

        public static AcsRdrModeDto AcsRdrModeToAcsRdrModeDto(ArReaderConfigMode d)
        {
            AcsRdrModeDto dto = new AcsRdrModeDto();
            dto.Name = d.Name;
            dto.Description = d.Description;
            dto.Value = d.Value;
            return dto;
        }

        public static StrikeModeDto StrikeModeToStrikeModeDto(ArStrkMode d)
        {
            StrikeModeDto dto = new StrikeModeDto();
            dto.Name = d.Name;
            dto.Description = d.Description;
            dto.Value = d.Value;
            return dto;
        }

        public static AcrModeDto ACRModeToACRModeDto(ArAcrMode d)
        {
            AcrModeDto dto = new AcrModeDto();
            dto.Name = d.Name;
            dto.Description = d.Description;
            dto.Value = d.Value;
            return dto;
        }

        public static ApbModeDto ApbModeToApbModeDto(ArApbMode d)
        {
            ApbModeDto dto = new ApbModeDto();
            dto.Name = d.Name;
            dto.Description = d.Description;
            dto.Value = d.Value;
            return dto;
        }

        public static ArAcrNo ACRDtoTonACR(AddAcrDto dto, short acrNo)
        {
            ArAcrNo cp = new ArAcrNo();
            cp.ScpMac = dto.ScpMac;
            cp.AcrNo = acrNo;
            cp.SioNo = dto.ReaderSioNumber;
            cp.IsAvailable = false;
            return cp;
        }

        public static ArCpNo AddCpDtoTonCp(AddCpDto cpDto, short cp_number)
        {
            ArCpNo cp = new ArCpNo();
            cp.ScpMac = cpDto.ScpMac;
            cp.CpNo = cp_number;
            cp.SioNo = cpDto.SioNumber;
            cp.IsAvailable = false;
            return cp;
        }

        public static IpModeDto IpModeToIpModeDto(ArIpMode data)
        {
            IpModeDto dto = new IpModeDto();
            dto.Description = data.Description;
            dto.Value = data.Value;
            dto.Name = data.Name;
            return dto;
        }

        public static ArCardHolder CreateCardHolderDtoToCardHolder(CreateCardHolderDto dto)
        {
            ArCardHolder data = new ArCardHolder();
            data.CardHolderId = dto.CardHolderId;
            data.CardHolderRefNo = dto.CardHolderReferenceNumber;
            data.Title = dto.Title;
            data.FirstName = dto.FirstName;
            data.MiddleName = dto.MiddleName;
            data.LastName = dto.LastName;
            data.Email = dto.Email;
            data.Phone = dto.Phone;
            data.Description = dto.Description;
            data.HolderStatus = "Active";
            data.Sex = dto.Sex;
            data.CreatedDate = DateTime.Now;
            data.UpdatedDate = DateTime.Now;

            return data;
        }

        public static ArCredential CreateCredentialDtoToCreateCredential(CreateCredentialDto dto, string cardHolderReferenceNumber)
        {
            ArCredential data = new ArCredential();
            data.CardHolderRefNo = cardHolderReferenceNumber;
            data.Bits = dto.Bits;
            data.IssueCode = dto.IssueCode;
            data.FacilityCode = dto.FacilityCode;
            data.CardNo = dto.CardNumber;
            data.Pin = dto.Pin;
            data.ActTime = dto.ActiveDate;
            data.DeactTime = dto.DeactiveDate;
            data.AccessLevel = dto.AccessLevel;
            data.Image = dto.Image;
            data.IsActive = true;
            data.CreatedDate = DateTime.Now;
            data.UpdatedDate = DateTime.Now;
            return data;
        }

        public static CardHolderDto CardHolderToCardHolderDto(ArCardHolder d,int i)
        {
            CardHolderDto dto = new CardHolderDto();
            dto.No = i;
            dto.CardHolderId = d.CardHolderId;
            dto.CardHolderReferenceNumber = d.CardHolderRefNo;
            dto.Title = d.Title;
            dto.FirstName = d.FirstName;
            dto.MiddleName = d.MiddleName;
            dto.LastName = d.LastName;
            dto.Sex = d.Sex;
            dto.Email = d.Email;
            dto.Phone = d.Phone;
            dto.Description = d.Description;
            dto.HolderStatus = d.HolderStatus;
            dto.IssueCodeRunningNumber = d.IssueCodeRunningNo;
            return dto;
        }

    }
}
