using PhysicalData.Presentation.Result;

namespace Presentation.Result
{
    public static class DefaultApiError
    {
        public static ApiError TaskAborted = new ApiError() { Code = "TASK_ABORTED", Description = "Task has been cancelled." };
        public static ApiError UnexpectedStatusCode = new ApiError() { Code = "UNEXPECTED_STATUS_CODE", Description = "Status code is not expected at this endpoint." };
        public static ApiError DeserializationReturnsNull = new ApiError() { Code = "DESERIALIZATION_RETURNS_NULL", Description = "Deserialization returns null" };

        public static class TokenStorage
        {
            public static ApiError ReturnsInvalidValue = new ApiError() { Code = "STORAGE_VALUE_INVALID", Description = "Could not read value from storage." };
            public static ApiError ReturnsInvalidToken = new ApiError() { Code = "STORAGE_TOKEN_INVALID", Description = "Could not read token from storage." };
            public static ApiError TokenIsNotStored = new ApiError() { Code = "STORAGE_TOKEN_NOT_STORED", Description = "Could not write token to storage." };
            public static ApiError TokenIsNotRemoved = new ApiError() { Code = "STORAGE_TOKEN_NOT_REMOVED", Description = "Could not remove token from storage." };
        }

        public static class Api
        {
            public static ApiError BadRequest = new ApiError() { Code = "API_BAD_REQUEST", Description = "Bad request" };
            public static ApiError NotAuthenticated = new ApiError() { Code = "API_NOT_AUTHENTICATED", Description = "Authentication is required." };
            public static ApiError NotAuthorized = new ApiError() { Code = "API_NOT_AUTHORIZED", Description = "Authorization is required." };
        }
    }
}