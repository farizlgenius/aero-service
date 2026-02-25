using Aero.Domain.Enums;
using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class User : BaseDomain
{
       public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Gender Sex { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public short Flag { get; set; }
        public List<string> Additionals { get; set; } = new List<string>();
        public List<Credential> Credentials { get; set; } = new List<Credential>();
        public List<AccessLevel> AccessLevels { get; set; } = new List<AccessLevel>();

        public User(string userid,string title,string firstname,string middlename,string lastname,string sex,string email,string phone,string company,string position,string department,string image,short flag,List<string> additional,List<Credential> creds,List<AccessLevel> alvl,int locationId,bool status) : base(locationId,status)
        {
            SetUserId(userid);
        }

    private void SetUserId(string userid)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userid);
        if (!RegexHelper.IsValidOnlyCharAndDigit(userid)) throw new ArgumentException("User id invalid.");

        this.UserId = userid;
    }

}
