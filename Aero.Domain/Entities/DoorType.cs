using System;

namespace Aero.Domain.Entities;

public sealed class DoorType
{
      public int Id {get; private set;}
      public string Name {get; private set;} = string.Empty;
      public string Value {get; private set;} = string.Empty;
      public string Description {get; private set;} = string.Empty;

      public DoorType()
      {
            
      }

}
