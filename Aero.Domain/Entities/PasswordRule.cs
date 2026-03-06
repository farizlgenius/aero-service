using System;

namespace Aero.Domain.Entities;

public sealed class PasswordRule
{
  public int Id {get; set;}
      public int Len { get; set; }
        public bool IsLower { get; set; }
        public bool IsUpper { get; set; }
        public bool IsDigit { get; set; }
        public bool IsSymbol { get; set; }
        public List<string> Weaks { get; set; } = new List<string>();

        public PasswordRule(int len,bool islower,bool isupper,bool isdigit,bool issymbol,List<string> weaks)
        {
            SetLen(len);
            this.IsLower = islower;
            this.IsUpper = isupper;
            this.IsDigit = isdigit;
            this.IsSymbol = issymbol;
            if(weaks.Count <= 0 )this.Weaks = weaks;
        }

        private void SetLen(int len)
  {
    if(len < 4) throw new ArgumentException("Password lenght must geater than 3 digit");
    this.Len = len;
  }



}
