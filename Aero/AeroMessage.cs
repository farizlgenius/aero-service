using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
using HIDAeroService.Entity;
using HIDAeroService.Model;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using HIDAeroService.Utility;
using Microsoft.Extensions.DependencyInjection;


namespace HIDAeroService.AeroLibrary
{
    public sealed class AeroMessage(AeroCommand command, IServiceScopeFactory scopeFactory)
    {
        private bool _shutdownFlag;
        private List<IdReport> report = new List<IdReport>();

        public List<IdReport> iDReports { get; set; } = new List<IdReport>();
        public IdReport iDReport { get; set; }
        public bool isWaitingCardScan { private get; set; }
        public short ScanScpId { private get; set; }
        public short ScanAcrNo {private get; set;} 


        public void SetShutDownflag()
        {
            SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
            Thread.Sleep(100);
            _shutdownFlag = true;
        }

        public void GetTransactionUntilShutDown()
        {
            while (_shutdownFlag == false)
            {
                // GetAsync Transaction here
                GetTransaction();
            }
        }

        private async void GetTransaction()
        {
            SCPReplyMessage message = new SCPReplyMessage();
            if (message.GetMessage())
            {
                await ProcessMessage(message);
            }

        }

        private async Task ProcessMessage(SCPReplyMessage message)
        {
            using var scope = scopeFactory.CreateScope();
            var handle = scope.ServiceProvider.GetRequiredService<MessageHandler>();
            var hardwareService = scope.ServiceProvider.GetRequiredService<IHardwareService>();
            var transactionService = scope.ServiceProvider.GetRequiredService<ITransactionService>();
            var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
            var doorService = scope.ServiceProvider.GetRequiredService<IDoorService>();
            var controlPointService = scope.ServiceProvider.GetRequiredService<IControlPointService>();
            var monitorPointService = scope.ServiceProvider.GetRequiredService<IMonitorPointService>();
            var helperService = scope.ServiceProvider.GetRequiredService<IHelperService>();
            var commandService = scope.ServiceProvider.GetRequiredService<ICommandService>();
            switch (message.ReplyType)
            {
                // Occur when command to SCP not success
                case (int)enSCPReplyType.enSCPReplyNAK:
                    handle.SCPReplyNAKHandler(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyTransaction:
                    handle.SCPReplyTransactionHandler(message,isWaitingCardScan,ScanScpId,ScanAcrNo);
                    await transactionService.SaveToDatabase(message);
                    transactionService.TriggerEventRecieve();
                    break;
                case (int)enSCPReplyType.enSCPReplyCommStatus:
                    hardwareService.TriggerDeviceStatus(helperService.GetMacFromId((short)message.SCPId),message.comm.status);
                    if (message.comm.status != 2)
                    {
                        iDReports.RemoveAll(obj => obj.ScpId == message.SCPId);
                        hardwareService.TriggerIdReport(iDReports);
                    }
                    handle.SCPReplyCommStatus(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyIDReport:
                    handle.SCPReplyIDReport(message);
                    // If Controller not register yet
                    iDReport = await hardwareService.HandleFoundHardware(message);
                    if(iDReport != null)
                    {
                        iDReports.Add(iDReport);
                    }
                    break;
                case (int)enSCPReplyType.enSCPReplyTranStatus:
                    hardwareService.TriggerTranStatus(message);
                    handle.SCPReplyTranStatus(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrMsp1Drvr:
                    handle.SCPReplySrMsp1Drvr(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrSio:
                    moduleService.TriggerDeviceStatus(message.SCPId, message.sts_sio.number, DecodeHelper.TypeSioCommTranCodeDecode(message.sts_sio.com_status), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[4])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[5])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[6])));
                    handle.SCPReplySrSio(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrMp:
                    monitorPointService.TriggerDeviceStatus(message.SCPId, message.sts_mp.first,DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_mp.status[0])));
                    handle.SCPReplySrMp(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrCp:
                    handle.SCPReplySrCp(message);
                    controlPointService.TriggerDeviceStatus(helperService.GetMacFromId((short)message.SCPId), message.sts_cp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_cp.status[0])));
                    break;
                case (int)enSCPReplyType.enSCPReplySrAcr:
                    handle.SCPReplySrAcr(message);
                    doorService.TriggerDeviceStatus(message.SCPId, message.sts_acr.number, Description.GetACRModeForStatus(message.sts_acr.door_status), Description.GetAccessPointStatusFlagResult((byte)message.sts_acr.ap_status));
                    break;
                case (int)enSCPReplyType.enSCPReplySrTz:
                    handle.SCPReplySrTz(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrTv:
                    handle.SCPReplySrTv(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrMpg:
                    handle.SCPReplySrMpg(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrArea:
                    handle.SCPReplySrArea(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySioRelayCounts:
                    handle.SCPReplySioRelayCounts(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyStrStatus:
                    handle.SCPReplyStrStatus(message);
                    await hardwareService.VerifyAllocateHardwareMemoryAsync(message);
                     break;
                case (int)enSCPReplyType.enSCPReplyCmndStatus:
                    // Save to DB if fail
                    commandService.HandleSaveFailCommand(command,message);
                    hardwareService.HandleUploadCommand(command,message);
                    handle.SCPReplyCmndStatus(message,command);
                    command.CommandResultTrigger(message.cmnd_sts.sequence_number,message.cmnd_sts.status == 1 ? true : false);
                        break;
                case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                    handle.SCPReplyWebConfigNetwork(message);
                    hardwareService.AssignIpToIdReport(message, iDReports);
                    
                    break;
                    
                default:
                    break;
            }
        }
    }
}
