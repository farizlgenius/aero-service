

using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{


    public sealed record CredentialDto(int Bits,int IssueCode,int FacilityCode,long CardNo,string Pin,string ActiveDate,string DeactiveDate);
}
