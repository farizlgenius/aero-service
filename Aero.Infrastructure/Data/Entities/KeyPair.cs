using System;
using System.ComponentModel.DataAnnotations;


namespace Aero.Infrastructure.Data.Entities;

public sealed class KeyPair 
{
  [Key]
  public int id { get; set; }
  public Guid key_uuid { get; set; } = Guid.NewGuid();
  public required byte[] public_key { get; set; }
  public required byte[] secret_key { get; set; }
  public DateTime created_date { get; set; }
  public DateTime expire_date { get; set; }
  public bool is_revoked { get; set; }
  public ICollection<SecretKey>? secrets { get; set; }
}
