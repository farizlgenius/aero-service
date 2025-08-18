using HIDAeroService.Data;

namespace HIDAeroService.Service
{
    public class SysService
    {
        private readonly AppDbContext _context;
        private readonly AppConfigData _config;

        public SysService(AppDbContext context,AppConfigData config)
        {
            _config = config;
            _context = context;
        }

        #region Command Group

        public bool InitialDriver(short cPort)
        {

            if (!_config.write.SystemLevelSpecification(1, 100))
            {
                Console.WriteLine("SystemLevelSpecification : False");
                return false;
            }
            Console.WriteLine("SystemLevelSpecification : True");

            if (!_config.write.CreateChannel(cPort))
            {
                Console.WriteLine("CreateChannel : False");
                return false;
            }
            Console.WriteLine("CreateChannel : True");
            return true;

        }

        #endregion



    }
}
