using System.ComponentModel.DataAnnotations;

namespace Aero.Application.Models
{
    public sealed class MonitorGroupListDto
    {
        public short PointType { get; set; }
        public string PointTypeDesc { get; set; } = string.Empty;
        public short PointNumber { get; set; }

    }
}
