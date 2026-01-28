using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class CreateUpdateAccessLevelDto : NoMacBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<AccessLevelComponentDto> Components { get; set; } = new List<AccessLevelComponentDto>();
    }
}
