using System;

namespace Aero.Domain.Entities;

public sealed record SioStatus(int DeviceId, short DriverId, string Status, string Tamper, string Ac, string Batt);