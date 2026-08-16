using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public sealed class ErrorDiagnostic
    {
        public string? ComponentName { get; init; }
        public string? MethodName { get; init; }
        public string? TraceId { get; init; }
    }
}
