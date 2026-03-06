using Aero.Domain.Enums;
using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class User : BaseDomain
{
       public string UserId { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string MiddleName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public Gender Sex { get; private set; } 
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public int CompanyId {get; private set;}
        public string Company { get; private set; } = string.Empty;
        public int PositionId {get; private set;}
        public string Position { get; private set; } = string.Empty;
        public int DepartmentId {get; private set;}
        public string Department { get; private set; } = string.Empty;
        public string Image { get; private set; } = string.Empty;
        public short Flag { get; private set; }
        public List<string> Additionals { get; private set; } = new List<string>();
        public List<Credential> Credentials { get; private set; } = new List<Credential>();
        public List<AccessLevel> AccessLevels { get; private set; } = new List<AccessLevel>();

        public User(string userid,string title,string firstname,string middlename,string lastname,Gender sex,string email,string phone,int company_id,string company,int position_id,string position,int department_id,string department,string image,short flag,List<string> additional,List<Credential> creds,List<AccessLevel> alvl,int locationId,bool status) : base(locationId,status)
        {
            SetUserId(userid);
            this.Title = title;
            SetFirstname(firstname);
            SetLastname(lastname);
            this.Sex = sex;
            SetEmail(email);
            this.Phone = phone;
            this.CompanyId = company_id;
            this.Company = company;
            this.PositionId = position_id;
            this.DepartmentId = department_id;
            this.Department = department;
            this.Image = image;
            this.Flag = flag;
            this.Additionals = additional;
            this.Credentials = creds;
            this.AccessLevels = alvl;

        }

    private void SetUserId(string userid)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userid);
        if (!RegexHelper.IsValidOnlyCharAndDigit(userid)) throw new ArgumentException("User id invalid.");

        this.UserId = userid;
    }

    private void SetFirstname(string first)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(first);
        if(!RegexHelper.IsValidName(first)) throw new ArgumentException("Firstname invalid.");
        this.FirstName = first;
    }

    private void SetLastname(string last)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(last);
        if(!RegexHelper.IsValidName(last)) throw new ArgumentException("Lastname invalid.");
        this.LastName = last;
    }

    private void SetEmail(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        if(!RegexHelper.IsValidEmail(email)) throw new ArgumentException("Email invalid.");
        this.LastName = email;
    } 

}
