using System.Net;

namespace Common;

public sealed class AppException : Exception
{
    public ErrorCode ErrorCode { get; }
    public HttpStatusCode StatusCode { get; }
    public bool IsOperational { get; }
    public ErrorDiagnostic? ErrorDiagnostic { get; set; }

    public AppException(
        string message,
        ErrorCode errorCode,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        bool isOperational = true,
        Exception? innerException = null,
        ErrorDiagnostic? errorDiagnostic = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
        IsOperational = isOperational;
        this.ErrorDiagnostic = errorDiagnostic; 
    }
}
