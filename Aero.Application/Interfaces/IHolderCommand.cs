using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IHolderCommand
{
      bool AccessDatabaseCardRecord(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<AccessLevelDto> AccessLevel, int Active, int Deactive = 2085970000);
      bool CardDelete(short ScpId, long CardNo);
}
