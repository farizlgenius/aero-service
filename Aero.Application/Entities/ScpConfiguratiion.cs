using System;

namespace Aero.Application.Entities;

public sealed record ScpConfiguratiion(string Mac,short LocationId,List<Configurations> Configurations);
