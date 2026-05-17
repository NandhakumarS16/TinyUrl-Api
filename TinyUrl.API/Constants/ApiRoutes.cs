namespace TinyUrl.API.Constants;

/// <summary>
/// Defines API route constants for Tiny URL endpoints.
/// </summary>
public static class ApiRoutes
{
    public const string Add = "/api/add";

    public const string GetPublic = "/api/public";

    public const string DeleteOne = "/api/delete/{code}";

    public const string DeleteAll = "/api/delete-all";

    public const string Update = "/api/update/{code}";

    // Catch-all redirect route
    public const string Redirect = "/{code}";
}