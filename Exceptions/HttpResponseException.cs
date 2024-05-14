namespace MiddleApi.Exceptions;

public class HttpResponseException : Exception
{
    public HttpResponseException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}