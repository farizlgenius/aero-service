using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.AeroLibrary;

namespace AeroService.Data
{
    public sealed class Aero 
    {
        public AeroMessage read { get; set; }
        public AeroCommandService write { get; set; }
    }
}