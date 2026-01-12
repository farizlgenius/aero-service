using System;
using System.ComponentModel.DataAnnotations;
using AeroService.Entity.Interface;

namespace AeroService.Entity;

public sealed class KeyPair
{
        [Key]
    public int id { get; set; }
    public string key_uuid { get; set; } = new Guid().ToString();
    public byte[] public_key { get; set; }
    public byte[] shared_secret { get; set; }
    public DateTime created_date {get; set;}
    public DateTime updated_date {get; set;}


}
