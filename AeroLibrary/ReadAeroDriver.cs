using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Constants;
using HIDAeroService.Models;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using HIDAeroService.Service.Interface;


namespace HIDAeroService.AeroLibrary
{
    public sealed class ReadAeroDriver
    {
        private bool _shutdownFlag;
        private readonly WriteAeroDriver _write;
        private readonly IServiceScopeFactory _scopeFactory;
        private List<IDReport> report = new List<IDReport>();

        public List<IDReport> iDReports { get; set; } = new List<IDReport>();
        public IDReport iDReport { get; set; }
        public bool isWaitingCardScan { private get; set; }
        public short ScanScpId { private get; set; }
        public short ScanAcrNo {private get; set;} 

        public ReadAeroDriver(WriteAeroDriver write, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _write = write;
            _shutdownFlag = false;
        }

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
                // Get Transaction here
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
            using var scope = _scopeFactory.CreateScope();
            var handle = scope.ServiceProvider.GetRequiredService<MessageHandler>();
            var scp = scope.ServiceProvider.GetRequiredService<ScpService>();
            var sio = scope.ServiceProvider.GetRequiredService<SioService>();
            var acr = scope.ServiceProvider.GetRequiredService<AcrService>();
            var cp = scope.ServiceProvider.GetRequiredService<CpService>();
            var mp = scope.ServiceProvider.GetRequiredService<MpService>();
            var tz = scope.ServiceProvider.GetRequiredService<TimeZoneService>();
            var cfmt = scope.ServiceProvider.GetRequiredService<ICardFormatService>();
            var alvl = scope.ServiceProvider.GetRequiredService<IAccessLevelService>();
            var helper = scope.ServiceProvider.GetRequiredService<HelperService>();
            var sys = scope.ServiceProvider.GetRequiredService<SysService>();
            var cmnd = scope.ServiceProvider.GetRequiredService<CmndService>();
            switch (message.ReplyType)
            {
                // Occur when command to SCP not success
                case (int)enSCPReplyType.enSCPReplyNAK:
                    handle.SCPReplyNAKHandler(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyTransaction:
                    handle.SCPReplyTransactionHandler(message,isWaitingCardScan,ScanScpId,ScanAcrNo); 
                    break;
                case (int)enSCPReplyType.enSCPReplyCommStatus:
                    scp.TriggerDeviceStatus(helper.GetMacFromId((short)message.SCPId),message.comm.status);
                    if (message.comm.status != 2)
                    {
                        iDReports.RemoveAll(obj => obj.ScpID == message.SCPId);
                        scp.TriggerIdReport(iDReports);
                    }
                    handle.SCPReplyCommStatus(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyIDReport:
                    handle.SCPReplyIDReport(message);
                    // If Controller not register yet
                    iDReport = scp.handleInCommingScp(message,tz,cfmt,alvl);
                    if(iDReport != null)
                    {
                        iDReports.Add(iDReport);
                    }
                    break;
                case (int)enSCPReplyType.enSCPReplyTranStatus:
                    handle.SCPReplyTranStatus(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrMsp1Drvr:
                    handle.SCPReplySrMsp1Drvr(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrSio:
                    sio.TriggerDeviceStatus(message.SCPId, message.sts_sio.number, message.sts_sio.com_status, message.sts_sio.ip_stat[4], message.sts_sio.ip_stat[5], message.sts_sio.ip_stat[6]);
                    handle.SCPReplySrSio(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrMp:
                    mp.TriggerDeviceStatus(message.SCPId, message.sts_mp.first, message.sts_mp.status);
                    handle.SCPReplySrMp(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrCp:
                    handle.SCPReplySrCp(message);
                    cp.TriggerDeviceStatus(helper.GetMacFromId((short)message.SCPId), message.sts_cp.first, message.sts_cp.status);
                    break;
                case (int)enSCPReplyType.enSCPReplySrAcr:
                    handle.SCPReplySrAcr(message);
                    acr.TriggerDeviceStatus(message.SCPId, message.sts_acr.number, Description.GetACRModeForStatus(message.sts_acr.door_status), Description.GetAccessPointStatusFlagResult((byte)message.sts_acr.ap_status));
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
                    scp.VerifyScpConfiguration(message);
                     break;
                case (int)enSCPReplyType.enSCPReplyCmndStatus:
                    // Save to DB if fail
                    cmnd.HandleSaveFailCommand(_write,message);
                    cmnd.HandleCommandResponse(message);
                    scp.HandleUploadCommand(_write,message);
                    handle.SCPReplyCmndStatus(message,_write);
                        break;
                case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                    handle.SCPReplyWebConfigNetwork(message);
                    scp.AssignIpToIdReport(message, iDReports);
                    
                    break;
                    
                default:
                    break;
            }
        }
    }
}
