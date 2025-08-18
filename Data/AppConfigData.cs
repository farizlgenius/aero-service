using HIDAeroService.AeroLibrary;

namespace HIDAeroService.Data
{
    public sealed class AppConfigData 
    {
        public ReadAeroDriver read { get; set; }
        public WriteAeroDriver write { get; set; }
    }
}