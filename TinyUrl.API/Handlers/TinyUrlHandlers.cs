using TinyUrl.Application.Common;
using TinyUrl.Application.DTOs;
using TinyUrl.Application.Interfaces.Services;

namespace TinyUrl.API.Handlers;

public static class TinyUrlHandlers
{
    // POST /api/add
    public static async Task<IResult> Add(
        TinyUrlAddDto dto,
        ITinyUrlService service,
        HttpContext ctx)
    {
        if (string.IsNullOrWhiteSpace(dto.Url))
            return Results.BadRequest(ApiResponse<string>.Fail("URL is required."));

        var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";
        var result = await service.AddAsync(dto, baseUrl);
        return Results.Ok(ApiResponse<TinyUrlResponseDto>.Ok(result, "Short URL created."));
    }

    // GET /api/public?search=
    public static async Task<IResult> GetPublic(
        ITinyUrlService service,
        HttpContext ctx,
        string? search = null)
    {
        var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";
        var result = await service.GetPublicAsync(search, baseUrl);
        return Results.Ok(ApiResponse<List<TinyUrlResponseDto>>.Ok(result));
    }

    // DELETE /api/delete/{code}
    public static async Task<IResult> Delete(string code, ITinyUrlService service)
    {
        var deleted = await service.DeleteAsync(code);
        return deleted
            ? Results.Ok(ApiResponse<bool>.Ok(true, "Deleted successfully."))
            : Results.NotFound(ApiResponse<bool>.Fail($"Code '{code}' not found."));
    }

    // DELETE /api/delete-all
    public static async Task<IResult> DeleteAll(ITinyUrlService service)
    {
        await service.DeleteAllAsync();
        return Results.Ok(ApiResponse<bool>.Ok(true, "All URLs deleted."));
    }

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
            ? Results.Ok(ApiResponse<TinyUrlResponseDto>.Ok(result, "Updated."))
            : Results.NotFound(ApiResponse<TinyUrlResponseDto>.Fail($"Code '{code}' not found."));
    }

    // GET /{code}  →  redirect
    public static async Task<IResult> Redirect(string code, ITinyUrlService service)
    {
        var url = await service.GetOriginalUrlAsync(code);
        return url is not null
            ? Results.Redirect(url)
            : Results.NotFound($"Short code '{code}' not found.");
    }
}