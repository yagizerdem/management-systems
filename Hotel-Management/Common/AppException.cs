using System.Net;

namespace Common;

public sealed class AppException : Exception
{
    public ErrorCode ErrorCode { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsOperational { get; set; }
    public ErrorDiagnostic? ErrorDiagnostic { get; set; }
 

    public AppException(string message)
    : base(message)
    {
    }

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
