

using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record OperatorDto(
        int Id, 
        string Username, 
        string Email, 
        string Title, 
        string Firstname, 
        string Middlename, 
        string Lastname, 
        string Phone, 
        string Image, 
        int Role,
        List<int> LocationIds, 
        List<int> Features,
        bool IsActive);
}
