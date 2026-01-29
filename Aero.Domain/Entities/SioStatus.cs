using System;

namespace Aero.Domain.Entities;

public sealed record SioStatus(string Mac, short SioNo, string Status, string Tamper, string Ac, string Batt);