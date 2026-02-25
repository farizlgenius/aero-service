using System;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class UserAccessLevel 
{
      public string user_id { get; private set;} = string.Empty;
      public User user {get; private set;} 
      public short accesslevel_id {get; private set;}
      public AccessLevel accessLevel {get; private set;}
    public UserAccessLevel(string user_id,short accesslevel_id) 
    {
        this.user_id = user_id;
        this.accesslevel_id = accesslevel_id;
    }
}
