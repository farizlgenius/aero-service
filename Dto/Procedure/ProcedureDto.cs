using AeroService.DTO.Action;

namespace AeroService.DTO.Procedure
{
    public sealed class ProcedureDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public List<ActionDto> Actions { get; set; }
    }
}
