using TinyUrl.Application.Common;
using TinyUrl.Application.DTOs.TinyUrlDto;
using TinyUrl.Application.Interfaces.Services;

namespace TinyUrl.API.Handlers;

/// <summary>
/// Handles Tiny URL API endpoints.
/// </summary>
public static class TinyUrlHandlers
{
    /// <summary>
    /// Creates a new short URL.
    /// </summary>
    /// <param name="dto">URL request data.</param>
    /// <param name="service">Tiny URL service.</param>
    /// <param name="ctx">HTTP context.</param>
    /// <returns>Created short URL details.</returns>
    // POST /api/add
    public static async Task<IResult> Add(
        TinyUrlAddDto dto,
        ITinyUrlService service,
        HttpContext ctx)
    {
        if (string.IsNullOrWhiteSpace(dto.Url))
            return Results.BadRequest(
                ApiResponse<string>.Fail("URL is required."));

        if (!Uri.TryCreate(dto.Url, UriKind.Absolute, out _))
        {
            return Results.BadRequest(
                ApiResponse<string>.Fail("Invalid URL format."));
        }

        var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";

        var result = await service.AddAsync(dto, baseUrl);

        return Results.Ok(
            ApiResponse<TinyUrlResponseDto>.Ok(
                result,
                "Short URL created."));
    }

    /// <summary>
    /// Gets all public short URLs.
    /// </summary>
    /// <param name="service">Tiny URL service.</param>
    /// <param name="ctx">HTTP context.</param>
    /// <param name="search">Optional search keyword.</param>
    /// <returns>List of short URLs.</returns>
    // GET /api/public?search=
    public static async Task<IResult> GetPublic(
        ITinyUrlService service,
        HttpContext ctx,
        string? search = null)
    {
        var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";

        var result = await service.GetPublicAsync(search, baseUrl);

        return Results.Ok(
            ApiResponse<List<TinyUrlResponseDto>>.Ok(result));
    }

    /// <summary>
    /// Deletes a short URL by code.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <param name="service">Tiny URL service.</param>
    /// <returns>Success or not found response.</returns>
    // DELETE /api/delete/{code}
    public static async Task<IResult> Delete(
        string code,
        ITinyUrlService service)
    {
        var deleted = await service.DeleteAsync(code);

        return deleted
            ? Results.Ok(
                ApiResponse<bool>.Ok(
                    true,
                    "Deleted successfully."))
            : Results.NotFound(
                ApiResponse<bool>.Fail(
                    $"Code '{code}' not found."));
    }

    /// <summary>
    /// Deletes all short URLs.
    /// </summary>
    /// <param name="service">Tiny URL service.</param>
    /// <returns>Success response.</returns>
    // DELETE /api/delete-all
    public static async Task<IResult> DeleteAll(
        ITinyUrlService service)
    {
        await service.DeleteAllAsync();

        return Results.Ok(
            ApiResponse<bool>.Ok(
                true,
                "All URLs deleted."));
    }

    /// <summary>
    /// Updates an existing short URL.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <param name="dto">Updated URL data.</param>
    /// <param name="service">Tiny URL service.</param>
    /// <param name="ctx">HTTP context.</param>
    /// <returns>Updated short URL details.</returns>
    // PUT /api/update/{code}
    public static async Task<IResult> Update(
        string code,
        TinyUrlUpdateDto dto,
        ITinyUrlService service,
        HttpContext ctx)
    {
        var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";

        var result = await service.UpdateAsync(code, dto, baseUrl);

        return result is not null
            ? Results.Ok(
                ApiResponse<TinyUrlResponseDto>.Ok(
                    result,
                    "Updated."))
            : Results.NotFound(
                ApiResponse<TinyUrlResponseDto>.Fail(
                    $"Code '{code}' not found."));
    }

    /// <summary>
    /// Redirects a short URL to its original URL.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <param name="service">Tiny URL service.</param>
    /// <returns>Redirects to the original URL if found.</returns>
    // GET /{code}
    public static async Task<IResult> Redirect(
        string code,
        ITinyUrlService service)
    {
        var url = await service.GetOriginalUrlAsync(code);

        return url is not null
            ? Results.Redirect(url)
            : Results.NotFound(
                $"Short code '{code}' not found.");
    }
}