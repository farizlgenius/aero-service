
using HID.Aero.ScpdNet.Wrapper;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Aero.Infrastructure.Data;
using Aero.Application.Interfaces;
using Aero.Application.DTOs;
using Aero.Infrastructure.Services;
using Aero.Application.Services;
using Aero.Application.Helpers;
using Aero.Domain.Interface;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Aero.Infrastructure.Helpers;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Mapper
{
    public sealed class AeroWorker(Channel<IScpReply> queue, IServiceScopeFactory scopeFactory, INotificationPublisher publisher) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            await foreach (var message in queue.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = scopeFactory.CreateScope();
                var qhw = scope.ServiceProvider.GetRequiredService<IQHwRepository>();    
                var rhw = scope.ServiceProvider.GetRequiredService<IHwRepository>();
                var hw = scope.ServiceProvider.GetRequiredService<IHardwareService>();
                
            
                try
                {
                    switch (message.ReplyType)
                    {
                        case (int)enSCPReplyType.enSCPReplyNAK:
                        break;
                        case (int)enSCPReplyType.enSCPReplyTransaction:
                            await SaveToDatabaseAsync(message);
                            switch (message.TranType)
                            {
                                case (short)tranType.tranTypeSioComm:
                                    await md.HandleFoundModuleAsync(message);
                                    break;
                                case (short)tranType.tranTypeCardFull:
                                    var status = new CardScanStatus
                                    {
                                        Mac = "",
                                        FormatNumber = "",
                                        Fac = "",
                                        CardId = "",
                                        Issue = "",
                                        Floor = ""
                                    };
                                    await publisher.CardScanNotifyStatus(status);
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
                                var reports = await qId.DeletePendingRecordAsync(UtilitiesHelper.ByteToHex(message.id.mac_addr),(short)message.ScpId);
                                await publisher.IdReportNotifyAsync(reports.ToList());
                                break;
                            }
                            var s = scp.CheckSCPStatus((short)message.ScpId);
                            var ss = new ScpStatus(UtilitiesHelper.ByteToHex(message.id.mac_addr),s);
                            await publisher.ScpNotifyStatus(ss);
                            break;
                        case (int)enSCPReplyType.enSCPReplyTranStatus:
                            TranStatus t = new TranStatus
                            {
                                MacAddress = await hw.GetMacFromComponentAsync((short)message.SCPId),
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
                            var mpstatus = new MpStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId),message.sts_mp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_mp.status[0])));
                            await publisher.MpNotifyStatus(mpstatus);
                            break;
                        case (int)enSCPReplyType.enSCPReplySrCp:
                            var cpstatus = new CpStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId),message.sts_cp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_cp.status[0])));
                            await publisher.CpNotifyStatus(cpstatus);
                            break;
                        case (int)enSCPReplyType.enSCPReplySrAcr:
                            var acrstatus = new AcrStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId),message.sts_acr.number, DescriptionHelper.GetACRModeForStatus(message.sts_acr.door_status), DescriptionHelper.GetAccessPointStatusFlagResult((byte)message.sts_acr.ap_status));
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
