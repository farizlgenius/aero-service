using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Model
{
    public sealed class MonitorGroupListDto
    {
        public short PointType { get; set; }
        public string PointTypeDesc { get; set; } = string.Empty;
        public short PointNumber { get; set; }

    }
}
