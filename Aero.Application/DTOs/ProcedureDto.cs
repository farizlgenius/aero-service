
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class ProcedureDto : BaseDomain
    {
        public short ProcId {get; set;}
        public string Name { get; set; } = string.Empty;
        public List<ActionDto> Actions { get; set; } = new List<ActionDto>();
    }
}
