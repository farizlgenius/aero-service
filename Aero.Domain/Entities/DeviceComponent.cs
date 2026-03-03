using System;

namespace Aero.Domain.Entities;

public sealed class DeviceComponent
{

      public int Id {get; private set;}
      public short ModelNo {get; private set;}
      public string Name {get; private set;} = string.Empty;
      public int nInput {get; private set;}
      public int nOutput  {get; private set;}
      public int nReader  {get; private set;}

      public DeviceComponent(int id,short model,string name,int input,int output,int reader)
      {
            this.Id = id;
            this.ModelNo = model;
            this.Name = name;
            this.nInput = input;
            this.nOutput = output;
            this.nReader = reader;
      }

}
