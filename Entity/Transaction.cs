using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AeroService.Entity
{
    public sealed class Transaction : BaseEntity
    {
        public string date { get; set; } = string.Empty;
        public string time { get; set; } = string.Empty;
        public int serial_number { get; set; }
        public string actor { get; set; } = string.Empty;
        public double source { get; set; }
        public string source_desc { get; set; } = string.Empty;
        public string origin { get; set; } = string.Empty;
        public string source_module { get; set; } = string.Empty;
        public double type { get; set; }
        public string type_desc { get; set; } = string.Empty;
        public double tran_code { get; set; }
        public string image_path { get; set; } = string.Empty;
        public string tran_code_desc { get; set; } = string.Empty;
        public string extend_desc { get; set; } = string.Empty;  
        public string remark { get; set; } = string.Empty;
        public string hardware_mac {  get; set; } = string.Empty;
        public string hardware_name { get; set; } = string.Empty;   
        public List<TransactionFlag> transaction_flag { get; set; }
        
    }
}
