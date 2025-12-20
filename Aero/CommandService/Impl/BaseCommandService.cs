using HID.Aero.ScpdNet.Wrapper;
using System.Collections.Concurrent;

namespace HIDAeroService.Aero.CommandService.Impl
{
    public class BaseCommandService
    {
        protected readonly ConcurrentDictionary<int, TaskCompletionSource<bool>> _pendingCommands = new ConcurrentDictionary<int, TaskCompletionSource<bool>>();
        public int TagNo { get; private set; } = 0;
        public List<int> UploadCommandTags { get; private set; } = new List<int>();
        public string Command { get; private set; } = "";
        public int CommandTimeOut
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        protected int _commandTimeout = 10;

        protected Task<bool> SendCommandAsync(int tag, int timeout)
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

        protected void CommandResultTrigger(int tag, bool result)
        {
            if (_pendingCommands.TryRemove(tag, out var tcs))
            {
                tcs.TrySetResult(result);
            }
        }

        //////
        // Method: Turn on debug to file
        //////
        protected void TurnOnDebug()
        {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
            if (flag)
            {
                Console.WriteLine("Debug to file on");
            }
            else
            {
                Console.WriteLine("Debug to file off");
            }

        }

        //////
        // Method: Turn off debug to file
        //////
        protected void TurnOffDebug()
        {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugOff);
            if (flag)
            {
                Console.WriteLine("Debug to file off");
            }
            else
            {
                Console.WriteLine("Debug to file on");
            }
        }

        //////
        // Method: SendCommand to Driver
        // PreCondition: SCP online
        // PostCondition: Pass/Fail response is sent from the driver
        //////
        protected bool SendASCIICommand(string command)
        {
            // send the command
            return SCPDLL.scpConfigCommand(command);
        }

        //////
        // Method: SendCommand to Driver
        // PreCondition: SCP online
        // PostCondition: Pass/Fail response is sent from the driver
        //////
        protected bool SendCommand(short command, IConfigCommand cfg)
        {
            SCPConfig scp = new SCPConfig();
            bool success = scp.scpCfgCmndEx(command, cfg);
            return success;
        }

    }
}
