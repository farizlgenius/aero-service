

using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record OperatorDto(int Id, string UserId, string Username, string Email, string title, string Firstname, string Middlename, string Lastname, string Phone, string Image, short Role, List<int> LocationIds, bool IsActive);
}
