using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class AuditTrail 
{
       [Key]
        public int id { get; set; }
        public DateTime timestamp { get; set;} = DateTime.UtcNow;
      //   public int location_id { get; set; } = 1;
      //   public Location location {get; set;}
        public string username {get; set;} = string.Empty;
      // public Aero.Domain.Enums.Action action { get; set;}
      // public Aero.Domain.Enums.Module module {get; set;} 
      // public string object_name {get; set;} = string.Empty;
      // public string old_value {get; set;} = string.Empty;
      // public string new_value {get; set;} = string.Empty;

      public string controller {get; set;} = string.Empty;
      public string action {get; set;} = string.Empty;
      public string http_method { get; set;} = string.Empty;
      public string path {get; set;} = string.Empty;
      public string request_body { get; set;} = string.Empty;
      public int status_code {get; set;}
      public long execution_time {get; set;}
      public string ip {get; set;} = string.Empty;



}
