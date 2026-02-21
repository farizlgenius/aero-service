using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities;

public sealed class AccessLevelMacs 
{
      [Key]
      public int id {get; set;}
      public string mac {get; set;} = string.Empty;
      public short acccesslevel_id {get; set;}
      public AccessLevel accesslevel {get; set;}
}
