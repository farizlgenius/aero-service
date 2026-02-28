using Aero.Domain.Entities;


namespace Aero.Application.DTOs
{


    public sealed record IntervalDto(int Id,DaysInWeekDto Days,string DaysDetail,string Start,string End,int LocationId,bool Status) : BaseDto(LocationId,Status);
}
