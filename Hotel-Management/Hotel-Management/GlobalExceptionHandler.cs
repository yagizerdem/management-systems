using Azure;
using Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel_Management
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            ApiResponse<ProblemDetails> response;

            if (exception is AppException ex && ex.IsOperational)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)ex.StatusCode,
                    Title = ex.ErrorCode.ToString(),
                    Detail = ex.Message
                };

                response = new ApiResponse<ProblemDetails>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = problemDetails,
                    StatusCode = ex.StatusCode
                };
            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = Enum.GetName(ErrorCode.INTERNAL_SERVER_ERROR),
                    Detail = "An unexpected error occurred."
                };

                response = new ApiResponse<ProblemDetails>
                {
                    Success = false,
                    Message = "Internal server error",
                    Data = problemDetails,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            httpContext.Response.StatusCode = (int)response.StatusCode;

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken
            );

            return true;
        }
    }

}
