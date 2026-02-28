namespace Aero.Application.DTOs
{


    public sealed record DaysInWeekDto(
        bool Sunday,
        bool Monday,
        bool Tuesday,
        bool Wednesday,
        bool Thursday,
        bool Friday,
        bool Saturday
        );
}
