using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IUserCommand
{
      bool AccessDatabaseCardRecord(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<short> AccessLevel, int Active, int Deactive = 2085970000);
      bool CardDelete(short ScpId, long CardNo);
}
