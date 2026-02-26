namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Transaction : BaseEntity
    {
        public DateTime date_time { get; set; } = DateTime.UtcNow;
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
        public string mac { get; set; } = string.Empty;
        public string hardware_name { get; set; } = string.Empty;
        public List<TransactionFlag> transaction_flag { get; set; } = new List<TransactionFlag>();

        public Transaction(Aero.Domain.Entities.Transaction data) : base(data.LocationId)
        {
            this.date_time = data.DateTime;
            this.serial_number = data.SerialNumber;
            this.actor = data.Actor;
            this.source = data.Source;
            this.source_desc = data.SourceDesc;
            this.origin = data.Origin;
            this.source_module = data.SourceModule;
            this.type = data.Type;
            this.type_desc = data.TypeDesc;
            this.tran_code = data.TranCode;
            this.image_path = data.Image;
            this.tran_code_desc = data.TranCodeDesc;
            this.extend_desc = data.ExtendDesc;
            this.remark = data.Remark;
            this.mac = data.Mac;
            this.hardware_name = data.HardwareName;

        }

    }
}
