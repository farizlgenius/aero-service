using System;

namespace Aero.Domain.Entities;

public sealed class OperatorLocation 
{
      public int LocationId {get; set;}
      public int OperatorId {get; set;}
      public OperatorLocation(int locationid,int operatorid)
      {
            if(locationid < 0) throw new ArgumentException("Location id.",nameof(locationid));
            this.LocationId = locationid;
            if(operatorid < 0) throw new ArgumentException("Operator id.",nameof(operatorid));
            this.OperatorId = operatorid;
      }


}
