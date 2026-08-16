namespace Common
{
    public enum ErrorCode
    {
        // Programmatic Errors
        INVALID_CONFIGURATION,

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
        TOKEN_EXPIRED,
        INVALID_TOKEN,
        USER_NOT_FOUND,
        INVALID_PASSWORD,
        INVALID_USER_ROLE,
        USER_REGISTRATION_FAILED,
        USER_ROLE_ASSIGNMENT_FAILED

    }
}
