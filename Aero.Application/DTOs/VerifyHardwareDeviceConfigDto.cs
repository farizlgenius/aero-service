namespace Aero.Application.DTOs;

public sealed record VerifyHardwareDeviceConfigDto(string ComponentName, int nMismatchRecord, bool IsUpload);
