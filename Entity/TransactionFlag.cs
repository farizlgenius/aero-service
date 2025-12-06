using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TransactionFlag
    {
        [Key]
        public int Id { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public Transaction Transaction { get; set; }
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
        //public TypeCosElevator TypeCosElevator
        //{
        //    get; set;
        //}

        //public TypeCosElevatorAccess TypeCosElevatorAccess { get; set; }
        //public TypeAcrExtFeatureStls TypeAcrExtFeatureStls { get; set; }
        //public TypeAcrExtFeatureCoS TypeAcrExtFeatureCoS { get; set; }
        //public TypeCoSDoor TypeCoSDoor { get; set; }
        //public TypeMPG TypeMPG { get; set; }
        //public TypeArea TypeArea { get; set; }
    }
}
