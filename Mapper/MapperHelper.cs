using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Dto;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Dto.Credential;
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
            return dto;
        }

        public static ar_scp ScpRegisDtoToSCP(ScpRegisDto dto)
        {
            ar_scp scp = new ar_scp();
            scp.scp_id = dto.ScpId;
            scp.ip_address = dto.Ip;
            scp.serial_number = dto.SerialNumber;
            scp.name = dto.Name;
            scp.mac = dto.Mac;
            scp.model = dto.Model;
            scp.n_sio = 16;
            scp.n_mp = 615;
            scp.n_cp = 388;
            scp.n_acr = 64;
            scp.n_alvl = 32000;
            scp.n_trgr = 1024;
            scp.n_proc = 1024;
            scp.n_tz = 255;
            scp.n_hol = 255;
            scp.n_mpg = 128;
            return scp;

        }

        public static ScpDto SCPToScpDto(int No,ar_scp scp,short status)
        {
            ScpDto dto = new ScpDto();
            dto.No = No;
            dto.ScpId = scp.scp_id;
            dto.IpAddress = scp.ip_address;
            dto.SerialNumber = scp.serial_number;
            dto.Mac = scp.mac;
            dto.Name = scp.name;
            dto.Model = scp.model;
            dto.Status = status;
            return dto;

        }

        public static EventDto EventToEventDto(event_transction even) 
        {
            EventDto dto = new EventDto();
            dto.Date = even.date;
            dto.Time = even.time;
            //dto.SerialNumber = even.serial_number;
            dto.Source = even.source;
            dto.SourceNumber = even.source_number;
            //dto.Type = even.type;
            dto.Description = even.description;
            dto.Additional = even.additional;
            return dto;

        }

        public static ar_control_point AddCpDtoToCp(AddCpDto cpDto,short cp_number) 
        {
            ar_control_point cp = new ar_control_point();
            cp.name = cpDto.Name;
            cp.sio_number = cpDto.SioNumber;
            cp.cp_number = cp_number;
            cp.op_number = cpDto.OpNumber;
            cp.mode = cpDto.Mode;
            cp.scp_ip = cpDto.ScpIp;
            return cp;
        }

        public static CpDto CpToCpDto(int No,ar_control_point cp,string model_desc,string sio_name)  
        {

        CpDto dto = new CpDto();
            dto.No = No;
            dto.Name = cp.name;
            dto.SioName = sio_name;
            dto.SioModel = model_desc;
            dto.SioNumber = cp.sio_number;
            dto.CpNumber = cp.cp_number;
            dto.OpNumber = cp.op_number;
            dto.Mode = cp.mode == 0 || cp.mode == 16 || cp.mode == 32 ? "Normal" : "Inverted";
            dto.ScpIp = cp.scp_ip;
            return dto;
        }

        public static SioDto SioToSioDto(int No,ar_sio s)
        {
            SioDto dto = new SioDto();
            dto.No = No;
            dto.Name = s.name;
            dto.ScpName = s.scp_name;
            dto.ScpIp = s.scp_ip;
            dto.SioNumber = s.sio_number;
            dto.Model = s.model_desc;
            dto.Address = s.address;
            dto.BaudRate = s.baud_rate;
            dto.ProtoCol = s.n_protocol;
            
            return dto;
        }

        public static OpModeDto OpModeToOpModeDto(ar_op_mode op) 
        {
            OpModeDto dtos = new OpModeDto();
            dtos.Description = op.description;
            dtos.Value = op.value;
            return dtos;
        }

        public static ar_n_cp ACRDtoToACR(AddCpDto dto,short cp_number)
        {
            ar_n_cp cp = new ar_n_cp();
            cp.scp_ip = dto.ScpIp;
            cp.cp_number = cp_number;
            cp.sio_number = dto.SioNumber;
            cp.is_available = false;
            return cp;
        }

        public static AccessLevelDto AccessLevelToAccessLevelDto(ar_access_lv a)
        {
            AccessLevelDto dtos = new AccessLevelDto();
            dtos.Name = a.name;
            dtos.AccessLevelNumber = a.access_lv_number;
            return dtos;
        }

        public static MpDto MpToMpDto(int No,ar_monitor_point mp,string model_desc, string sioName)
        {

            MpDto dtos = new MpDto();
            dtos.No = No;
            dtos.Name = mp.name;
            dtos.SioNumber = mp.sio_number;
            dtos.SioName = sioName;
            dtos.SioModel = model_desc;
            dtos.MpNumber = mp.mp_number;
            dtos.IpNumber = mp.ip_number;
            dtos.Mode = mp.icvt_num == 0 ? "Normally closed" : "Normally open";
            dtos.DelayEntry = mp.delay_entry;
            dtos.DelayExit = mp.delay_exit;
            dtos.ScpIp = mp.scp_ip;
            return dtos;
            
        }

        public static TZDto TzToTzDto(ar_tz tz,int no,string active, string deactive)
        {
            TZDto dto = new TZDto();
            dto.No = no;
            dto.TzNumber = tz.tz_number;
            dto.Intervals = tz.intervals;
            dto.Name = tz.name;
            dto.ActiveDate = active;
            dto.DeactiveDate = deactive;
            dto.Mode = tz.mode;
            return dto;

        }

        public static ACRDto ACRToACRDto(ar_acr data,int no,string sio_name)
        {
            ACRDto dto = new ACRDto();
            dto.No = no;
            dto.Name = data.name;
            dto.AcrNumber = data.acr_number;
            dto.SioNumber = data.rdr_sio;
            dto.SioName = sio_name;
            dto.ScpIp = data.scp_ip;
            dto.AcrMode = data.default_mode;
            dto.AcrModeDesc = Description.GetACRModeForStatus(data.default_mode);
            return dto;
        }

        public static ar_acr ACRDtoToACR(AddACRDto dto,short AcrNo)
        {
           ar_acr data = new ar_acr();
            data.name = dto.Name;
            data.scp_ip = dto.ScpIp;
            data.acr_number = AcrNo;
            data.access_cfg = dto.AccessConfig;
            data.rdr_sio = dto.ReaderSioNumber;
            data.reader_number = dto.ReaderNumber;
            data.strk_sio = dto.StrikeSioNumber;
            data.strk_number = dto.StrikeNumber;
            data.strike_t_min = dto.StrikeMinActiveTime;
            data.strike_t_max = dto.StrikeMaxActiveTime;
            data.strike_mode = dto.StrikeMode;
            data.sensor_sio = dto.SensorSioNumber;
            data.sensor_number = dto.SensorNumber;
            data.dc_held = dto.HeldOpenDelay;
            data.rex1_sio = dto.REX0SioNumber;
            data.rex1_number = dto.REX0Number;
            data.rex2_sio = dto.REX1SioNumber;
            data.rex2_number = dto.REX1Number;
            data.rex1_tzmask = dto.REX0TimeZone;
            data.rex2_tzmask = dto.REX1TimeZone;
            data.altrdr_sio = dto.AlternateReaderSioNumber;
            data.altrdr_number = dto.AlternateReaderNumber;
            data.altrdr_spec = dto.AlternateReaderConfig;
            data.cd_format = dto.CardFormat;
            data.apb_mode = dto.AntiPassbackMode;
            data.offline_mode = dto.OfflineMode;
            data.default_mode = dto.DefaultMode;
            data.default_led_mode = dto.DefaultLEDMode;
            data.pre_alarm = dto.PreAlarm;
            data.apb_delay = dto.AntiPassbackDelay;
            return data;

        }

        public static AcsRdrModeDto AcsRdrModeToAcsRdrModeDto(ar_rdr_cfg_mode d)
        {
            AcsRdrModeDto dto = new AcsRdrModeDto();
            dto.Name = d.name;
            dto.Description = d.description;
            dto.Value = d.value;
            return dto;
        }

        public static StrikeModeDto StrikeModeToStrikeModeDto(ar_strk_mode d)
        {
            StrikeModeDto dto = new StrikeModeDto();
            dto.Name = d.name;
            dto.Description = d.description;
            dto.Value = d.value;
            return dto;
        }

        public static ACRModeDto ACRModeToACRModeDto(ar_acr_mode d)
        {
            ACRModeDto dto = new ACRModeDto();
            dto.Name = d.name;
            dto.Description = d.description;
            dto.Value = d.value;
            return dto;
        }

        public static ApbModeDto ApbModeToApbModeDto(ar_apb_mode d)
        {
            ApbModeDto dto = new ApbModeDto();
            dto.Name = d.name;
            dto.Description = d.description;
            dto.Value = d.value;
            return dto;
        }

        public static ar_n_acr ACRDtoTonACR(AddACRDto dto, short acrNo)
        {
            ar_n_acr cp = new ar_n_acr();
            cp.scp_ip = dto.ScpIp;
            cp.acr_number = acrNo;
            cp.sio_number = dto.ReaderSioNumber;
            cp.is_available = false;
            return cp;
        }

        public static ar_n_cp AddCpDtoTonCp(AddCpDto cpDto, short cp_number)
        {
            ar_n_cp cp = new ar_n_cp();
            cp.scp_ip = cpDto.ScpIp;
            cp.cp_number = cp_number;
            cp.sio_number = cpDto.SioNumber;
            cp.is_available = false;
            return cp;
        }

        public static IpModeDto IpModeToIpModeDto(ar_ip_mode data)
        {
            IpModeDto dto = new IpModeDto();
            dto.Description = data.description;
            dto.Value = data.value;
            dto.Name = data.name;
            return dto;
        }

        public static ar_card_holder CreateCardHolderDtoToCardHolder(CreateCardHolderDto dto)
        {
            ar_card_holder data = new ar_card_holder();
            data.card_holder_id = dto.CardHolderId;
            data.card_holder_refenrence_number = Guid.NewGuid();
            data.title = dto.Title;
            data.name = dto.Name;
            data.email = dto.Email;
            data.phone = dto.Phone;
            data.description = dto.Description;
            data.holder_status = "Active";
            data.sex = dto.Sex;
            data.created_date = DateTime.Now;
            data.updated_date = DateTime.Now;

            return data;
        }

        public static ar_credentials CreateCredentialDtoToCreateCredential(CreateCredentialDto dto)
        {
            ar_credentials data = new ar_credentials();
            data.card_holder_refenrence_number = dto.CardHolderReferenceNumber;
            data.bits = dto.Bits;
            data.issue_code = dto.IssueCode;
            data.facility_code = dto.FacilityCode;
            data.card_number = dto.CardNumber;
            data.pin = dto.Pin;
            data.act_time = dto.ActiveDate;
            data.deact_time = dto.DeactiveDate;
            data.access_level = dto.AccessLevel;
            data.image = dto.Image;
            data.created_date = DateTime.Now;
            data.updated_date = DateTime.Now;
            return data;
        }

        public static CardHolderDto CardHolderToCardHolderDto(ar_card_holder d)
        {
            CardHolderDto dto = new CardHolderDto();
            dto.CardHolderId = d.card_holder_id;
            dto.CardHolderReferenceNumber = d.card_holder_refenrence_number;
            dto.Title = d.title;
            dto.Name = d.name;
            dto.Sex = d.sex;
            dto.Email = d.email;
            dto.Phone = d.phone;
            dto.Description = d.description;
            dto.HolderStatus = d.holder_status;
            dto.IssueCodeRunningNumber = d.issue_code_running_number;
            return dto;
        }

        public static ar_access_lv AccessLevelDtoToAccessLevel(CreateAccessLevelDto dto,short alvlNumber)
        {
            ar_access_lv data = new ar_access_lv();
            data.name = dto.Name;
            data.access_lv_number = alvlNumber;
            foreach(var d in dto.Doors)
            {
                var prop = typeof(ar_access_lv).GetProperty($"tz_acr{d.AcrNumber}");
                if (prop != null && prop.CanWrite)
                    prop.SetValue(data,d.TzNumber);
            }
            return data;
        }
    }
}
