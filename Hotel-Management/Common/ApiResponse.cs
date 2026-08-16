using System.Net;

namespace Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiResponse() { }

        public ApiResponse(
            bool success,
            string message,
            T? data,
            HttpStatusCode statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public static ApiResponse<T> Ok(
            T data,
            string message = "Success")
        {
            return new(
                true,
                message,
                data,
                HttpStatusCode.OK
            );
        }

        public static ApiResponse<T> Created(
            T data,
            string message = "Created successfully")
        {
            return new(
                true,
                message,
                data,
                HttpStatusCode.Created
            );
        }

        public static ApiResponse<T> NoContent(
            string message = "No content")
        {
            return new(
                true,
                message,
                default,
                HttpStatusCode.NoContent
            );
        }

        public static ApiResponse<T> BadRequest(
            string message = "Bad request")
        {
            return new(
                false,
                message,
                default,
                HttpStatusCode.BadRequest
            );
        }

        public static ApiResponse<T> Unauthorized(
            string message = "Unauthorized")
        {
            return new(
                false,
                message,
                default,
                HttpStatusCode.Unauthorized
            );
        }

        public static ApiResponse<T> Forbidden(
            string message = "Forbidden")
        {
            return new(
                false,
                message,
                default,
                HttpStatusCode.Forbidden
            );
        }

        public static ApiResponse<T> NotFound(
            string message = "Resource not found")
        {
            return new(
                false,
                message,
                default,
                HttpStatusCode.NotFound
            );
        }

        public static ApiResponse<T> Conflict(
            string message = "Conflict")
        {
            return new(
                false,
                message,
                default,
                HttpStatusCode.Conflict
            );
        }

        public static ApiResponse<T> InternalServerError(
            string message = "Internal server error")
        {
            return new(
                false,
                message,
                default,
                HttpStatusCode.InternalServerError
            );
        }

        public static ApiResponse<T> Fail(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new(
                false,
                message,
                default,
                statusCode
            );
        }
    }
}