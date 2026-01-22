using System;
using System.Threading.Channels;
using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Aero.Infrastructure.Helpers.DescriptionHelper;

namespace Aero.Infrastructure.Services;

public sealed class AeroMessageListener(Channel<SCPReplyMessage> queue, IServiceScopeFactory scopeFactory) : IAeroMessage
{

    public bool isWaitingCardScan { private get; set; }
    public short ScanScpId { private get; set; }
    public short ScanAcrNo { private get; set; }
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
                MessageHandlerHelper.SCPReplyNAKHandler(message);
                break;
            case (int)enSCPReplyType.enSCPReplyTransaction:
                MessageHandlerHelper.SCPReplyTransactionHandler(message, isWaitingCardScan, ScanScpId, ScanAcrNo);
                queue.Writer.TryWrite(message);
                transactionService.TriggerEventRecieve();
                break;
            case (int)enSCPReplyType.enSCPReplyCommStatus:
                hardwareService.TriggerDeviceStatus(helperService.GetMacFromId((short)message.SCPId), message.comm.status);
                queue.Writer.TryWrite(message);
                MessageHandlerHelper.SCPReplyCommStatus(message);
                break;
            case (int)enSCPReplyType.enSCPReplyIDReport:
                MessageHandlerHelper.SCPReplyIDReport(message);
                queue.Writer.TryWrite(message);
                break;
            case (int)enSCPReplyType.enSCPReplyTranStatus:
                queue.Writer.TryWrite(message);
                hardwareService.TriggerTranStatus(message);
                MessageHandlerHelper.SCPReplyTranStatus(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrMsp1Drvr:
                MessageHandlerHelper.SCPReplySrMsp1Drvr(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrSio:
                moduleService.TriggerDeviceStatus(message.SCPId, message.sts_sio.number, DecodeHelper.TypeSioCommTranCodeDecode(message.sts_sio.com_status), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[4])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[5])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[6])));
                MessageHandlerHelper.SCPReplySrSio(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrMp:
                monitorPointService.TriggerDeviceStatus(message.SCPId, message.sts_mp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_mp.status[0])));
                MessageHandlerHelper.SCPReplySrMp(message);
                break;
            case (int)enSCPReplyType.enSCPReplySrCp:
                MessageHandlerHelper.SCPReplySrCp(message);
                controlPointService.TriggerDeviceStatus(helperService.GetMacFromId((short)message.SCPId), message.sts_cp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_cp.status[0])));
                break;
            case (int)enSCPReplyType.enSCPReplySrAcr:
                MessageHandlerHelper.SCPReplySrAcr(message);
                doorService.TriggerDeviceStatus(message.SCPId, message.sts_acr.number, DescriptionHelper.GetACRModeForStatus(message.sts_acr.door_status), DescriptionHelper.GetAccessPointStatusFlagResult((byte)message.sts_acr.ap_status));
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
                MessageHandlerHelper.SCPReplyStrStatus(message);
                queue.Writer.TryWrite(message);
                break;
            case (int)enSCPReplyType.enSCPReplyCmndStatus:
                // Save to DB if fail
                commandService.HandleSaveFailCommand(command, message);
                MessageHandlerHelper.SCPReplyCmndStatus(message, command);
                //command.CompleteCommand($"{message.SCPId}/{message.cmnd_sts.sequence_number}",message.cmnd_sts.status == 1);
                break;
            case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                MessageHandlerHelper.SCPReplyWebConfigNetwork(message);
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

    public async Task VerifyAllocateHardwareMemoryAsync(SCPReplyMessage message)
    {
        using var scope = scopeFactory.CreateScope();
        var qHw = scope.ServiceProvider.GetRequiredService<IQHwRepository>();
        var hw = scope.ServiceProvider.GetRequiredService<IHwRepository>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var notify = scope.ServiceProvider.GetRequiredService<INotificationHandler>();

        List<MemoryDto> mems = new List<MemoryDto>();

        var scp = await qHw.GetByComponentIdAsync((short)message.SCPId);

        if (scp is null) return;

        var config = await context.scp_setting.AsNoTracking().FirstOrDefaultAsync();

        if (config is null) return;

        foreach (var i in message.str_sts.sStrSpec)
        {
            switch ((ScpStructure)i.nStrType)
            {
                case ScpStructure.SCPSID_TRAN:
                    // Handle transaction
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_transaction,
                        nSwRecord = await context.transaction.AsNoTracking().CountAsync(),
                        IsSync = config.n_transaction > i.nRecords,
                    });
                    break;

                case ScpStructure.SCPSID_TZ:
                    // Handle time zones
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_tz,
                        nSwRecord = await context.timezone.AsNoTracking().CountAsync(),
                        IsSync = config.n_tz + 1 == i.nRecords,
                    });
                    break;

                case ScpStructure.SCPSID_HOL:
                    // Handle holiday
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_hol,
                        nSwRecord = await context.holiday.AsNoTracking().CountAsync(),
                        IsSync = config.n_hol == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_MSP1:
                    // Handle Msp1 ports (SIO drivers)
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_msp1_port,
                        nSwRecord = 0,
                        IsSync = true,
                    });
                    break;

                case ScpStructure.SCPSID_SIO:
                    // Handle SIOs
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_sio,
                        nSwRecord = await context.module.AsNoTracking().CountAsync(),
                        IsSync = config.n_sio == i.nRecords,
                    });
                    break;

                case ScpStructure.SCPSID_MP:
                    // Handle Monitor points
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_mp,
                        nSwRecord = await context.monitor_point.AsNoTracking().CountAsync(),
                        IsSync = config.n_mp == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_CP:
                    // Handle Control points
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_cp,
                        nSwRecord = await context.control_point.AsNoTracking().CountAsync(),
                        IsSync = config.n_cp == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_ACR:
                    // Handle Access control reader
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_acr,
                        nSwRecord = await context.door.AsNoTracking().CountAsync(),
                        IsSync = config.n_acr == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_ALVL:
                    // Handle Access levels
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_alvl,
                        nSwRecord = await context.accesslevel.AsNoTracking().CountAsync(),
                        IsSync = config.n_alvl == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_TRIG:
                    // Handle trigger
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_trgr,
                        nSwRecord = await context.trigger.AsNoTracking().CountAsync(),
                        IsSync = config.n_trgr == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_PROC:
                    // Handle procedure
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_proc,
                        nSwRecord = await context.procedure.AsNoTracking().CountAsync(),
                        IsSync = config.n_proc == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_MPG:
                    // Handle Monitor point groups
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_mpg,
                        nSwRecord = await context.monitor_group.AsNoTracking().CountAsync(),
                        IsSync = config.n_mpg == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_AREA:
                    // Handle Access area
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_area,
                        nSwRecord = await context.area.AsNoTracking().CountAsync(),
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_EAL:
                    // Handle Elevator access levels
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_CRDB:
                    // Handle Cardholder database
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.n_card,
                        nSwRecord = await context.credential.AsNoTracking().CountAsync(),
                        IsSync = config.n_card == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_FLASH:
                    // Handle FLASH specs
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_BSQN:
                    // Handle Build sequence number
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_SAVE_STAT:
                    // Handle Flash save status
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_MAB1_FREE:
                    // Handle Memory alloc block 1 free memory
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_MAB2_FREE:
                    // Handle Memory alloc block 2 free memory
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_ARQ_BUFFER:
                    // Handle Access request buffers
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_PART_FREE_CNT:
                    // Handle Partition memory free info
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_LOGIN_STANDARD:
                    // Handle Web logins - standard
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;
                case ScpStructure.SCPSID_FILE_SYSTEM:
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                default:
                    // Handle unknown/unsupported types
                    break;
            }
        }

        var res = await hw.UpdateVerifyMemoryAllocateByComponentIdAsync((short)message.SCPId,mems.Any(x => x.IsSync == false));
        if(res <= 0) return;

    
        // Check mismatch device configuration
        //await VerifyDeviceConfigurationAsync(hw.mac,hw.location_id);
        var data = new MemoryAllocateDto
        {
            Mac = await qHw.GetMacFromComponentAsync((short)message.SCPId),
            Memories = mems,
        };
        await notify.ScpNotifyMemoryAllocate(data);
    }


}


