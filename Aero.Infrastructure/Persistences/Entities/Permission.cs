using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class Permission
{
      [Key]
      public int id { get; set; }
      public int source_id { get; set; }
      public Source source { get; set; }  
      public int role_id { get; set; }
      public Role role { get; set; }
       public bool is_allow { get; set; }
       public bool is_create { get; set; }
      public bool is_modify { get; set; }
      public bool is_delete { get; set; }
      public bool is_action { get; set; }

      public Permission()
      {
      }

      public Permission(int source,int role,bool is_allow,bool is_create,bool is_modify,bool is_delete,bool is_action)
      {
            this.source_id = source;
            this.role_id = role;
            this.is_allow = is_allow;
            this.is_create = is_create;
            this.is_modify = is_modify;
            this.is_delete = is_delete;
            this.is_action = is_action;
      }

      public void Update(Permission data)
      {
            this.source_id = data.source_id;
            this.role_id = data.role_id;
            this.is_allow = data.is_allow;
            this.is_create = data.is_create;
            this.is_modify = data.is_modify;
            this.is_delete = data.is_delete;
            this.is_action = data.is_action;
      }


}
