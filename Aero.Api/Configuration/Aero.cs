using AeroService.Aero.CommandService;
using AeroService.AeroLibrary;

namespace Aero.Api.Configuration
{
    public sealed class Aero 
    {
        public AeroMessage read { get; set; }
        public AeroCommandService write { get; set; }
    }
}