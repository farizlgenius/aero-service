using System;
using System.Threading.Channels;
using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Adapter;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Aero.Infrastructure.Helpers.DescriptionHelper;

namespace Aero.Infrastructure.Listenser;

public sealed class AeroMessageListener(ILogger<AeroMessageListener> logger,Channel<IScpReply> queue, IServiceScopeFactory scopeFactory) 
{

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
        // using var scope = scopeFactory.CreateScope();
        switch (message.ReplyType)
        {
            // Occur when command to SCP not success
            case (int)enSCPReplyType.enSCPReplyNAK:
                //queue.Writer.TryWrite(new ScpReplyAdapter(message));
                MessageHandlerHelper.SCPReplyNakHandler(message,logger);
                break;
            case (int)enSCPReplyType.enSCPReplyTransaction:
                MessageHandlerHelper.SCPReplyTransactionHandler(message,queue,logger,scopeFactory);
                break;
            case (int)enSCPReplyType.enSCPReplyCommStatus:
                MessageHandlerHelper.SCPReplyCommStatus(message,logger);
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                break;
            case (int)enSCPReplyType.enSCPReplyIDReport:
                MessageHandlerHelper.SCPReplyIDReport(message);
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                break;
            case (int)enSCPReplyType.enSCPReplyTranStatus:
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                MessageHandlerHelper.SCPReplyTranStatus(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrMsp1Drvr:
                MessageHandlerHelper.SCPReplySrMsp1Drvr(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrSio:
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                MessageHandlerHelper.SCPReplySrSio(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrMp:
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                MessageHandlerHelper.SCPReplySrMp(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrCp:
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                MessageHandlerHelper.SCPReplySrCp(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrAcr:
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                MessageHandlerHelper.SCPReplySrAcr(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrTz:
                MessageHandlerHelper.SCPReplySrTz(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrTv:
                MessageHandlerHelper.SCPReplySrTv(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrMpg:
                MessageHandlerHelper.SCPReplySrMpg(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrArea:
                MessageHandlerHelper.SCPReplySrArea(message);
                break;
            case (int)enSCPReplyType.enSCPReplySioRelayCounts:
                MessageHandlerHelper.SCPReplySioRelayCounts(message);
                break;
            case (int)enSCPReplyType.enSCPReplyStrStatus:
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                MessageHandlerHelper.SCPReplyStrStatus(message);
                break;
            case (int)enSCPReplyType.enSCPReplyCmndStatus:
                // Save to DB if fail
                // commandService.HandleSaveFailCommand(command, message);
                MessageHandlerHelper.SCPReplyCmndStatus(message);
                //command.CompleteCommand($"{message.SCPId}/{message.cmnd_sts.sequence_number}",message.cmnd_sts.status == 1);
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                break;
            case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                MessageHandlerHelper.SCPReplyWebConfigNetwork(message);
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
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
                queue.Writer.TryWrite(new ScpReplyAdapter(message));
                break;

            default:
                break;
        }
    }

   


}


