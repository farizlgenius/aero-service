using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constants;
using AeroService.Data;
using AeroService.Entity;
using AeroService.Helpers;

namespace AeroService.Service.Impl
{
    public class SysService(AppDbContext context, AeroCommandService command)
    {

        public bool ConfigureDriver()
        {
            var data = context.system_configuration.First();
            if (data == null) return false;

            if (!command.SystemLevelSpecification(data.n_ports, data.n_scp)) return false;

            if (!command.CreateChannel(data.c_port, data.n_channel_id, data.c_type)) return false;


            return true;

        }


    }
}
