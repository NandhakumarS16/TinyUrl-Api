using TinyUrl.API.Constants;

using TinyUrl.API.Handlers;

/// <summary>
/// Defines all TinyURL-related API endpoints and maps them to their respective handlers.
/// </summary>
public static class TinyUrlEndpoints
{
    /// <summary>
    /// Maps TinyURL endpoints (create, retrieve, update, delete, and redirect) to the application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance used to register endpoints.</param>
    public static void MapTinyUrlEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("")
                       .WithTags("tiny-url");

        /// <summary>
        /// Creates a new short URL entry.
        /// </summary>
        group.MapPost(ApiRoutes.Add, TinyUrlHandlers.Add)
             .WithName("AddTinyUrl")
             .WithSummary("Create a new short URL");

        /// <summary>
        /// Retrieves all publicly available short URLs.
        /// </summary>
        group.MapGet(ApiRoutes.GetPublic, TinyUrlHandlers.GetPublic)
             .WithName("GetPublicUrls")
             .WithSummary("List all public short URLs");

        /// <summary>
        /// Deletes a specific short URL identified by its code.
        /// </summary>
        group.MapDelete(ApiRoutes.DeleteOne, TinyUrlHandlers.Delete)
             .WithName("DeleteTinyUrl")
             .WithSummary("Delete a short URL by code");

        /// <summary>
        /// Deletes all existing short URLs.
        /// </summary>
        group.MapDelete(ApiRoutes.DeleteAll, TinyUrlHandlers.DeleteAll)
             .WithName("DeleteAllTinyUrls")
             .WithSummary("Delete all short URLs");

        /// <summary>
        /// Updates an existing short URL entry.
        /// </summary>
        group.MapPut(ApiRoutes.Update, TinyUrlHandlers.Update)
             .WithName("UpdateTinyUrl")
             .WithSummary("Update a short URL");

        // Must be LAST — catch-all short code redirect

        /// <summary>
        /// Redirects a short URL code to its original destination and increments click count.
        /// This must be the last route to avoid route conflicts.
        /// </summary>
        group.MapGet(ApiRoutes.Redirect, TinyUrlHandlers.Redirect)
             .WithName("RedirectTinyUrl")
             .WithSummary("Redirect to original URL and count click");
    }
}