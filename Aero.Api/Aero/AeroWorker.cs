
using HID.Aero.ScpdNet.Wrapper;
using AeroService.Data;
using AeroService.Entity;
using AeroService.Model;
using AeroService.Service;
using AeroService.Service.Impl;
using Newtonsoft.Json.Linq;
using System.Threading.Channels;

namespace AeroService.Aero
{
    public sealed class AeroWorker(Channel<SCPReplyMessage> queue, IServiceScopeFactory scopeFactory) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var message in queue.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var tran = scope.ServiceProvider.GetRequiredService<ITransactionService>();
                var hw = scope.ServiceProvider.GetRequiredService<IHardwareService>();
                var md = scope.ServiceProvider.GetRequiredService<IModuleService>();

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
                            await hw.HandleFoundHardware(message);
                            break;
                        case (int)enSCPReplyType.enSCPReplyCommStatus:
                            if (message.comm.status != 2)
                            {
                                var reports = await hw.RemoveIdReportAsync(message);
                                hw.TriggerIdReport(reports.ToList());
                            }
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

    }
}
