using static HIDAeroService.Constants.Command;

namespace HIDAeroService.Service
{
    public interface ICommand
    {
        Task<bool> ExecuteAsync<T>(CommandFlags CommandFlag, T Data);
    }
}
