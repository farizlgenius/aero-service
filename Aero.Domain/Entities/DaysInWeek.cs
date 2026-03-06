using System;

namespace Aero.Domain.Entities;

public sealed class DaysInWeek
{
   public int IntervalId { get; set; }
  public bool Sunday { get; set; }
  public bool Monday { get; set; }
  public bool Tuesday { get; set; }
  public bool Wednesday { get; set; }
  public bool Thursday { get; set; }
  public bool Friday { get; set; }
  public bool Saturday { get; set; }

    public DaysInWeek()
    {
        IntervalId = 0;
        Saturday = false;
        Monday = false;
        Tuesday = false;
        Wednesday = false;
        Thursday = false;
        Friday = false;
        Saturday = false;
    }

    public DaysInWeek(bool sun,bool mon,bool tue,bool wed,bool thu,bool fri,bool sat)
    {
        IntervalId = 0;
        Saturday = sun;
        Monday = mon;
        Tuesday = tue;
        Wednesday = wed;
        Thursday = thu;
        Friday = fri;
        Saturday = sat;
    }
}