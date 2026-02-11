
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Services;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Adapter;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Helpers;
using Aero.Infrastructure.Services;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.NetworkInformation;
using System.Threading.Channels;

namespace Aero.Infrastructure.Mapper
{
    public sealed class AeroWorker(Channel<IScpReply> queue, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        public bool isWaitingCardScan { private get; set; }
        public short ScanScpId { private get; set; }
        public short ScanAcrNo { private get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await foreach (var message in queue.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = scopeFactory.CreateScope();
                var qhw = scope.ServiceProvider.GetRequiredService<IQHwRepository>();
                var rhw = scope.ServiceProvider.GetRequiredService<IHwRepository>();
                var hw = scope.ServiceProvider.GetRequiredService<IHardwareService>();
                var tran = scope.ServiceProvider.GetRequiredService<ITransactionService>();
                var publisher = scope.ServiceProvider.GetRequiredService<INotificationPublisher>();


                try
                {
                    switch (message.ReplyType)
                    {
                        case (int)enSCPReplyType.enSCPReplyNAK:
                            break;
                        case (int)enSCPReplyType.enSCPReplyTransaction:
                            await tran.SaveToDatabaseAsync(message);
                            switch (message.TranType)
                            {
                                case (short)tranType.tranTypeSioComm:
                                    // await md.HandleFoundModuleAsync(message);
                                    break;
                                case (short)tranType.tranTypeCardFull:
                                    if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                                    {
                                        var status = new CardScanStatus
                                        {
                                            Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                                            FormatNumber = message.tran.c_full.format_number,
                                            Fac = message.tran.c_full.facility_code,
                                            CardId = message.tran.c_full.cardholder_id,
                                            Issue = message.tran.c_full.issue_code,
                                            Floor = message.tran.c_full.floor_number
                                        };
                                        await publisher.CardScanNotifyStatus(status);
                                        isWaitingCardScan = false;
                                        ScanAcrNo = -1;
                                        ScanScpId = -1;
                                    }
                                    
                                    break;
                                case (short)tranType.tranTypeDblCardFull:
                                    if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                                    {
                                        var status = new CardScanStatus
                                        {
                                            Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                                            FormatNumber = message.tran.c_fulldbl.format_number,
                                            Fac = message.tran.c_fulldbl.facility_code,
                                            CardId = message.tran.c_fulldbl.cardholder_id,
                                            Issue = message.tran.c_fulldbl.issue_code,
                                            Floor = message.tran.c_fulldbl.floor_number
                                        };
                                        await publisher.CardScanNotifyStatus(status);
                                        isWaitingCardScan = false;
                                        ScanAcrNo = -1;
                                        ScanScpId = -1;
                                    }
                                    
                                    break;
                                case (short)tranType.tranTypeI64CardFull:
                                    if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                                    {
                                        var status = new CardScanStatus
                                        {
                                            Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                                            FormatNumber = message.tran.c_fulli64.format_number,
                                            Fac = message.tran.c_fulli64.facility_code,
                                            CardId = message.tran.c_fulli64.cardholder_id,
                                            Issue = message.tran.c_fulli64.issue_code,
                                            Floor = message.tran.c_fulli64.floor_number
                                        };
                                        await publisher.CardScanNotifyStatus(status);
                                        isWaitingCardScan = false;
                                        ScanAcrNo = -1;
                                        ScanScpId = -1;
                                    }
                                    
                                    break;
                                case (short)tranType.tranTypeI64CardFullIc32:
                                    if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                                    {
                                        var status = new CardScanStatus
                                        {
                                            Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                                            FormatNumber = message.tran.c_fulli64i32.format_number,
                                            Fac = message.tran.c_fulli64i32.facility_code,
                                            CardId = message.tran.c_fulli64i32.cardholder_id,
                                            Issue = message.tran.c_fulli64i32.issue_code,
                                            Floor = message.tran.c_fulli64i32.floor_number
                                        };
                                        await publisher.CardScanNotifyStatus(status);
                                        isWaitingCardScan = false;
                                        ScanAcrNo = -1;
                                        ScanScpId = -1;
                                    }
                                    
                                    break;
                                case (short)tranType.tranTypeCardID:
                                    break;
                                case (short)tranType.tranTypeDblCardID:
                                    break;
                                case (short)tranType.tranTypeI64CardID:
                                    break;
                                case (short)tranType.tranTypeCoS:
                                    switch (message.tran.source_type)
                                    {
                                        case (short)tranSrc.tranSrcSioCom:
                                            // moduleService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, DecodeHelper.TypeSioCommStatusDecode(message.tran.cos.status), null, null, null);
                                            // publisher
                                            break;
                                        case (short)tranSrc.tranSrcMP:
                                            var mp = new MpStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.tran.source_number, DecodeHelper.TypeCosStatusDecode(message.tran.cos.status));
                                            await publisher.MpNotifyStatus(mp);
                                            break;
                                        case (short)tranSrc.tranSrcCP:
                                            var cp = new CpStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.tran.source_number, DecodeHelper.TypeCosStatusDecode(message.tran.cos.status));
                                            await publisher.CpNotifyStatus(cp);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case (short)tranType.tranTypeREX:
      
                                    break;
                                case (short)tranType.tranTypeCoSDoor:
                                    var doorstatus = new AcrStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId),message.tran.source_number,"",DescriptionHelper.GetAccessPointStatusFlagResult(message.tran.door.ap_status));
                                    await publisher.AcrNotifyStatus(doorstatus);
                                    break;
                                case (short)tranType.tranTypeProcedure:

                                    break;
                                case (short)tranType.tranTypeUserCmnd:
           
                                    break;
                                case (short)tranType.tranTypeActivate:
   
                                    break;
                                case (short)tranType.tranTypeAcr:
                                    var modestatus = new AcrStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId),message.tran.source_number,DescriptionHelper.GetAcrModeForStatus(message.tran.tran_code),"");
                                    await publisher.AcrNotifyStatus(modestatus);
                                    break;
                                case (short)tranType.tranTypeMpg:

                                    break;
                                case (short)tranType.tranTypeArea:

                                    break;
                                case (short)tranType.tranTypeUseLimit:

                                    break;
                                case (short)tranType.tranTypeWebActivity:

                                    break;
                                case (short)tranType.tranTypeOperatingMode:

                                    break;
                                case (short)tranType.tranTypeCoSElevator:

                                    break;
                                case (short)tranType.tranTypeFileDownloadStatus:

                                    break;
                                case (short)tranType.tranTypeCoSElevatorAccess:

                                    break;
                                case (short)tranType.tranTypeAcrExtFeatureStls:

                                    break;
                                case (short)tranType.tranTypeAcrExtFeatureCoS:

                                    break;
                                case (short)tranType.tranTypeAsci:

                                    break;
                                case (short)tranType.tranTypeSioDiag:

                                    break;
                                default:
                                    break;
                            }
                            await publisher.EventNotifyRecieve();
                            break;
                        case (int)enSCPReplyType.enSCPReplyIDReport:
                            await hw.HandleFoundHardware(message);
                            break;
                        case (int)enSCPReplyType.enSCPReplyCommStatus:
                            hw = scope.ServiceProvider.GetRequiredService<IHardwareService>();
                            var scp = scope.ServiceProvider.GetRequiredService<IScpCommand>();
                            var qId = scope.ServiceProvider.GetRequiredService<IQIdReportRepository>();
                            if (message.comm.status != 2)
                            {
                                var reports = await qId.DeletePendingRecordAsync((short)message.ScpId);
                                await publisher.IdReportNotifyAsync(reports.ToList());
                                break;
                            }                            
                            break;
                        case (int)enSCPReplyType.enSCPReplyTranStatus:
                            TranStatus t = new TranStatus
                            {
                                MacAddress = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                                Capacity = message.tran_sts.capacity,
                                Oldest = message.tran_sts.oldest,
                                LastLog = message.tran_sts.last_loggd,
                                LastReport = message.tran_sts.last_rprtd,
                                Disabled = message.tran_sts.disabled,
                                Status = message.tran_sts.disabled == 0 ? "Enable" : "Disable"

                            };
                            await publisher.ScpNotifyTranStatus(t);
                            break;
                        case (int)enSCPReplyType.enSCPReplySrSio:
                            var siostatus = new SioStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.sts_sio.number, DecodeHelper.TypeSioCommTranCodeDecode(message.sts_sio.com_status), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[4])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[5])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[6])));
                            await publisher.SioNotifyStatus(siostatus);
                            break;
                        case (int)enSCPReplyType.enSCPReplySrMp:
                            var mpstatus = new MpStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.sts_mp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_mp.status[0])));
                            await publisher.MpNotifyStatus(mpstatus);
                            break;
                        case (int)enSCPReplyType.enSCPReplySrCp:
                            var cpstatus = new CpStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.sts_cp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_cp.status[0])));
                            await publisher.CpNotifyStatus(cpstatus);
                            break;
                        case (int)enSCPReplyType.enSCPReplySrAcr:
                            var acrstatus = new AcrStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.sts_acr.number, DescriptionHelper.GetAcrModeForStatus(message.sts_acr.door_status), DescriptionHelper.GetAccessPointStatusFlagResult((byte)message.sts_acr.ap_status));
                            await publisher.AcrNotifyStatus(acrstatus);
                            break;
                        case (int)enSCPReplyType.enSCPReplyStrStatus:
                            await hw.VerifyAllocateHardwareMemoryAsync(message);
                            break;
                        case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                            await hw.AssignIpAddressAsync(message);
                            break;
                        case (int)enSCPReplyType.enSCPReplyWebConfigHostCommPrim:
                            await hw.AssignPortAsync(message);

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {

                }


            }


        }












    }
}
