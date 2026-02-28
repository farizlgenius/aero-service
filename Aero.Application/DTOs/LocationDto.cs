using System.ComponentModel.DataAnnotations;

namespace Aero.Application.DTOs
{

    public sealed record LocationDto(int Id, string Name, string Description, bool IsActive);
}
