using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HIDAeroService.Entity
{
    public sealed class Transaction : BaseEntity
    {
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public int SerialNumber { get; set; }
        public string Actor { get; set; } = string.Empty;
        public double Source { get; set; }
        public string SourceDesc { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string SourceModule { get; set; } = string.Empty;
        public double Type { get; set; }
        public string TypeDesc { get; set; } = string.Empty;
        public double TranCode { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string TranCodeDesc { get; set; } = string.Empty;
        public string ExtendDesc { get; set; } = string.Empty;  
        public string Remark { get; set; } = string.Empty;
        public List<TransactionFlag> TransactionFlags { get; set; }
        //public TypeSys TypeSys { get; set; }
        //public TypeWebActivity TypeWebActivity { get; set; }
        //public TypeFileDownloadStatus TypeFileDownloadStatus { get; set; }
        //public TypeCos TypeCos { get; set; }
        //public TypeSioDiag TypeSioDiag { get; set; }
        //public TypeSioComm TypeSioComm { get; set; }
        //public TypeCardBin TypeCardBin { get; set; }
        //public TypeCardBcd TypeCardBcd { get; set; }
        //public TypeCardFull TypeCardFull { get; set; }
        //public TypeCardID TypeCardID { get; set; }
        //public TypeREX TypeREX { get; set; }
        //public TypeUserCmnd TypeUserCmnd { get; set; }
        //public TypeAcr TypeAcr { get; set; }
        //public TypeUseLimit TypeUseLimit { get; set; }
        //public TypeCosElevator TypeCosElevator { get; set;}
        //public TypeCosElevatorAccess TypeCosElevatorAccess { get; set; }
        //public TypeAcrExtFeatureStls TypeAcrExtFeatureStls { get; set; }
        //public TypeAcrExtFeatureCoS TypeAcrExtFeatureCoS { get; set; }
        //public TypeCoSDoor TypeCoSDoor { get; set; }
        //public TypeMPG TypeMPG { get; set; }
        //public TypeArea TypeArea { get; set; }
    }
}
