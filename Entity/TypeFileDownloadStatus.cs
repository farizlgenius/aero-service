namespace HIDAeroService.Entity
{
    public sealed class TypeFileDownloadStatus : BaseTransactionType
    {
        public string fileType { get; set; } = string.Empty;

        public string fileName { get; set; } = string.Empty;
    }
}
