using HIDAeroService.Aero.CommandService;
using HIDAeroService.Aero.CommandService.Impl;
using HIDAeroService.AeroLibrary;

namespace HIDAeroService.Data
{
    public sealed class Aero 
    {
        public AeroMessage read { get; set; }
        public AeroCommandService write { get; set; }
    }
}