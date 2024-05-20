namespace Oauth2.sdk.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException()
        {
        }

        public ForbiddenAccessException(string message)
            : base(message)
        {
        }

        public ForbiddenAccessException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
