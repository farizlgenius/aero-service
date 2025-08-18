using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_operator : ar_base_entity
    {
        public string user_id  { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string title { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string role { get; set; }
        public string phone { get; set; }
        public string photo { get; set; }

    }
}
