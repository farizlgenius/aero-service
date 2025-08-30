using HIDAeroService.AeroLibrary;

namespace HIDAeroService.Data
{
    public sealed class AeroLibMiddleware 
    {
        public ReadAeroDriver read { get; set; }
        public WriteAeroDriver write { get; set; }
    }
}