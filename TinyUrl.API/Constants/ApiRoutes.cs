// Defines all API route constants used across the Tiny URL API to ensure
// consistent and centralized route management.
namespace TinyUrl.API.Constants;

public static class ApiRoutes
{
    public const string Add = "/api/add";
    public const string GetPublic = "/api/public";
    public const string DeleteOne = "/api/delete/{code}";
    public const string DeleteAll = "/api/delete-all";
    public const string Update = "/api/update/{code}";
    public const string Redirect = "/{code}";
}