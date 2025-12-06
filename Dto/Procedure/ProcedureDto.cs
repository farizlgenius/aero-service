using HIDAeroService.DTO.Action;

namespace HIDAeroService.DTO.Procedure
{
    public sealed class ProcedureDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public List<ActionDto> Actions { get; set; }
    }
}
