using System;

namespace Aero.Domain.Entities;

public sealed record SioStatus(int ScpId, short SioId, string Status, string Tamper, string Ac, string Batt);