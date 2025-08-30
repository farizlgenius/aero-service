using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HIDAeroService.Entity
{
    public sealed class ArEvent : ArBaseEntity
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public int Serialnumber { get; set; }
        public string Source { get; set; }
        public string SourceNo { get; set; }
        public string Type { get; set; }
        public double TransactionCode { get; set; }
        public string Description { get; set; }
        [AllowNull]
        public string Additional {  get; set; }

    }
}
