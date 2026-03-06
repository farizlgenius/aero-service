using System;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class UserAccessLevel 
{
      public string user_id { get; private set;} = string.Empty;
      public User user {get; private set;} 
      public int accesslevel_id {get; private set;}
      public AccessLevel accessLevel {get; private set;}
    public UserAccessLevel(){}

    public UserAccessLevel(string user_id,int accesslevel_id) 
    {
        this.user_id = user_id;
        this.accesslevel_id = accesslevel_id;
    }
}

