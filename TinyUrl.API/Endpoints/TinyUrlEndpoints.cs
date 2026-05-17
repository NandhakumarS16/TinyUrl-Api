using TinyUrl.API.Constants;
using TinyUrl.API.Handlers;

/// <summary>
/// Maps Tiny URL API endpoints to their respective handlers.
/// </summary>
public static class TinyUrlEndpoints
{
    /// <summary>
    /// Registers all Tiny URL endpoints.
    /// </summary>
    /// <param name="app">Web application instance.</param>
    public static void MapTinyUrlEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("")
            .WithTags("TinyUrl");

        group.MapPost(ApiRoutes.Add, TinyUrlHandlers.Add)
            .WithName("AddTinyUrl")
            .WithSummary("Create a new short URL");

        group.MapGet(ApiRoutes.GetPublic, TinyUrlHandlers.GetPublic)
            .WithName("GetPublicUrls")
            .WithSummary("Retrieve all public short URLs");

        group.MapDelete(ApiRoutes.DeleteOne, TinyUrlHandlers.Delete)
            .WithName("DeleteTinyUrl")
            .WithSummary("Delete a short URL by code");

        group.MapDelete(ApiRoutes.DeleteAll, TinyUrlHandlers.DeleteAll)
            .WithName("DeleteAllTinyUrls")
            .WithSummary("Delete all short URLs");

        group.MapPut(ApiRoutes.Update, TinyUrlHandlers.Update)
            .WithName("UpdateTinyUrl")
            .WithSummary("Update an existing short URL");

        // Must be last to avoid route conflicts
        group.MapGet(ApiRoutes.Redirect, TinyUrlHandlers.Redirect)
            .WithName("RedirectTinyUrl")
            .WithSummary("Redirect to original URL");
    }
}