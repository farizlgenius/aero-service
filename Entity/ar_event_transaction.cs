using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HIDAeroService.Entity
{
    public sealed class event_transction : ar_base_entity
    {
        public string date { get; set; }
        public string time { get; set; }
        public int serial_number { get; set; }
        public string source { get; set; }
        public string source_number { get; set; }
        public string type { get; set; }
        public double transaction_code { get; set; }
        public string description { get; set; }
        [AllowNull]
        public string additional {  get; set; }

    }
}
