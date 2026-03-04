using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Helpers;
using Aero.Infrastructure.Mapper;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;
using Aero.Infrastructure.Persistences.Entities;

namespace Aero.Infrastructure.Repositories;

public class TranRepository(AppDbContext context,IDeviceRepository hw,ICredRepository qCred) : ITransactionRepository
{
      public async Task<int> AddAsync(Aero.Domain.Entities.Transaction data)
      {
            var en = new Persistences.Entities.Transaction(data);
            await context.transaction.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }


      public async Task<int> UpdateAsync(Aero.Domain.Entities.Transaction newData)
      {
            throw new NotImplementedException();
      }

      public async Task<Aero.Domain.Entities.Transaction> HandleTransactionAsync(IScpReply message)
      {
            var tran = new Aero.Domain.Entities.Transaction();
            switch (message.tran.source_type)
            {

                  case (short)tranSrc.tranSrcScpDiag:
                        switch (message.tran.tran_type)
                        {
                              case (short)tranType.tranTypeSys:
                                    tran = await TransactionBuilder(message);
                                    tran.Origin = await context.device.AsNoTracking().OrderBy(x => x.id).Where(x => x.driver_id == (short)message.ScpId).Select(s => s.name).FirstOrDefaultAsync() ?? "";
                                    if (message.tran.tran_code == 1)
                                    {
                                          tran.TransactionFlags = AeroTransactionHandlerHelper.TypeSysErrorFlag(message.tran.sys_comm.error_code);
                                    }
                                    break;
                              case (short)tranType.tranTypeWebActivity:
                                    tran = await TransactionBuilder(message);
                                    tran.Origin = UtilitiesHelper.IntegerToIp(message.tran.web_activity.ipAddress);
                                    tran.Actor = new string(message.tran.web_activity.szObjectUser);
                                    break;
                              case (short)tranType.tranTypeFileDownloadStatus:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"hardware_type: {AeroTransactionHandlerHelper.FileTypeToText(message.tran.file_download.fileType)} ,name: {new string(message.tran.file_download.fileName)}";
                                    break;
                              default:
                                    break;
                        }
                        break;
                  case (short)tranSrc.tranSrcScpLcl:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.device.AsNoTracking().OrderBy(x => x.id).Where(x => x.driver_id == (short)message.ScpId).Select(s => s.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcScpLcl);
                        break;
                  case (short)tranSrc.tranSrcSioDiag:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.model_detail).FirstOrDefaultAsync() ?? "";
                        break;
                  case (short)tranSrc.tranSrcSioCom:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.model_detail).FirstOrDefaultAsync() ?? "";
                        //tran.TypeSioComm.CommSts = AeroTransactionHandlerHelper.TypeSioCommStatus(message.tran.s_comm.comm_sts);
                        //tran.TypeSioComm.hardware_type = AeroTransactionHandlerHelper.TypeSioCommModel(message.tran.s_comm.model, message.tran.s_comm.comm_sts);
                        //tran.TypeSioComm.revision = message.tran.s_comm.revision;
                        //tran.TypeSioComm.Serial = message.tran.s_comm.ser_num;
                        //tran.TypeSioComm.nExtendedInfoValid = message.tran.s_comm.nExtendedInfoValid;
                        //if(message.tran.s_comm.nExtendedInfoValid != 0)
                        //{
                        //    tran.TypeSioComm.hardware = AeroTransactionHandlerHelper.TypeSioCommHardwareId(message.tran.s_comm.n_hardware_id);
                        //    tran.TypeSioComm.n_hardware_rev = message.tran.s_comm.n_hardware_rev;
                        //    tran.TypeSioComm.n_product_id = message.tran.s_comm.n_product_id;
                        //    tran.TypeSioComm.n_product_ver = message.tran.s_comm.n_product_ver;
                        //    tran.TypeSioComm.nFirmwareBoot = message.tran.s_comm.nFirmwareBoot;
                        //    tran.TypeSioComm.nFirmwareLdr = message.tran.s_comm.nFirmwareLdr;
                        //    tran.TypeSioComm.nFirmwareApp = message.tran.s_comm.nFirmwareApp;
                        //    tran.TypeSioComm.nOemCode = message.tran.s_comm.nOemCode;
                        //    tran.TypeSioComm.n_enc_config = AeroTransactionHandlerHelper.TypeSioCommEncConfig(message.tran.s_comm.n_enc_config);
                        //    tran.TypeSioComm.nKeyStatus = AeroTransactionHandlerHelper.TypeSioCommEncKeyStatus(message.tran.s_comm.n_enc_key_status);
                        //    tran.TypeSioComm.nHardwareComponents = message.tran.s_comm.nHardwareComponents;
                        //    tran.TypeSioComm.transaction_flag = AeroTransactionHandlerHelper.TypeSioHardwareComponent(message.tran.s_comm.nHardwareComponents);

                        //}
                        break;
                  case (short)tranSrc.tranSrcSioTmpr:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.model_detail).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioTmpr);
                        break;
                  case (short)tranSrc.tranSrcSioPwr:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.model_detail).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioPwr);
                        break;
                  case (short)tranSrc.tranSrcMP:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.monitor_point.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcMP);
                        break;
                  case (short)tranSrc.tranSrcCP:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.control_point.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcCP);
                        break;
                  case (short)tranSrc.tranSrcACR:
                        switch (message.tran.tran_type)
                        {

                              case (short)tranType.tranTypeCardBin:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"bits Count: {message.tran.c_bin.bit_count}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCardBcd:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"Digits Count: {message.tran.c_bcd.digit_count}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCardFull:
                                    tran = await TransactionBuilder(message);
                                    var holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_full.cardholder_id);
                                    tran.Actor = holder[0];
                                    tran.Image = holder[1];
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_full.facility_code}   Card: {message.tran.c_full.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeDblCardFull:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_fulldbl.cardholder_id);
                                    tran.Actor = holder[0];
                                    tran.Image = holder[1];
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_fulldbl.facility_code}   Card: {message.tran.c_fulldbl.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeI64CardFull:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_fulli64.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_fulli64.facility_code}   Card: {message.tran.c_fulli64.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeI64CardFullIc32:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_fulli64i32.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_fulli64i32.facility_code}   Card: {message.tran.c_fulli64i32.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeCardID:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_id.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"Card: {message.tran.c_id.cardholder_id}";
                                    break;
                                case (short)tranType.tranTypeDblCardID:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_iddbl.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"Card: {message.tran.c_iddbl.cardholder_id}";
                                    break;
                                case (short)tranType.tranTypeI64CardID:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_idi64.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeREX:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"REX No: {message.tran.rex.rex_number}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeUserCmnd:
                                    tran = await TransactionBuilder(message);
                                    //tran.TypeUserCmnd.nKeys = message.tran.usrcmd.nKeys;
                                    //tran.TypeUserCmnd.keys = new string(message.tran.usrcmd.keys);
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeAcr:
                                    tran = await TransactionBuilder(message);
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeAcrAccessControlFlag(message.tran.acr.actl_flags));
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeAcrSpareFlag(message.tran.acr.actl_flags_e));
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeUseLimit:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"Use count: {message.tran.c_uselimit.use_count}";
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_full.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCoSElevator:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"Floor: {message.tran.floor.floorNumber}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCoSElevatorAccess:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_full.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Remark = string.Join(" ", message.tran.elev_access.floors.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeAcrExtFeatureStls:
                                    tran = await TransactionBuilder(message);
                                    tran.TransactionFlags = AeroTransactionHandlerHelper.TypeAcrSpareFlag(message.tran.extfeat_stls.nExtFeatureType);
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeAcrExtFeatureCoS:
                                    tran = await TransactionBuilder(message);
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeAcrSpareFlag(message.tran.extfeat_cos.nExtFeatureType));
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeCosStatus(message.tran.extfeat_cos.nStatus, message.tran.source_number, tranSrc.tranSrcACR));
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              default:
                                    break;
                        }
                        break;
                  case (short)tranSrc.tranSrcAcrTmpr:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrTmpr);
                        break;
                  case (short)tranSrc.tranSrcAcrDoor:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeCosStatus(message.tran.door.door_status, message.tran.source_number, tranSrc.tranSrcAcrDoor));
                        tran.ExtendDesc += AeroTransactionHandlerHelper.TypeCosDoorAccessPointStatusString(message.tran.door.ap_status);
                        tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeCosDoorAccessPointStatus(message.tran.door.ap_status));
                        break;
                  case (short)tranSrc.tranSrcAcrRex0:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrRex0);
                        break;
                  case (short)tranSrc.tranSrcAcrRex1:
                        tran = await TransactionBuilder(message);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrRex1);
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
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.id).Where(x => x.device_id == message.ScpId && x.driver_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrTmprAlt);
                        break;
                  case (short)tranSrc.tranSrcLoginService:
                        tran = await TransactionBuilder(message);
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcLoginService);
                        break;
                  default:
                        break;
            }

            return tran;
      }

      private async Task<Aero.Domain.Entities.Transaction> TransactionBuilder(IScpReply message)
        {
            var trand = AeroTransactionHandlerHelper.TranCodeDesc((tranType)message.tran.tran_type, message.tran.tran_code);
            var tran = new Aero.Domain.Entities.Transaction(
                DateTime.UtcNow,
                message.tran.ser_num,
                "",
                message.tran.source_number,
                AeroTransactionHandlerHelper.SourceDesc((tranSrc)message.tran.source_type),
                "",
                AeroTransactionHandlerHelper.Source((tranSrc)message.tran.source_type),
                message.tran.tran_type,
                AeroTransactionHandlerHelper.TypeDesc((tranType)message.tran.tran_type),
                message.tran.tran_code,
                "",
                trand[0],
                trand[1],
                "",
                new List<Aero.Domain.Entities.TransactionFlag>(),
                await hw.GetMacFromComponentAsync((short)message.ScpId),
                await hw.GetNameByDeviceIdAsync((short)message.ScpId)

            );
            return tran;
        }

    public async Task<Pagination<TransactionDto>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param, int location)
    {
        var query = context.transaction.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.mac, pattern) ||
                        EF.Functions.ILike(x.actor, pattern) ||
                        EF.Functions.ILike(x.origin, pattern) ||
                        EF.Functions.ILike(x.source_desc, pattern) ||
                        EF.Functions.ILike(x.type_desc, pattern) ||
                        EF.Functions.ILike(x.tran_code_desc, pattern) ||
                        EF.Functions.ILike(x.extend_desc, pattern) ||
                        EF.Functions.ILike(x.remark, pattern) ||
                        EF.Functions.ILike(x.hardware_name, pattern)
                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.mac.Contains(search) ||
                        x.actor.Contains(search) ||
                        x.origin.Contains(search) ||
                        x.source_desc.Contains(search) ||
                        x.type_desc.Contains(search) ||
                        x.tran_code_desc.Contains(search) ||
                        x.extend_desc.Contains(search) ||
                        x.remark.Contains(search) ||
                        x.hardware_name.Contains(search)
                    );
                }
            }
        }

        if (location >= 0)
        {
            query = query.Where(x => x.location_id == location);
        }

        if (param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.date_time >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.date_time <= endUtc);
        }

        var count = await query.CountAsync();


        var data = await query
            .AsNoTracking()
            .Include(x => x.transaction_flag)
            .OrderByDescending(t => t.created_date)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(en => new {
                en.date_time,
                en.serial_number,
                en.actor,
                en.source,
                en.source_desc,
                en.origin,
                en.source_module,
                en.type,
                en.type_desc,
                en.tran_code,
                en.image_path,
                en.tran_code_desc,
                en.extend_desc,
                en.remark,
                tranflag = en.transaction_flag.Select(x => new TransactionFlagDto
                (
                    x.topic,
                   x.name,
                    x.description
                )),
                en.location_id,
                en.is_active
            }) .ToListAsync();

            var res = data.Select(en => new TransactionDto(
                en.date_time,
                en.serial_number,
                en.actor,
                en.source,
                en.source_desc,
                en.origin,
                en.source_module,
                en.type,
                en.type_desc,
                en.tran_code,
                en.image_path,
                en.tran_code_desc,
                en.extend_desc,
                en.remark,
                en.tranflag.ToList(),
                en.location_id,
                en.is_active
            )).ToArray();


        return new Pagination<TransactionDto>
        {
            Data = res,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public async Task<Pagination<TransactionDto>> GetPageTransactionWithCountAsync(PaginationParams param)
    {
        var query = context.transaction.AsQueryable();
        var count = await query.CountAsync();
        var data = await query
            .AsNoTracking()
            .Include(x => x.transaction_flag)
            .OrderByDescending(t => t.created_date)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(en => new {
                en.date_time,
                en.serial_number,
                en.actor,
                en.source,
                en.source_desc,
                en.origin,
                en.source_module,
                en.type,
                en.type_desc,
                en.tran_code,
                en.image_path,
                en.tran_code_desc,
                en.extend_desc,
                en.remark,
                tranflag = en.transaction_flag.Select(x => new TransactionFlagDto
                (
                    x.topic,
                   x.name,
                    x.description
                )),
                en.location_id,
                en.is_active
            })
            .ToListAsync();

             var res = data.Select(en => new TransactionDto(
                en.date_time,
                en.serial_number,
                en.actor,
                en.source,
                en.source_desc,
                en.origin,
                en.source_module,
                en.type,
                en.type_desc,
                en.tran_code,
                en.image_path,
                en.tran_code_desc,
                en.extend_desc,
                en.remark,
                en.tranflag.ToList(),
                en.location_id,
                en.is_active
            )).ToArray();


        return new Pagination<TransactionDto>
        {
            Data = res,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public IEnumerable<ModeDto> GetSourceAsync()
    {
        var res = AeroTransactionHandlerHelper.SourceMap
     .GroupBy(x => x.Value)
     .Select(g => new ModeDto(g.Key,(short)g.First().Key,g.Key))
     .ToList();

        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetDeviceAsync(int source)
    {
        switch (source)
        {
            case (int)tranSrc.tranSrcScpDiag:
            case (int)tranSrc.tranSrcScpCom:
            case (int)tranSrc.tranSrcScpLcl:
                return await context.device.AsNoTracking().Select(x => new ModeDto(x.name,(short)x.driver_id,x.mac)).ToArrayAsync();
            case (int)tranSrc.tranSrcSioDiag:
            case (int)tranSrc.tranSrcSioCom:
            case (int)tranSrc.tranSrcSioTmpr:
            case (int)tranSrc.tranSrcSioPwr:
                return await context.module.AsNoTracking().Select(x => new ModeDto(x.model_detail,x.driver_id,"")).ToArrayAsync();
            case (int)tranSrc.tranSrcMP:
                return await context.monitor_point.AsNoTracking().Select(x => new ModeDto(x.name,x.driver_id,"")).ToArrayAsync();
            case (int)tranSrc.tranSrcCP:
                return await context.control_point.AsNoTracking().Select(x => new ModeDto(x.name,x.driver_id,"")).ToArrayAsync();
            case (int)tranSrc.tranSrcACR:
            case (int)tranSrc.tranSrcAcrTmpr:
            case (int)tranSrc.tranSrcAcrDoor:
            case (int)tranSrc.tranSrcAcrTmprAlt:
            case (int)tranSrc.tranSrcAcrRex0:
            case (int)tranSrc.tranSrcAcrRex1:
                return await context.door.AsNoTracking().Select(x => new ModeDto(x.name,x.driver_id,"")).ToArrayAsync();
            case (int)tranSrc.tranSrcTimeZone:
                return await context.timezone.AsNoTracking().Select(x => new ModeDto(x.name,x.driver_id,"")).ToArrayAsync();
            case (int)tranSrc.tranSrcProcedure:
                return await context.procedure.AsNoTracking().Select(x => new ModeDto(x.name,x.driver_id,x.driver_id.ToString())).ToArrayAsync();
            case (int)tranSrc.tranSrcTrigger:
            case (int)tranSrc.tranSrcTrigVar:
                return await context.trigger.AsNoTracking().Select(x => new ModeDto(x.name,x.driver_id,x.driver_id.ToString())).ToArrayAsync();
            case (int)tranSrc.tranSrcMPG:
                return await context.monitor_group.AsNoTracking().Select(x => new ModeDto(x.name,x.driver_id,"")).ToArrayAsync();
            case (int)tranSrc.tranSrcArea:
                return await context.area.AsNoTracking().Select(x => new ModeDto(x.name,(short)x.driver_id,"")).ToArrayAsync();
            case (int)tranSrc.tranSrcLoginService:
            default:
                return Enumerable.Empty<ModeDto>();
        }


    }

      IEnumerable<Mode> ITransactionRepository.GetSourceAsync()
      {
            throw new NotImplementedException();
      }

      Task<IEnumerable<Mode>> ITransactionRepository.GetDeviceAsync(int source)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<TransactionDto>> GetAsync()
      {
            throw new NotImplementedException();
      }

      public Task<TransactionDto> GetByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<TransactionDto>> GetByLocationIdAsync(int locationId)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<TransactionDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByNameAsync(string name)
      {
            throw new NotImplementedException();
      }
}
