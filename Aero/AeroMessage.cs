using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Aero.CommandService;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.Utility;
using System;
using System.Threading.Channels;


namespace HIDAeroService.AeroLibrary
{
    public sealed class AeroMessage(AeroCommandService command, IServiceScopeFactory scopeFactory,ILogger<AeroMessage> logger, Channel<SCPReplyMessage> queue)
    {

        public bool isWaitingCardScan { private get; set; }
        public short ScanScpId { private get; set; }
        public short ScanAcrNo {private get; set;}
        private bool shutdownFlag;

        public void SetShutDownflag()
        {
            SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
            Thread.Sleep(100);
            shutdownFlag = true;
        }


        public void GetTransactionUntilShutDown()
        {
            while (shutdownFlag == false)
            {
                GetTransaction();
            }
        }

        private void GetTransaction()
        {
            SCPReplyMessage message = new SCPReplyMessage();
            if (message.GetMessage())
            {
                ProcessMessage(message);
            }

        }

        private void ProcessMessage(SCPReplyMessage message)
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
                    queue.Writer.TryWrite(message);
                    transactionService.TriggerEventRecieve();
                    break;
                case (int)enSCPReplyType.enSCPReplyCommStatus:
                    hardwareService.TriggerDeviceStatus(helperService.GetMacFromId((short)message.SCPId),message.comm.status);                  
                    queue.Writer.TryWrite(message);
                    handle.SCPReplyCommStatus(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyIDReport:
                    handle.SCPReplyIDReport(message);
                    queue.Writer.TryWrite(message);       
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
                    queue.Writer.TryWrite(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyCmndStatus:
                    // Save to DB if fail
                    commandService.HandleSaveFailCommand(command,message);
                    handle.SCPReplyCmndStatus(message,command);
                    //command.CompleteCommand($"{message.SCPId}/{message.cmnd_sts.sequence_number}",message.cmnd_sts.status == 1);
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                    handle.SCPReplyWebConfigNetwork(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigNotes:
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigSessionTmr:
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigWebConn:
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigAutoSave:
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigNetDiag:
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigTimeServer:
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigDiagnostics:
                    break;
                case (int)enSCPReplyType.enSCPReplyWebConfigHostCommPrim:
                    queue.Writer.TryWrite(message);
                    break;
                   
                default:
                    break;
            }
        }
    }
}
