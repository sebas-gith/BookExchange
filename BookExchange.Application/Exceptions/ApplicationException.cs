namespace BookExchange.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ValidationException : ApplicationException
    {
        public List<string> Errors { get; }

        public ValidationException(string message, List<string> errors = null) : base(message)
        {
            Errors = errors ?? new List<string>();
        }
    }
}
