using HIDAeroService.Aero.Impl;
using System.Threading;

namespace HIDAeroService.Aero.Intf
{
    public sealed class Monitor<T> : IMonitor<T>
    {
        private int _timeout;

        private static TaskCompletionSource<T>? _pendingTask;

        public Monitor(int timeout)
        {
            _timeout = timeout;
        }

        public async Task<T> MonitorAsync()
        {
            
            
            if (_pendingTask != null)
            {
                throw new Exception("Another request is already waiting.");
            }

            var cts = new CancellationTokenSource(_timeout);

            _pendingTask = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            var completedTask = await Task.WhenAny(_pendingTask.Task,Task.Delay(Timeout.Infinite,cts.Token));

            _pendingTask = null; // reset for next request

            if (completedTask == _pendingTask.Task)
            {
                return await _pendingTask.Task;
            }
            else
            {
                throw new TimeoutException("Timeout waiting for result.");
            }
        }

        // This is a "normal function" that can be called by any service or background worker
        public static void TriggerResult(T Result)
        {
            if (_pendingTask != null)
            {
                _pendingTask.TrySetResult(Result);
                _pendingTask = null;
            }
        }
    }
}
