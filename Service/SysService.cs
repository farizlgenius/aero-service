using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Entity;

namespace HIDAeroService.Service
{
    public class SysService
    {
        private readonly AppDbContext _context;
        private readonly AeroLibMiddleware _config;
        private readonly ILogger<SysService> _logger;

        public SysService(AppDbContext context,AeroLibMiddleware config,ILogger<SysService> logger)
        {
            _logger = logger;
            _config = config;
            _context = context;
        }

        #region Command Group

        public string ConfigureDriver()
        {
            var data = _context.ArSystemConfigs.First();
            if(data == null)
            {
                _logger.LogError(Constants.ConstantsHelper.NO_SYSTEM_CONFIG_IN_DB);
                Console.WriteLine(Constants.ConstantsHelper.NO_SYSTEM_CONFIG_IN_DB);
                return Constants.ConstantsHelper.NO_SYSTEM_CONFIG_IN_DB;
            }
            if (!_config.write.SystemLevelSpecification(data.NPorts, data.NScp))
            {
                _logger.LogError(Constants.ConstantsHelper.SYSTEM_LEVEL_SPEC_COMMAND_UNSUCCESS);
                Console.WriteLine(Constants.ConstantsHelper.SYSTEM_LEVEL_SPEC_COMMAND_UNSUCCESS);
                return Constants.ConstantsHelper.SYSTEM_LEVEL_SPEC_COMMAND_UNSUCCESS;
            }
            _logger.LogInformation(Constants.ConstantsHelper.SYSTEM_LEVEL_SPEC_COMMAND_SUCCESS);
            Console.WriteLine(Constants.ConstantsHelper.SYSTEM_LEVEL_SPEC_COMMAND_SUCCESS);

            if (!_config.write.CreateChannel(data.CPort,data.NChannelId,data.CType))
            {
                _logger.LogError(Constants.ConstantsHelper.CREATE_CHANNEL_COMMAND_UNSUCCESS);
                Console.WriteLine(Constants.ConstantsHelper.CREATE_CHANNEL_COMMAND_UNSUCCESS);
                return Constants.ConstantsHelper.CREATE_CHANNEL_COMMAND_UNSUCCESS;
            }
            _logger.LogInformation(Constants.ConstantsHelper.CREATE_CHANNEL_COMMAND_SUCCESS);
            Console.WriteLine(Constants.ConstantsHelper.CREATE_CHANNEL_COMMAND_SUCCESS);

            return Constants.ConstantsHelper.INITIAL_DRIVER_SUCCESS;

        }

        #endregion

        public ArScpSetting GetScpSetting()
        {
            try
            {
                return _context.ArScpSettings.First();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new ArScpSetting();
            }
        }



    }
}
