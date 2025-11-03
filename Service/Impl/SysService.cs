using HIDAeroService.AeroLibrary;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;

namespace HIDAeroService.Service.Impl
{
    public class SysService(AppDbContext context, AeroCommand command)
    {

        public bool ConfigureDriver()
        {
            var data = context.SystemConfigurations.First();
            if (data == null) return false;

            if (!command.SystemLevelSpecification(data.nPorts, data.nScp)) return false;

            if (!command.CreateChannel(data.cPort, data.nChannelId, data.cType)) return false;


            return true;

        }


    }
}
