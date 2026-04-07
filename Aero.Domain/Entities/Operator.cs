using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Operator : BaseDomain
{
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set;  }  = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public short RoleId { get; set; }
        public List<int> LocationIds { get; set; } = new List<int>();

    public Operator(int Id,string username,string pass,string email,string title,string first,string middle,string last,string phone,string image,short role,List<int> locations) : base()
    {
        if(Id < 0) throw new ArgumentException("Id invalid.",nameof(Id));
        this.Id = Id;
        SetUserName(username);
        this.Password = pass;
        SetEmail(email);
        this.Title = title;
        SetFirstname(first);
        this.MiddleName = middle;
        this.LastName = last;
        this.Phone = phone;
        this.Image = image;
        if (role < 0) throw new ArgumentException("Role invalid.",nameof(role));
        this.RoleId = role;
        this.LocationIds = locations;
    }


    private void SetUserName(string user)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(user);
        if (!RegexHelper.IsValidOnlyCharAndDigit(user)) throw new ArgumentException("Username invalid.", nameof(user));
        this.Username = user;
    }

    private void SetEmail(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        if (!RegexHelper.IsValidEmail(email)) throw new ArgumentException("email invalid.", nameof(email));
        this.Email = email;
    }

    private void SetFirstname(string first)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(first);
        if (!RegexHelper.IsValidName(first)) throw new ArgumentException("Firstname invalid.", nameof(first));
        this.FirstName = first;
    }

}
