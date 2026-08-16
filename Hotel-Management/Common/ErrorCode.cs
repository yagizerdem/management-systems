namespace Common
{
    public enum ErrorCode
    {
        // General
        UNKNOWN_ERROR,
        INTERNAL_SERVER_ERROR,
        VALIDATION_ERROR,
        INVALID_REQUEST,
        RESOURCE_NOT_FOUND,
        CONFLICT,

        // Authentication / Authorization
        UNAUTHORIZED,
        FORBIDDEN,
        INVALID_CREDENTIALS,
        TOKEN_EXPIRED,
        INVALID_TOKEN,
    }
}
