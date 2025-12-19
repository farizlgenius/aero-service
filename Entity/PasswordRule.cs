namespace HIDAeroService.Entity
{
    public sealed class PasswordRule
    {
        public int Id { get; set; }
        public int Len { get; set; }
        public bool IsLower { get; set; }
        public bool IsUpper { get; set; }
        public bool IsDigit { get; set; }
        public bool IsSymbol { get; set; }
        public ICollection<WeakPassword> Weaks { get; set; }
    }
}
