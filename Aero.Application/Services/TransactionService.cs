using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.Transactions;
using AeroService.Entity;
using AeroService.Enums;
using AeroService.Helpers;
using AeroService.Hubs;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Utils;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using static MiNET.Net.McpeUpdateBlock;

namespace AeroService.Service.Impl
{
    public sealed class TransactionService(AppDbContext context, IHubContext<AeroHub> hub,AeroCommandService command,IHelperService helperService,ILogger<TransactionService> logger) : ITransactionService
    {
        public async Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAsync(PaginationParams param)
        {
            var query = context.transaction.AsQueryable();
            var count = await query.CountAsync();
            var data = await query
                .AsNoTracking()
                .Include(x => x.transaction_flag)
                .OrderByDescending(t => t.created_date)
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(en => new TransactionDto
                {
                    // Base 
                    Uuid = en.uuid,
                    ComponentId = en.component_id,
                    Mac = en.hardware_mac,
                    LocationId = en.location_id,

                    // extend_desc
                    Date = en.date,
                    Time = en.time,
                    SerialNumber = en.serial_number,
                    Actor = en.actor,
                    Source = en.source,
                    Origin = en.origin,
                    SourceModule = en.source_module,
                    Type = en.type,
                    TypeDesc = en.type_desc,
                    TranCode = en.tran_code,
                    Image = en.image_path,
                    TranCodeDesc = en.tran_code_desc,
                    ExtendDesc = en.extend_desc,
                    Remark = en.remark,
                    TransactionFlags = en.transaction_flag.Select(x => new TransactionFlagDto
                    {
                        Topic = x.topic,
                        Name = x.name,
                        Description = x.description
                    }).ToList(),

                })
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
            hub.Clients.All.SendAsync("transaction");
        }

        public async Task SaveToDatabaseAsync(SCPReplyMessage message)
        {       
            try
            {
                if (!await context.hardware.AnyAsync(s => s.component_id == (short)message.SCPId)) return;
                AeroService.Entity.Transaction tran = new AeroService.Entity.Transaction();
                switch (message.tran.source_type)
                {

                    case (short)tranSrc.tranSrcScpDiag:
                        switch (message.tran.tran_type)
                        {
                            case (short)tranType.tranTypeSys:
                                tran = await TransactionBuilder(message);
                                tran.origin = await context.hardware.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.component_id == (short)message.SCPId).Select(s => s.name).FirstOrDefaultAsync() ?? "";
                                if (message.tran.tran_code == 1)
                                {
                                    tran.transaction_flag = TransactionHelper.TypeSysErrorFlag(message.tran.sys_comm.error_code);
                                }
                                break;
                            case (short)tranType.tranTypeWebActivity:
                                tran = await TransactionBuilder(message);
                                tran.origin = UtilityHelper.IntegerToIp(message.tran.web_activity.ipAddress);
                                tran.actor = new string(message.tran.web_activity.szObjectUser);
                                break;
                            case (short)tranType.tranTypeFileDownloadStatus:
                                tran = await TransactionBuilder(message);
                                tran.remark = $"hardware_type: {TransactionHelper.FileTypeToText(message.tran.file_download.fileType)} ,name: {new string(message.tran.file_download.fileName)}";
                                break;
                            default:
                                break;
                        }
                        break;
                    case (short)tranSrc.tranSrcScpLcl:
                        tran = await TransactionBuilder(message);
                        tran.origin = await context.hardware.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.component_id == (short)message.SCPId).Select(s => s.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcScpLcl);
                        break;
                    case (short)tranSrc.tranSrcSioDiag:
                        tran = await TransactionBuilder(message);
                        var mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
                        break;
                    case (short)tranSrc.tranSrcSioCom:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
                        //tran.TypeSioComm.CommSts = TransactionHelper.TypeSioCommStatus(message.tran.s_comm.comm_sts);
                        //tran.TypeSioComm.hardware_type = TransactionHelper.TypeSioCommModel(message.tran.s_comm.model, message.tran.s_comm.comm_sts);
                        //tran.TypeSioComm.revision = message.tran.s_comm.revision;
                        //tran.TypeSioComm.Serial = message.tran.s_comm.ser_num;
                        //tran.TypeSioComm.nExtendedInfoValid = message.tran.s_comm.nExtendedInfoValid;
                        //if(message.tran.s_comm.nExtendedInfoValid != 0)
                        //{
                        //    tran.TypeSioComm.hardware = TransactionHelper.TypeSioCommHardwareId(message.tran.s_comm.n_hardware_id);
                        //    tran.TypeSioComm.n_hardware_rev = message.tran.s_comm.n_hardware_rev;
                        //    tran.TypeSioComm.n_product_id = message.tran.s_comm.n_product_id;
                        //    tran.TypeSioComm.n_product_ver = message.tran.s_comm.n_product_ver;
                        //    tran.TypeSioComm.nFirmwareBoot = message.tran.s_comm.nFirmwareBoot;
                        //    tran.TypeSioComm.nFirmwareLdr = message.tran.s_comm.nFirmwareLdr;
                        //    tran.TypeSioComm.nFirmwareApp = message.tran.s_comm.nFirmwareApp;
                        //    tran.TypeSioComm.nOemCode = message.tran.s_comm.nOemCode;
                        //    tran.TypeSioComm.n_enc_config = TransactionHelper.TypeSioCommEncConfig(message.tran.s_comm.n_enc_config);
                        //    tran.TypeSioComm.nKeyStatus = TransactionHelper.TypeSioCommEncKeyStatus(message.tran.s_comm.n_enc_key_status);
                        //    tran.TypeSioComm.nHardwareComponents = message.tran.s_comm.nHardwareComponents;
                        //    tran.TypeSioComm.transaction_flag = TransactionHelper.TypeSioHardwareComponent(message.tran.s_comm.nHardwareComponents);

                        //}
                        break;
                    case (short)tranSrc.tranSrcSioTmpr:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioTmpr);
                        break;
                    case (short)tranSrc.tranSrcSioPwr:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioPwr);
                        break;
                    case (short)tranSrc.tranSrcMP:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.monitor_point.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.module.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcMP);
                        break;
                    case (short)tranSrc.tranSrcCP:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.control_point.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.module.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcCP);
                        break;
                    case (short)tranSrc.tranSrcACR:
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        switch (message.tran.tran_type)
                        {

                            case (short)tranType.tranTypeCardBin:
                                tran = await TransactionBuilder(message);
                                tran.remark = $"bits Count: {message.tran.c_bin.bit_count}";
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCardBcd:
                                tran = await TransactionBuilder(message);
                                tran.remark = $"Digits Count: {message.tran.c_bin.bit_count}";
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCardFull:
                                tran = await TransactionBuilder(message);
                                var holder = await context.credential.AsNoTracking().Include(x => x.cardholder).OrderBy(x => x.component_id).Where(x => x.card_no.Equals(message.tran.c_full.cardholder_id)).Select(x => x.cardholder).FirstOrDefaultAsync();
                                if (holder is not null) tran.actor = TransactionHelper.ContructFullName(holder);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                tran.remark = $"FAC: {message.tran.c_full.facility_code}   Card: {message.tran.c_full.cardholder_id}";
                                break;
                            case (short)tranType.tranTypeDblCardFull:
                                tran = await TransactionBuilder(message);
                                holder = await context.credential.AsNoTracking().Include(x => x.cardholder).OrderBy(x => x.component_id).Where(x => x.card_no.Equals(message.tran.c_fulldbl.cardholder_id)).Select(x => x.cardholder).FirstOrDefaultAsync();
                                if (holder is not null) tran.actor = TransactionHelper.ContructFullName(holder);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                tran.remark = $"FAC: {message.tran.c_fulldbl.facility_code}   Card: {message.tran.c_fulldbl.cardholder_id}";
                                break;
                            case (short)tranType.tranTypeI64CardFull:
                                tran = await TransactionBuilder(message);
                                holder = await context.credential.AsNoTracking().Include(x => x.cardholder).OrderBy(x => x.component_id).Where(x => x.card_no.Equals(message.tran.c_fulli64.cardholder_id)).Select(x => x.cardholder).FirstOrDefaultAsync();
                                if (holder is not null) tran.actor = TransactionHelper.ContructFullName(holder);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                tran.remark = $"FAC: {message.tran.c_fulli64.facility_code}   Card: {message.tran.c_fulli64.cardholder_id}";
                                break;
                            case (short)tranType.tranTypeI64CardFullIc32:
                                tran = await TransactionBuilder(message);
                                holder = await context.credential.AsNoTracking().Include(x => x.cardholder).OrderBy(x => x.component_id).Where(x => x.card_no.Equals(message.tran.c_fulli64i32.cardholder_id)).Select(x => x.cardholder).FirstOrDefaultAsync();
                                if (holder is not null) tran.actor = TransactionHelper.ContructFullName(holder);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                tran.remark = $"FAC: {message.tran.c_fulli64i32.facility_code}   Card: {message.tran.c_fulli64i32.cardholder_id}";
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
                                //tran.TypeCardID.transaction_flag = TransactionHelper.TypeCardIDCardTypeFlag(message.tran.c_id.card_type_flags);
                                holder = await context.credential.AsNoTracking().Include(x => x.cardholder).OrderBy(x => x.component_id).Where(x => x.card_no.Equals(message.tran.c_id.cardholder_id)).Select(x => x.cardholder).FirstOrDefaultAsync();
                                if(holder is not null) tran.actor = TransactionHelper.ContructFullName(holder);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? ""; 
                                break;
                            case (short)tranType.tranTypeREX:
                                tran = await TransactionBuilder(message);
                                tran.remark = $"REX No: {message.tran.rex.rex_number}";
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeUserCmnd:
                                tran = await TransactionBuilder(message);
                                //tran.TypeUserCmnd.nKeys = message.tran.usrcmd.nKeys;
                                //tran.TypeUserCmnd.keys = new string(message.tran.usrcmd.keys);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeAcr:
                                tran = await TransactionBuilder(message);
                                tran.transaction_flag.AddRange(TransactionHelper.TypeAcrAccessControlFlag(message.tran.acr.actl_flags));
                                tran.transaction_flag.AddRange(TransactionHelper.TypeAcrSpareFlag(message.tran.acr.actl_flags_e));
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeUseLimit:
                                tran = await TransactionBuilder(message);
                                tran.remark = $"Use count: {message.tran.c_uselimit.use_count}";
                                holder = await context.credential.AsNoTracking().Include(x => x.cardholder).OrderBy(x => x.component_id).Where(x => x.card_no.Equals(message.tran.c_uselimit.cardholder_id)).Select(x => x.cardholder).FirstOrDefaultAsync();
                                if (holder is not null) tran.actor = TransactionHelper.ContructFullName(holder);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCoSElevator:
                                tran = await TransactionBuilder(message);
                                tran.remark = $"Floor: {message.tran.floor.floorNumber}";
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeCoSElevatorAccess:
                                tran = await TransactionBuilder(message);
                                holder = await context.credential.AsNoTracking().Include(x => x.cardholder).OrderBy(x => x.component_id).Where(x => x.card_no.Equals(message.tran.elev_access.cardholder_id)).Select(x => x.cardholder).FirstOrDefaultAsync();
                                if (holder is not null) tran.actor = TransactionHelper.ContructFullName(holder);
                                tran.remark = string.Join(" ",message.tran.elev_access.floors.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeAcrExtFeatureStls:
                                tran = await TransactionBuilder(message);
                                tran.transaction_flag = TransactionHelper.TypeAcrSpareFlag(message.tran.extfeat_stls.nExtFeatureType);
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            case (short)tranType.tranTypeAcrExtFeatureCoS:
                                tran = await TransactionBuilder(message);
                                tran.transaction_flag.AddRange(TransactionHelper.TypeAcrSpareFlag(message.tran.extfeat_cos.nExtFeatureType));
                                tran.transaction_flag.AddRange(TransactionHelper.TypeCosStatus(message.tran.extfeat_cos.nStatus, message.tran.source_number, tranSrc.tranSrcACR));
                                tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                break;
                            default:
                                break;
                        }
                        break;
                    case (short)tranSrc.tranSrcAcrTmpr:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrTmpr);
                        break;
                    case (short)tranSrc.tranSrcAcrDoor:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag.AddRange(TransactionHelper.TypeCosStatus(message.tran.door.door_status, message.tran.source_number, tranSrc.tranSrcAcrDoor));
                        tran.transaction_flag.AddRange(TransactionHelper.TypeCosDoorAccessPointStatus(message.tran.door.ap_status));
                        break;
                    case (short)tranSrc.tranSrcAcrRex0:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrRex0);
                        break;
                    case (short)tranSrc.tranSrcAcrRex1:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrRex1);
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
                        tran.remark = $"Mask Count: {message.tran.mpg.mask_count}";
                        break;
                    case (short)tranSrc.tranSrcArea:
                        tran = await TransactionBuilder(message);
                        tran.remark = $"Occupancy: {message.tran.area.occupancy}";
                        break;
                    case (short)tranSrc.tranSrcAcrTmprAlt:
                        tran = await TransactionBuilder(message);
                        mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                        tran.origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrTmprAlt);
                        break;
                    case (short)tranSrc.tranSrcLoginService:
                        tran = await TransactionBuilder(message);
                        tran.transaction_flag = TransactionHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcLoginService);
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(tran.date) && string.IsNullOrEmpty(tran.time)) return;

                await context.transaction.AddAsync(tran);
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
            if (!command.SetTransactionLogIndex(id,true))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C303));
            }

            return ResponseHelper.SuccessBuilder(true);
        }

        private async Task<AeroService.Entity.Transaction> TransactionBuilder(SCPReplyMessage message)
        {
            var trand = TransactionHelper.TranCodeDesc((tranType)message.tran.tran_type, message.tran.tran_code);
            var tran = new AeroService.Entity.Transaction()
            {
                // Base 
                component_id = (short)message.SCPId,
                hardware_mac = await helperService.GetMacFromIdAsync((short)message.SCPId),
                hardware_name = await helperService.GetHardwareNameById((short)message.SCPId),
                location_id = await context.hardware
                  .AsNoTracking()
                  .OrderBy(h => h.component_id)
                  .Where(h => h.component_id == (short)message.SCPId)
                  .Select(h => h.location_id)
                  .FirstOrDefaultAsync(),
                created_date = DateTime.UtcNow,
                updated_date = DateTime.UtcNow,

                // extend_desc
                date = UtilityHelper.UnixToDateTimeParts(message.tran.time)[0],
                time = UtilityHelper.UnixToDateTimeParts(message.tran.time)[1],
                serial_number = message.tran.ser_num,
                source = message.tran.source_number,
                source_module = TransactionHelper.Source((tranSrc)message.tran.source_type),
                source_desc = TransactionHelper.SourceDesc((tranSrc)message.tran.source_type),
                type = message.tran.tran_type,
                type_desc = TransactionHelper.TypeDesc((tranType)message.tran.tran_type),
                tran_code = message.tran.tran_code,
                tran_code_desc = trand[0],
                extend_desc = trand[1],
                transaction_flag = new List<TransactionFlag>()
            };

            return tran;
        }
    }
}
