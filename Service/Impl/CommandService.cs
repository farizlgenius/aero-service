
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.DTO.Scp;
using MiNET.Entities.Passive;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using System.Collections.Concurrent;
using static HIDAeroService.Constants.Command;

namespace HIDAeroService.Service.Impl
{
    public class CommandService : ICommand
    {
        private readonly ConcurrentDictionary<int, TaskCompletionSource<bool>> _pendingCommands = new ConcurrentDictionary<int, TaskCompletionSource<bool>>();
      
        public async Task<bool> ExecuteAsync<T>(CommandFlags CommandFlag,T Data)
        {
            switch (Data)
            {
                case CommandFlags.enCcMpSrq: // Request Sensor Status from Controller
                    if(Data is HardwareDto)
                    {
                        //CC_MPSRQ cfg = new CC_MPSRQ();
                        //cfg.scp_number = ScpId;
                        //cfg.first = MpNo;
                        //cfg.count = Count;
                        //bool flag = Send((short)enCfgCmnd.enCcMpSrq, cfg);
                        //if (flag) return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), TimeOut);        
                    }
                    return false;
                default:
                    break;
            }
            return true;
        }

        private bool Send(short command, IConfigCommand cfg)
        {
            SCPConfig scp = new SCPConfig();
            bool success = scp.scpCfgCmndEx(command, cfg);
            return success;
        }

        private Task<bool> SendCommandAsync(int tag, int timeout)
        {

            TimeSpan span = TimeSpan.FromSeconds(timeout);
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!_pendingCommands.TryAdd(tag, tcs))
                throw new InvalidOperationException($"Command with tag {tag} already exists");

            // Timeout
            var cts = new CancellationTokenSource(span);
            cts.Token.Register(() => {
                if (_pendingCommands.TryRemove(tag, out var source))
                    source.TrySetException(new TimeoutException("Command time out"));
            });

            return tcs.Task;

        }
    }
}
