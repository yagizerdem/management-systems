namespace Common
{

    public sealed class DiagnosticDetails
    {
        public ErrorCode ErrorCode { get; set; }

        public string Message { get; set; }

        public DiagnosticDetails() { }

        public DiagnosticDetails(ErrorCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public sealed class ErrorDiagnostic
    {
        public string? ComponentName { get; init; }
        public string? MethodName { get; init; }
        public string? TraceId { get; init; }

        public IReadOnlyCollection<DiagnosticDetails> Details { get; init; }
            = Array.Empty<DiagnosticDetails>();

        public ErrorDiagnostic()
        {
        }

        public ErrorDiagnostic(
            string? componentName,
            string? methodName,
            string? traceId = null,
            IEnumerable<DiagnosticDetails>? details = null)
        {
            ComponentName = componentName;
            MethodName = methodName;
            TraceId = traceId;
            Details = details?.ToArray() ?? Array.Empty<DiagnosticDetails>();
        }
    }
}
