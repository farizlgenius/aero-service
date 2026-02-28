using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class UserDevice 
    {
        [Key]
        public int id { get; set; }
        public int device_id { get; set; }
        public Device device { get; set; }
        public string user_id { get; set; }
        public User user { get; set; }
    }
}
