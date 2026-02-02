using System;

namespace Aero.Domain.Interfaces;

public interface IStrSpec
{
      short nStrType {get;}
      int nRecords {get;}
      int nRecSize {get;}
      int nActive{get;}
}
