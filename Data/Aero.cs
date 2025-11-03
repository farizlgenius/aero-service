using HIDAeroService.AeroLibrary;

namespace HIDAeroService.Data
{
    public sealed class Aero 
    {
        public AeroMessage read { get; set; }
        public AeroCommand write { get; set; }
    }
}