namespace Aero.Api.Exceptions.Custom
{
    public class ExceptionWithErrors : Exception
    {
        public List<string> Errors { get; set; }
        public ExceptionWithErrors(string message,List<string> errors) : base(message) 
        {
            Errors = errors;
        }
    }
}
