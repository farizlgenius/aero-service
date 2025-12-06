using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Transactions;
using HIDAeroService.Entity;
using HIDAeroService.Enums;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Utils;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace HIDAeroService.Service.Impl
{
    public sealed class TransactionService(AppDbContext context, IHubContext<AeroHub> hub,AeroCommand command,IHelperService helperService,ILogger<TransactionService> logger) : ITransactionService
    {
        public async Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAsync(PaginationParams param)
        {
            var query = context.Transactions.AsQueryable();
            var count = await query.CountAsync();
            var data = await query
                .AsNoTracking()
                .Include(x => x.TransactionFlags)
                .OrderByDescending(t => t.CreatedDate)
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(x => MapperHelper.TransactionToDto(x))
                .ToListAsync();


            return ResponseHelper.SuccessBuilder<PaginationDto>(new PaginationDto 
            {
                Data = data,
                Page = new PaginationData
                {
                    TotalCount = count,
                    PageNumber = param.PageNumber,
                    PageSize = param.PageSize,
                    TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
                }
            });
        }

        public void TriggerEventRecieve()
        {
            hub.Clients.All.SendAsync("Transaction");
        }

        public async Task SaveToDatabase(SCPReplyMessage message)
        {       
            try
            {
                if (!await context.Hardwares.AnyAsync(s => s.ComponentId == (short)message.SCPId)) return;
                HIDAeroService.Entity.Transaction tran = new HIDAeroService.Entity.Transaction();
                switch (message.tran.source_type)
                {

                    case (short)tranSrc.tranSrcScpDiag:
                        switch (message.tran.tran_type)
                        {
                            case (short)tranType.tranTypeSys:
                                tran = await TransactionBuilder(message);
                                tran.Origin = await context.Hardwares.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.ComponentId == (short)message.SCPId).Select(s => s.Name).FirstOrDefaultAsync() ?? "";
                                if (message.tran.tran_code == 1)
                                {
                                    tran.TransactionFlags = TransactionHelper.TypeSysErrorFlag(message.tran.sys_comm.error_code);
                                }
                                break;
                            case (short)tranType.tranTypeWebActivity:
                                tran = await TransactionBuilder(message);
                                tran.Origin = UtilityHelper.IntegerToIp(message.tran.web_activity.ipAddress);
                                tran.Actor = new string(message.tran.web_activity.szObjectUser);
                                break;
                            case (short)tranType.tranTypeFileDownloadStatus:
                                tran = await TransactionBuilder(message);
                                tran.Remark = $"Type: {TransactionHelper.FileTypeToText(message.tran.file_download.fileType)} ,Name: {new string(message.tran.file_download.fileName)}";
                                break;
                            default:
                                break;
                        }
                        break;
                    case (short)tranSrc.tranSrcScpLcl:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.Hardwares.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.ComponentId == (short)message.SCPId).Select(s => s.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcScpLcl);
                        break;
                    case (short)tranSrc.tranSrcSioDiag:
                        tran = await TransactionBuilder(message);
                        var mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Modules.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Model).FirstOrDefaultAsync() ?? "";
                        break;
                    case (short)tranSrc.tranSrcSioCom:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Modules.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Model).FirstOrDefaultAsync() ?? "";
                        //tran.TypeSioComm.CommSts = TransactionHelper.TypeSioCommStatus(message.tran.s_comm.comm_sts);
                        //tran.TypeSioComm.Model = TransactionHelper.TypeSioCommModel(message.tran.s_comm.model, message.tran.s_comm.comm_sts);
                        //tran.TypeSioComm.Revision = message.tran.s_comm.revision;
                        //tran.TypeSioComm.Serial = message.tran.s_comm.ser_num;
                        //tran.TypeSioComm.nExtendedInfoValid = message.tran.s_comm.nExtendedInfoValid;
                        //if(message.tran.s_comm.nExtendedInfoValid != 0)
                        //{
                        //    tran.TypeSioComm.Hardware = TransactionHelper.TypeSioCommHardwareId(message.tran.s_comm.nHardwareId);
                        //    tran.TypeSioComm.nHardwareRev = message.tran.s_comm.nHardwareRev;
                        //    tran.TypeSioComm.nProductId = message.tran.s_comm.nProductId;
                        //    tran.TypeSioComm.nProductVer = message.tran.s_comm.nProductVer;
                        //    tran.TypeSioComm.nFirmwareBoot = message.tran.s_comm.nFirmwareBoot;
                        //    tran.TypeSioComm.nFirmwareLdr = message.tran.s_comm.nFirmwareLdr;
                        //    tran.TypeSioComm.nFirmwareApp = message.tran.s_comm.nFirmwareApp;
                        //    tran.TypeSioComm.nOemCode = message.tran.s_comm.nOemCode;
                        //    tran.TypeSioComm.nEncConfig = TransactionHelper.TypeSioCommEncConfig(message.tran.s_comm.nEncConfig);
                        //    tran.TypeSioComm.nKeyStatus = TransactionHelper.TypeSioCommEncKeyStatus(message.tran.s_comm.nEncKeyStatus);
                        //    tran.TypeSioComm.nHardwareComponents = message.tran.s_comm.nHardwareComponents;
                        //    tran.TypeSioComm.TransactionFlags = TransactionHelper.TypeSioHardwareComponent(message.tran.s_comm.nHardwareComponents);

                        //}
                        break;
                    case (short)tranSrc.tranSrcSioTmpr:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Modules.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Model).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioTmpr);
                        break;
                    case (short)tranSrc.tranSrcSioPwr:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Modules.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Model).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioPwr);
                        break;
                    case (short)tranSrc.tranSrcMP:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.MonitorPoints.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcMP);
                        break;
                    case (short)tranSrc.tranSrcCP:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.ControlPoints.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcCP);
                        break;
                    case (short)tranSrc.tranSrcACR:
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        switch (message.tran.tran_type)
                        {

                            case (short)tranType.tranTypeCardBin:
                                tran = await TransactionBuilder(message);
                                tran.Remark = $"Bits Count: {message.tran.c_bin.bit_count}";
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCardBcd:
                                tran = await TransactionBuilder(message);
                                tran.Remark = $"Digits Count: {message.tran.c_bin.bit_count}";
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCardFull:
                                tran = await TransactionBuilder(message);
                                var holder = await context.Credentials.AsNoTracking().Include(x => x.CardHolder).OrderBy(x => x.ComponentId).Where(x => x.CardNo.Equals(message.tran.c_full.cardholder_id)).Select(x => x.CardHolder).FirstOrDefaultAsync();
                                if (holder is not null) tran.Actor = TransactionHelper.ContructFullName(holder);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                tran.Remark = $"FAC: {message.tran.c_full.facility_code}   Card: {message.tran.c_full.cardholder_id}";
                                break;
                            case (short)tranType.tranTypeDblCardFull:
                                tran = await TransactionBuilder(message);
                                holder = await context.Credentials.AsNoTracking().Include(x => x.CardHolder).OrderBy(x => x.ComponentId).Where(x => x.CardNo.Equals(message.tran.c_fulldbl.cardholder_id)).Select(x => x.CardHolder).FirstOrDefaultAsync();
                                if (holder is not null) tran.Actor = TransactionHelper.ContructFullName(holder);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                tran.Remark = $"FAC: {message.tran.c_fulldbl.facility_code}   Card: {message.tran.c_fulldbl.cardholder_id}";
                                break;
                            case (short)tranType.tranTypeI64CardFull:
                                tran = await TransactionBuilder(message);
                                holder = await context.Credentials.AsNoTracking().Include(x => x.CardHolder).OrderBy(x => x.ComponentId).Where(x => x.CardNo.Equals(message.tran.c_fulli64.cardholder_id)).Select(x => x.CardHolder).FirstOrDefaultAsync();
                                if (holder is not null) tran.Actor = TransactionHelper.ContructFullName(holder);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                tran.Remark = $"FAC: {message.tran.c_fulli64.facility_code}   Card: {message.tran.c_fulli64.cardholder_id}";
                                break;
                            case (short)tranType.tranTypeI64CardFullIc32:
                                tran = await TransactionBuilder(message);
                                holder = await context.Credentials.AsNoTracking().Include(x => x.CardHolder).OrderBy(x => x.ComponentId).Where(x => x.CardNo.Equals(message.tran.c_fulli64i32.cardholder_id)).Select(x => x.CardHolder).FirstOrDefaultAsync();
                                if (holder is not null) tran.Actor = TransactionHelper.ContructFullName(holder);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                tran.Remark = $"FAC: {message.tran.c_fulli64i32.facility_code}   Card: {message.tran.c_fulli64i32.cardholder_id}";
                                //tran.TypeCardFull.formatNumber = message.tran.c_full.format_number;
                                //tran.TypeCardFull.facilityCode = message.tran.c_full.facility_code;
                                //tran.TypeCardFull.cardHolderId = message.tran.c_full.cardholder_id;
                                //tran.TypeCardFull.issueCode = message.tran.c_full.issue_code;
                                //tran.TypeCardFull.floorNumber = message.tran.c_full.floor_number;
                                //tran.TypeCardFull.encodedCard = BitConverter.ToString(message.tran.c_full.encoded_card).Replace("-",":");
                                break;
                            case (short)tranType.tranTypeCardID:
                            case (short)tranType.tranTypeDblCardID:
                            case (short)tranType.tranTypeI64CardID:
                                tran = await TransactionBuilder(message);
                                //tran.TypeCardID.formatNumber = message.tran.c_id.format_number;
                                //tran.TypeCardID.cardHolderId = message.tran.c_id.cardholder_id;
                                //tran.TypeCardID.floorNumber = message.tran.c_id.floor_number;
                                //tran.TypeCardID.cardTypeFlag = message.tran.c_id.card_type_flags;
                                //tran.TypeCardID.TransactionFlags = TransactionHelper.TypeCardIDCardTypeFlag(message.tran.c_id.card_type_flags);
                                holder = await context.Credentials.AsNoTracking().Include(x => x.CardHolder).OrderBy(x => x.ComponentId).Where(x => x.CardNo.Equals(message.tran.c_id.cardholder_id)).Select(x => x.CardHolder).FirstOrDefaultAsync();
                                if(holder is not null) tran.Actor = TransactionHelper.ContructFullName(holder);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? ""; 
                                break;
                            case (short)tranType.tranTypeREX:
                                tran = await TransactionBuilder(message);
                                tran.Remark = $"REX No: {message.tran.rex.rex_number}";
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeUserCmnd:
                                tran = await TransactionBuilder(message);
                                //tran.TypeUserCmnd.nKeys = message.tran.usrcmd.nKeys;
                                //tran.TypeUserCmnd.keys = new string(message.tran.usrcmd.keys);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeAcr:
                                tran = await TransactionBuilder(message);
                                tran.TransactionFlags.AddRange(TransactionHelper.TypeAcrAccessControlFlag(message.tran.acr.actl_flags));
                                tran.TransactionFlags.AddRange(TransactionHelper.TypeAcrSpareFlag(message.tran.acr.actl_flags_e));
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeUseLimit:
                                tran = await TransactionBuilder(message);
                                tran.Remark = $"Use count: {message.tran.c_uselimit.use_count}";
                                holder = await context.Credentials.AsNoTracking().Include(x => x.CardHolder).OrderBy(x => x.ComponentId).Where(x => x.CardNo.Equals(message.tran.c_uselimit.cardholder_id)).Select(x => x.CardHolder).FirstOrDefaultAsync();
                                if (holder is not null) tran.Actor = TransactionHelper.ContructFullName(holder);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCoSElevator:
                                tran = await TransactionBuilder(message);
                                tran.Remark = $"Floor: {message.tran.floor.floorNumber}";
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCoSElevatorAccess:
                                tran = await TransactionBuilder(message);
                                holder = await context.Credentials.AsNoTracking().Include(x => x.CardHolder).OrderBy(x => x.ComponentId).Where(x => x.CardNo.Equals(message.tran.elev_access.cardholder_id)).Select(x => x.CardHolder).FirstOrDefaultAsync();
                                if (holder is not null) tran.Actor = TransactionHelper.ContructFullName(holder);
                                tran.Remark = string.Join(" ",message.tran.elev_access.floors.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeAcrExtFeatureStls:
                                tran = await TransactionBuilder(message);
                                tran.TransactionFlags = TransactionHelper.TypeAcrSpareFlag(message.tran.extfeat_stls.nExtFeatureType);
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeAcrExtFeatureCoS:
                                tran = await TransactionBuilder(message);
                                tran.TransactionFlags.AddRange(TransactionHelper.TypeAcrSpareFlag(message.tran.extfeat_cos.nExtFeatureType));
                                tran.TransactionFlags.AddRange(TransactionHelper.TypeCosStatus(message.tran.extfeat_cos.nStatus, message.tran.source_number, tranSrc.tranSrcACR));
                                tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                                break;
                            default:
                                break;
                        }
                        break;
                    case (short)tranSrc.tranSrcAcrTmpr:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrTmpr);
                        break;
                    case (short)tranSrc.tranSrcAcrDoor:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags.AddRange(TransactionHelper.TypeCosStatus(message.tran.door.door_status, message.tran.source_number, tranSrc.tranSrcAcrDoor));
                        tran.TransactionFlags.AddRange(TransactionHelper.TypeCosDoorAccessPointStatus(message.tran.door.ap_status));
                        break;
                    case (short)tranSrc.tranSrcAcrRex0:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrRex0);
                        break;
                    case (short)tranSrc.tranSrcAcrRex1:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrRex1);
                        break;
                    case (short)tranSrc.tranSrcTimeZone:
                        break;
                    case (short)tranSrc.tranSrcProcedure:
                        break;
                    case (short)tranSrc.tranSrcTrigger:
                        break;
                    case (short)tranSrc.tranSrcTrigVar:
                        break;
                    case (short)tranSrc.tranSrcMPG:
                        tran = await TransactionBuilder(message);
                        tran.Remark = $"Mask Count: {message.tran.mpg.mask_count}";
                        break;
                    case (short)tranSrc.tranSrcArea:
                        tran = await TransactionBuilder(message);
                        tran.Remark = $"Occupancy: {message.tran.area.occupancy}";
                        break;
                    case (short)tranSrc.tranSrcAcrTmprAlt:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.Origin = await context.Doors.AsNoTracking().OrderBy(x => x.ComponentId).Where(x => x.MacAddress == mac && x.ComponentId == message.tran.source_number).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrTmprAlt);
                        break;
                    case (short)tranSrc.tranSrcLoginService:
                        tran = await TransactionBuilder(message);
                        tran.TransactionFlags = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcLoginService);
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(tran.Date) && string.IsNullOrEmpty(tran.Time)) return;

                await context.Transactions.AddAsync(tran);
                context.SaveChanges();

            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message);
            }
          

        }

      
        public async Task<ResponseDto<bool>> SetTranIndexAsync(string mac)
        {
            var id = await helperService.GetIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!await command.SetTransactionLogIndexAsync(id,true))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C303));
            }

            return ResponseHelper.SuccessBuilder(true);
        }

        private async Task<HIDAeroService.Entity.Transaction> TransactionBuilder(SCPReplyMessage message)
        {
            var trand = TransactionHelper.TranCodeDesc((tranType)message.tran.tran_type, message.tran.tran_code);
            var tran = new HIDAeroService.Entity.Transaction()
            {
                // Base 
                ComponentId = (short)message.SCPId,
                MacAddress = helperService.GetMacFromId((short)message.SCPId),
                LocationId = await context.Hardwares
                  .AsNoTracking()
                  .OrderBy(h => h.ComponentId)
                  .Where(h => h.ComponentId == (short)message.SCPId)
                  .Select(h => h.LocationId)
                  .FirstOrDefaultAsync(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,

                // ExtendDesc
                Date = UtilityHelper.UnixToDateTimeParts(message.tran.time)[0],
                Time = UtilityHelper.UnixToDateTimeParts(message.tran.time)[1],
                SerialNumber = message.tran.ser_num,
                Source = message.tran.source_number,
                SourceModule = TransactionHelper.Source((tranSrc)message.tran.source_type),
                SourceDesc = TransactionHelper.SourceDesc((tranSrc)message.tran.source_type),
                Type = message.tran.tran_type,
                TypeDesc = TransactionHelper.TypeDesc((tranType)message.tran.tran_type),
                TranCode = message.tran.tran_code,
                TranCodeDesc = trand[0],
                ExtendDesc = trand[1],
                TransactionFlags = new List<TransactionFlag>()
            };

            return tran;
        }
    }
}
