using System;

namespace Aero.Domain.Entities;

public record AcrStatus(int ScpId,short number,string Mode,string Status);