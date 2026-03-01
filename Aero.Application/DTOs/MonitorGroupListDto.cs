using System.ComponentModel.DataAnnotations;

namespace Aero.Application.DTOs;

public sealed record MonitorGroupListDto(short PointType, string PointTypeDesc, short PointNumber);
