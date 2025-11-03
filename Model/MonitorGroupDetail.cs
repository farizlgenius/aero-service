using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Model
{
    public sealed class MonitorGroupDetail
    {
        [Key]
        public int Id { get; set; }
        public short PointType { get; set; }
        public short PointNo { get; set; }
    }
}
