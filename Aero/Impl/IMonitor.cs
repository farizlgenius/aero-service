namespace HIDAeroService.Aero.Impl
{
    public interface IMonitor<T>
    {
        Task<T> MonitorAsync();
    }
}
