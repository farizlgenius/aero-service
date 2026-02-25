
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Enums;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class User : BaseEntity
    {
        [Required]
        public string user_id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string middle_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public Gender gender { get; set; } 
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public int company_id { get; set; }
        public Company company { get; set; }
        public int department_id { get; set; } 
        public Department department { get; set; }
        public int position_id { get; set; }
        public Position position { get; set; }
        public short flag { get; set; }
        public ICollection<UserAdditional> additionals { get; set; }
        public string image { get; set; } = string.Empty;
        public ICollection<Credential> credentials { get; set; }
        public ICollection<UserAccessLevel> user_access_levels { get; set; }

        public User(string userid,string title,string firstname,string middlename,string lastname,Gender gender,string email,string phone,int company_id,int department_id,int position_id,short flag,List<string> additionals,string image,List<Credential> cred,List<short> user_accesslevel,int location) : base(location) 
        {
            this.user_id = userid;
            this.title = title;
            this.first_name = firstname;
            this.middle_name = middle_name;
            this.last_name = lastname;
            this.gender = gender;
            this.email = email;
            this.phone = phone;
            this.company_id = company_id;
            this.department_id = department_id;
            this.position_id = position_id;
            this.flag = flag;
            this.additionals = additionals.Select(x => new UserAdditional(userid,x)).ToList();
            this.image = image;
            this.user_access_levels = user_accesslevel.Select(a => new UserAccessLevel(userid,a)).ToList();
            this.credentials = cred;

        }
    }
}
