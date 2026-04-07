using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class DoorType 
{
      [Key]
      public int id {get; set;}
      public string name {get; set;} = string.Empty;
      public int value {get; set;}
      public string description {get; set;} = string.Empty;

      public DoorType(int id,string name,int value,string description)
      {
            this.id = id;
            this.name = name;
            this.value = value;
            this.description = description;
      }

}
