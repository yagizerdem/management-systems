using System.Net;

namespace Common;

public sealed class AppException : Exception
{
    public ErrorCode ErrorCode { get; }
    public HttpStatusCode StatusCode { get; }
    public bool IsOperational { get; }

    public AppException(
        string message,
        ErrorCode errorCode,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        bool isOperational = true,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
        IsOperational = isOperational;
    }
}
