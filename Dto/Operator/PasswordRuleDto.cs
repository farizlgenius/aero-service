namespace HIDAeroService.DTO.Operator
{
    public sealed class PasswordRuleDto
    {
        public int Len { get; set; }
        public bool IsLower { get; set; }
        public bool IsUpper { get; set; }
        public bool IsDigit { get; set; }
        public bool IsSymbol { get; set; }
        public List<string> Weaks { get; set; } = new List<string>();

    }
}
