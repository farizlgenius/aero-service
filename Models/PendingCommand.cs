namespace HIDAeroService.Models
{
    public sealed class PendingCommand
    {
        public string Tag { get; init; }
        public DateTime SentAt { get; init; }
        public TaskCompletionSource<bool> Tcs { get; init; }
        public CancellationTokenSource TimeoutCts { get; init; }
    }
}
