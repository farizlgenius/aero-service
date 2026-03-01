using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record ModuleDto(short DriverId, short Model, string ModelDescription, string Revision, string SerialNumber, int nHardwareId, string nHardwareIdDescription, int nHardwareRev, int nProductId, int nProductVer, short nEncConfig, string nEncConfigDescription, short nEncKeyStatus, string nEncKeyStatusDescription, List<ReaderDto>? Readers, List<SensorDto>? Sensors, List<StrikeDto>? Strikes, List<RequestExitDto>? RequestExits, List<MonitorPointDto>? MonitorPoints, List<ControlPointDto>? ControlPoints, short Address, string AddressDescription, short Port, short nInput, short nOutput, short nReader, short Msp1No, short BaudRate, short nProtocol, short nDialect,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
