
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

namespace Aero.Infrastructure.Mapper
{
    public sealed class AeroWorker(Channel<SCPReplyMessage> queue, IServiceScopeFactory scopeFactory, INotificationPublisher publisher) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var message in queue.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = scopeFactory.CreateScope();
                var qhw = scope.ServiceProvider.GetRequiredService<IQHwRepository>();
                var hw = scope.ServiceProvider.GetRequiredService<IHardwareService>();
                var rhw = scope.ServiceProvider.GetRequiredService<IHwRepository>();
                var scp = scope.ServiceProvider.GetRequiredService<IScpCommand>();
                var rId = scope.ServiceProvider.GetRequiredService<IQIdReportRepository>();

                try
                {
                    switch (message.ReplyType)
                    {
                        case (int)enSCPReplyType.enSCPReplyTransaction:
                            await tran.SaveToDatabaseAsync(message);
                            switch (message.tran.tran_type)
                            {
                                case (short)tranType.tranTypeSioComm:
                                    await md.HandleFoundModuleAsync(message);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case (int)enSCPReplyType.enSCPReplyIDReport:
                            await HandleFoundHardware(message,qhw,hw,rhw,scp);
                            break;
                        case (int)enSCPReplyType.enSCPReplyCommStatus:
                            if (message.comm.status != 2)
                            {
                                var reports = await hw.RemoveIdReportAsync(message);
                                hw.TriggerIdReport(reports.ToList());
                            }
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
                        case (int)enSCPReplyType.enSCPReplyStrStatus:
                            await hw.VerifyAllocateHardwareMemoryAsync(message);
                            break;
                        case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                            await hw.AssignIpAddress(message);
                            break;
                        case (int)enSCPReplyType.enSCPReplyWebConfigHostCommPrim:
                            await hw.AssignPort(message);

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


        public async Task HandleFoundHardware(SCPReplyMessage message,IQHwRepository qhw,IHardwareService hw,IHwRepository rhw,IScpCommand scp,IQIdReportRepository rId)
        {
            if(await qhw.IsAnyByMac(UtilitiesHelper.ByteToHexStr(message.id.mac_addr)))
            {
                var hardware = await rhw.GetByMacAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr));

                if (hardware is null) return;

                if (!await hw.MappingHardwareAndAllocateMemory(message.id.scp_id))
                {
                    hardware.IsReset = true;
                }
                else
                {
                    hardware.IsReset = false;
                }

                if (!await hw.VerifyMemoryAllocateAsync(hardware.Mac))
                {
                    hardware.IsReset = true;
                }
                else
                {
                    hardware.IsReset = false;
                }

                hardware.Firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);

                var component = await hw.VerifyDeviceConfigurationAsync(hardware);

                hardware.IsUpload = component.Any(s => s.IsUpload == true);

                var status = await rhw.UpdateAsync(hardware);

                if(status <= 0) return;

                // Call Get ip
                scp.GetWebConfigRead(message.id.scp_id, 2);


            }
            else
            {
                if (!await hw.VerifyHardwareConnection(message.id.scp_id)) return;


                if(await qhw.IsAnyByMacAndComponent(UtilitiesHelper.ByteToHexStr(message.id.mac_addr),message.id.scp_id))
                {

                    var iDReport = await rId.GetByMacAndComponentIdAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr),message.id.scp_id);

                    if (iDReport is null) return;
                    IdReportMapper.Update(iDReport,message);
                    context.id_report.Update(iDReport);
                }
                else
                {
                    short id = await helper.GetLowestUnassignedNumberNoLimitAsync<Hardware>(context);
                    var idReport = IdReportMapper.ToDomain(message,scp,id);
                    await context.id_report.AddAsync(iDReport);
                }



                scp.GetWebConfigRead(message.id.scp_id, 2);


            }

        }



    }
}
