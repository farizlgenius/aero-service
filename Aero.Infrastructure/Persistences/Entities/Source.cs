using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class Source 
{
      [Key]
      public int id {get; set;}
      public string name {get; set;} = string.Empty; 
      public ICollection<Permission> permissions { get; set; }

}
