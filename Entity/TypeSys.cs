using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TypeSys : BaseTransactionType
    {

        public int errorCode { get; set; }
        public string currentPrimaryComm { get; set; } = string.Empty;
        public string previousPrimaryComm { get; set; } = string.Empty;

    }
}
