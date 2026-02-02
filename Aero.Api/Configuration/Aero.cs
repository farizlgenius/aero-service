using Aero.Infrastructure.Services;
using AeroService.Aero.CommandService;

namespace Aero.Api.Configuration
{
    public sealed class Aero 
    {
        public AeroMessageListener read { get; set; } 
        public AeroCommandService write { get; set; }
    }
}