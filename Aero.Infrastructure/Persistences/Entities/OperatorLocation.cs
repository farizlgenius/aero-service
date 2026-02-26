using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class OperatorLocation
    {
        [Key]
        public int id { get; set; } 
        public int location_id { get; set; }
        public Location location { get; set; }
        public int operator_id { get; set; }
        public Operator Operators { get; set; }

        public OperatorLocation(){}

        public OperatorLocation(int locationid,int operatorid)
        {
            this.location_id = locationid;
            this.operator_id = operatorid;
        }

        public void Update(int locationid,int operatorid)
        {
            this.location_id = locationid;
            this.operator_id = operatorid;
        }
    }
}
