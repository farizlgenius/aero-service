using System;

namespace Aero.Application.Entities;

public sealed record SioStatus(string Mac, short SioNo, string Status, string Tamper, string Ac, string Batt);