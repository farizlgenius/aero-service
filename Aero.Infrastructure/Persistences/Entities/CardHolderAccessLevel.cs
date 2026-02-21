using System;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class CardHolderAccessLevel 
{
      public string holder_id { get; set;} = string.Empty;
      public CardHolder cardholder {get; set;} 
      public short accesslevel_id {get; set;}
      public AccessLevel accessLevel {get; set;}
}
