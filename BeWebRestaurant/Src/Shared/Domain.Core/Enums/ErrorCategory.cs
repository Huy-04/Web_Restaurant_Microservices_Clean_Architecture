namespace Domain.Core.Enums
{
    public enum ErrorCategory
    {
        // 400
        ValidationFailed,

        // 404
        NotFound,

        // 409
        Conflict,

        // 401
        Unauthorized,

        // 403
        Forbidden,

        // 500
        InternalServerError
    }
}