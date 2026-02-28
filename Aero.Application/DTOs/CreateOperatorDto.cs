using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record CreateOperatorDto(int Id, string UserId, string Username,string Password, string Email, string title, string Firstname, string Middlename, string Lastname, string Phone, string Image, short Role, List<int> LocationIds, bool IsActive);
}
