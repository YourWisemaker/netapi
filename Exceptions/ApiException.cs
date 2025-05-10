using System.Net;

namespace netapi.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {
        }
    }

    public class BadRequestException : ApiException
    {
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }

    public class ExternalApiException : ApiException
    {
        public ExternalApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
            : base($"External API error: {message}", statusCode)
        {
        }
    }
}
