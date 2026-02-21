using System;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Helpers;
using Aero.Infrastructure.Mapper;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class TransactionRepository(AppDbContext context,IQHwRepository qHw,IQCredRepository qCred) : Domain.Interface.ITransactionRepository
{
      public async Task<int> AddAsync(Transaction data)
      {
            var en = TransactionMapper.ToEf(data);
            await context.transaction.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }


      public async Task<int> UpdateAsync(Transaction newData)
      {
            throw new NotImplementedException();
      }

      public async Task<Transaction> HandleTransactionAsync(IScpReply message)
      {
            var tran = new Transaction();
            switch (message.tran.source_type)
            {

                  case (short)tranSrc.tranSrcScpDiag:
                        switch (message.tran.tran_type)
                        {
                              case (short)tranType.tranTypeSys:
                                    tran = await TransactionBuilder(message);
                                    tran.Origin = await context.hardware.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.component_id == (short)message.ScpId).Select(s => s.name).FirstOrDefaultAsync() ?? "";
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
                        tran.Origin = await context.hardware.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.component_id == (short)message.ScpId).Select(s => s.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcScpLcl);
                        break;
                  case (short)tranSrc.tranSrcSioDiag:
                        tran = await TransactionBuilder(message);
                        var mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
                        break;
                  case (short)tranSrc.tranSrcSioCom:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
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
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioTmpr);
                        break;
                  case (short)tranSrc.tranSrcSioPwr:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.module.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.model_desc).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcSioPwr);
                        break;
                  case (short)tranSrc.tranSrcMP:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.monitor_point.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.module.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcMP);
                        break;
                  case (short)tranSrc.tranSrcCP:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.control_point.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.module.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcCP);
                        break;
                  case (short)tranSrc.tranSrcACR:
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        switch (message.tran.tran_type)
                        {

                              case (short)tranType.tranTypeCardBin:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"bits Count: {message.tran.c_bin.bit_count}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCardBcd:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"Digits Count: {message.tran.c_bcd.digit_count}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCardFull:
                                    tran = await TransactionBuilder(message);
                                    var holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_full.cardholder_id);
                                    tran.Actor = holder[0];
                                    tran.Image = holder[1];
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_full.facility_code}   Card: {message.tran.c_full.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeDblCardFull:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_fulldbl.cardholder_id);
                                    tran.Actor = holder[0];
                                    tran.Image = holder[1];
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_fulldbl.facility_code}   Card: {message.tran.c_fulldbl.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeI64CardFull:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_fulli64.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_fulli64.facility_code}   Card: {message.tran.c_fulli64.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeI64CardFullIc32:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_fulli64i32.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"FAC: {message.tran.c_fulli64i32.facility_code}   Card: {message.tran.c_fulli64i32.cardholder_id}";
                                    break;
                              case (short)tranType.tranTypeCardID:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_id.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"Card: {message.tran.c_id.cardholder_id}";
                                    break;
                                case (short)tranType.tranTypeDblCardID:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_iddbl.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    tran.Remark = $"Card: {message.tran.c_iddbl.cardholder_id}";
                                    break;
                                case (short)tranType.tranTypeI64CardID:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_idi64.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeREX:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"REX No: {message.tran.rex.rex_number}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeUserCmnd:
                                    tran = await TransactionBuilder(message);
                                    //tran.TypeUserCmnd.nKeys = message.tran.usrcmd.nKeys;
                                    //tran.TypeUserCmnd.keys = new string(message.tran.usrcmd.keys);
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeAcr:
                                    tran = await TransactionBuilder(message);
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeAcrAccessControlFlag(message.tran.acr.actl_flags));
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeAcrSpareFlag(message.tran.acr.actl_flags_e));
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeUseLimit:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"Use count: {message.tran.c_uselimit.use_count}";
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_full.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCoSElevator:
                                    tran = await TransactionBuilder(message);
                                    tran.Remark = $"Floor: {message.tran.floor.floorNumber}";
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeCoSElevatorAccess:
                                    tran = await TransactionBuilder(message);
                                    holder = await qCred.GetCardHolderFullnameAndUserIdByCardNoAsync(message.tran.c_full.cardholder_id);
                        tran.Actor = holder[0];
                        tran.Image = holder[1];
                        tran.Remark = string.Join(" ", message.tran.elev_access.floors.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeAcrExtFeatureStls:
                                    tran = await TransactionBuilder(message);
                                    tran.TransactionFlags = AeroTransactionHandlerHelper.TypeAcrSpareFlag(message.tran.extfeat_stls.nExtFeatureType);
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              case (short)tranType.tranTypeAcrExtFeatureCoS:
                                    tran = await TransactionBuilder(message);
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeAcrSpareFlag(message.tran.extfeat_cos.nExtFeatureType));
                                    tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeCosStatus(message.tran.extfeat_cos.nStatus, message.tran.source_number, tranSrc.tranSrcACR));
                                    tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                                    break;
                              default:
                                    break;
                        }
                        break;
                  case (short)tranSrc.tranSrcAcrTmpr:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrTmpr);
                        break;
                  case (short)tranSrc.tranSrcAcrDoor:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeCosStatus(message.tran.door.door_status, message.tran.source_number, tranSrc.tranSrcAcrDoor));
                        tran.ExtendDesc += AeroTransactionHandlerHelper.TypeCosDoorAccessPointStatusString(message.tran.door.ap_status);
                        tran.TransactionFlags.AddRange(AeroTransactionHandlerHelper.TypeCosDoorAccessPointStatus(message.tran.door.ap_status));
                        break;
                  case (short)tranSrc.tranSrcAcrRex0:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
                        tran.TransactionFlags = AeroTransactionHandlerHelper.TypeCosStatus(message.tran.cos.status, message.tran.source_number, tranSrc.tranSrcAcrRex0);
                        break;
                  case (short)tranSrc.tranSrcAcrRex1:
                        tran = await TransactionBuilder(message);
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
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
                        mac = await qHw.GetMacFromComponentAsync((short)message.ScpId);
                        tran.Origin = await context.door.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.hardware_mac == mac && x.component_id == message.tran.source_number).Select(x => x.name).FirstOrDefaultAsync() ?? "";
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

      private async Task<Transaction> TransactionBuilder(IScpReply message)
        {
            var trand = AeroTransactionHandlerHelper.TranCodeDesc((tranType)message.tran.tran_type, message.tran.tran_code);
            var tran = new Aero.Domain.Entities.Transaction
            {
                // Base 
                ComponentId = (short)message.ScpId,
                Mac = await qHw.GetMacFromComponentAsync((short)message.ScpId),
                HardwareName = await qHw.GetNameByComponentIdAsync((short)message.ScpId),
                LocationId = await context.hardware
                  .AsNoTracking()
                  .OrderBy(h => h.component_id)
                  .Where(h => h.component_id == (short)message.ScpId)
                  .Select(h => h.location_id)
                  .FirstOrDefaultAsync(),
            //     created_date = DateTime.UtcNow,
            //     updated_date = DateTime.UtcNow,

                // extend_desc
                DateTime = UtilitiesHelper.UnixToDateTimeUtc(message.tran.time),
                SerialNumber = message.tran.ser_num,
                Source = message.tran.source_number,
                SourceModule = AeroTransactionHandlerHelper.Source((tranSrc)message.tran.source_type),
                SourceDesc = AeroTransactionHandlerHelper.SourceDesc((tranSrc)message.tran.source_type),
                Type = message.tran.tran_type,
                TypeDesc = AeroTransactionHandlerHelper.TypeDesc((tranType)message.tran.tran_type),
                TranCode = message.tran.tran_code,
                TranCodeDesc = trand[0],
                ExtendDesc = trand[1],
                TransactionFlags = new List<TransactionFlag>()
            };

            return tran;
        }
}
