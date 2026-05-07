namespace TinyUrl.Application.Common
{
    // Represents a standard API response wrapper.
    public class ApiResponse<T>
    {
        // Indicates whether the request was successful.
        public bool Success { get; set; }

        // Optional message providing additional information about the response.
        public string? Message { get; set; }

        // The data payload returned from the API.
        public T? Data { get; set; }

        // Creates a successful API response with optional message.
        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message };

        // Creates a failed API response with an error message.
        public static ApiResponse<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }
}