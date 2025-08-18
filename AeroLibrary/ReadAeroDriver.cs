using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Models;
using HIDAeroService.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;

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
        public SIOStatusReport sIOStatusReport { get; set; }
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
            var cp = scope.ServiceProvider.GetRequiredService<CpService>();
            var mp = scope.ServiceProvider.GetRequiredService<MpService>();
            var tz = scope.ServiceProvider.GetRequiredService<TZService>();
            var cfmt = scope.ServiceProvider.GetRequiredService<CardFormatService>();
            var alvl = scope.ServiceProvider.GetRequiredService<AlvlService>();
            switch (message.ReplyType)
            {
                // Occur when command to SCP not success
                case (int)enSCPReplyType.enSCPReplyNAK:
                    Console.WriteLine("SCPReplyNAK");
                    handle.SCPReplyNAKHandler(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyTransaction:
                    Console.WriteLine("SCPReplyTransaction");
                    handle.SCPReplyTransactionHandler(message,isWaitingCardScan,ScanScpId,ScanAcrNo); 
                    break;
                case (int)enSCPReplyType.enSCPReplyCommStatus:
                    Console.WriteLine("SCPReplyCommStatus");
                    handle.SCPReplyCommStatus(message);
                    break;
                case (int)enSCPReplyType.enSCPReplyIDReport:
                    Console.WriteLine("SCPReplyIDReport");
                    _write.GetWebConfig(message.id.scp_id);
                    handle.SCPReplyIDReport(message); 
                    iDReport = new IDReport();
                    iDReport.DeviceID = message.id.device_id;
                    iDReport.DeviceVer = message.id.device_ver;
                    iDReport.SoftwareRevMajor = message.id.sft_rev_major;
                    iDReport.SoftwareRevMinor = message.id.sft_rev_minor;   
                    iDReport.SerialNumber = message.id.serial_number;
                    iDReport.RamSize = message.id.ram_size;
                    iDReport.RamFree = message.id.ram_free;
                    iDReport.ESec = Utility.UnixToDateTime(message.id.e_sec);
                    iDReport.DatabaseMax = message.id.db_max;
                    iDReport.DatabaseActive = message.id.db_active;
                    iDReport.DipSwitchPowerUp = message.id.dip_switch_pwrup;
                    iDReport.DipSwitchCurrent = message.id.dip_switch_current;
                    iDReport.ScpID = message.id.scp_id;
                    iDReport.FirmWareAdvisory = message.id.firmware_advisory;
                    iDReport.ScpIn1 = message.id.scp_in_1;
                    iDReport.ScpIn2 = message.id.scp_in_2;
                    iDReport.NOemCode = message.id.nOemCode;
                    iDReport.ConfigFlag = message.id.config_flags;
                    iDReport.MacAddress = Utility.ByteToHexStr(message.id.mac_addr);
                    iDReport.TlsStatus = message.id.tls_status;
                    iDReport.OperMode = message.id.oper_mode;
                    iDReport.ScpIn3 = message.id.scp_in_3;
                    iDReport.CumulativeBldCnt = message.id.cumulative_bld_cnt;
                    if ((message.id.config_flags & 1) != 0)
                    {
                        if (scp.ValidateSCPConnection(message.id.scp_id, tz, cfmt, alvl))
                        {
                            report.Add(iDReport);
                        }

                    }
                    break;
                case (int)enSCPReplyType.enSCPReplyTranStatus:
                    Console.WriteLine("SCPReplyTranStatus");
                    handle.SCPReplyTranStatus(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrMsp1Drvr:
                    Console.WriteLine("SCPReplySrMsp1Drvr");
                    handle.SCPReplySrMsp1Drvr(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrSio:
                    Console.WriteLine("SCPReplySrSio");
                    handle.SCPReplySrSio(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrMp:
                    Console.WriteLine("SCPReplySrMp");
                    handle.SCPReplySrMp(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrCp:
                    Console.WriteLine("SCPReplySrCp");
                    handle.SCPReplySrCp(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrAcr:
                    Console.WriteLine("SCPReplySrAcr");
                    handle.SCPReplySrAcr(message);
                    break;
                case (int)enSCPReplyType.enSCPReplySrTz:
                    Console.WriteLine("SCPReplySrTz");
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
                    break;
                case (int)enSCPReplyType.enSCPReplyCmndStatus:
                    handle.SCPReplyCmndStatus(message,_write);
                        break;
                case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                    foreach(var i in report)
                    {
                        if(i.ScpID == message.SCPId)
                        {
                            i.Ip = message.web_network.cIpAddr;
                        }

                        if (scp.IsIpRegister(Utility.IntegerToIp(i.Ip)))
                        {
                            scp.InitialScpConfiguration(i.ScpID, cp, mp,i.MacAddress);
                        }
                        else
                        {
                            iDReports.Add(iDReport);
                        }
                    }
                    handle.SCPReplyWebConfigNetwork(message);
                    break;
                    
                default:
                    break;
            }
        }
    }
}
