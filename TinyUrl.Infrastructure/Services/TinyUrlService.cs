using TinyUrl.Application.DTOs.TinyUrlDto;
using TinyUrl.Application.Interfaces.Repositories;
using TinyUrl.Application.Interfaces.Services;

using TinyUrl.Domain.Entities;

/// <summary>
/// Service implementation for managing Tiny URLs.
/// </summary>
public class TinyUrlService : ITinyUrlService
{
    private readonly ITinyUrlRepository _repo;

    /// <summary>
    /// Characters used for short code generation.
    /// </summary>
    // 62 alphanumeric characters → 62^6 = ~56 billion combinations
    private const string Chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    /// <summary>
    /// Initializes a new instance of the <see cref="TinyUrlService"/> class.
    /// </summary>
    /// <param name="repo">Tiny URL repository.</param>
    public TinyUrlService(ITinyUrlRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Generates a unique 6-character short code.
    /// </summary>
    /// <returns>A unique short code.</returns>
    private async Task<string> GenerateUniqueCodeAsync()
    {
        string code;
        var random = new Random();

        do
        {
            code = new string(
                Enumerable.Range(0, 6)
                    .Select(_ => Chars[random.Next(Chars.Length)])
                    .ToArray());
        }
        while (await _repo.CodeExistsAsync(code));

        return code;
    }

    /// <summary>
    /// Maps an entity to a response DTO.
    /// </summary>
    /// <param name="entity">Tiny URL entity.</param>
    /// <param name="baseUrl">Application base URL.</param>
    /// <returns>Mapped response DTO.</returns>
    private static TinyUrlResponseDto MapToDto(
        TinyUrlEntity entity,
        string baseUrl)
    {
        return new TinyUrlResponseDto
        {
            Id = entity.Id,
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            ShortUrl = $"{baseUrl}/{entity.ShortCode}",
            IsPrivate = entity.IsPrivate,
            Clicks = entity.Clicks,
            CreatedAt = entity.CreatedAt
        };
    }

    /// <summary>
    /// Creates a new short URL.
    /// </summary>
    /// <param name="dto">URL request data.</param>
    /// <param name="baseUrl">Application base URL.</param>
    /// <returns>Created short URL details.</returns>
    public async Task<TinyUrlResponseDto> AddAsync(
        TinyUrlAddDto dto,
        string baseUrl)
    {
        try
        {
            var code = await GenerateUniqueCodeAsync();

            var entity = new TinyUrlEntity
            {
                OriginalUrl = dto.Url,
                ShortCode = code,
                IsPrivate = dto.IsPrivate,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _repo.AddAsync(entity);

            return MapToDto(saved, baseUrl);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create short URL.", ex);
        }
    }

    /// <summary>
    /// Gets all public short URLs.
    /// </summary>
    /// <param name="search">Optional search keyword.</param>
    /// <param name="baseUrl">Application base URL.</param>
    /// <returns>List of public short URLs.</returns>
    public async Task<List<TinyUrlResponseDto>> GetPublicAsync(
        string? search,
        string baseUrl)
    {
        try
        {
            var list = await _repo.GetAllPublicAsync(search);

            return list
                .Select(x => MapToDto(x, baseUrl))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve URLs.", ex);
        }
    }

    /// <summary>
    /// Updates an existing short URL.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <param name="dto">Updated URL data.</param>
    /// <param name="baseUrl">Application base URL.</param>
    /// <returns>Updated short URL details if found.</returns>
    public async Task<TinyUrlResponseDto?> UpdateAsync(
        string code,
        TinyUrlUpdateDto dto,
        string baseUrl)
    {
        try
        {
            var updated = await _repo.UpdateAsync(
                code,
                dto.Url,
                dto.IsPrivate);

            return updated is null
                ? null
                : MapToDto(updated, baseUrl);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update short URL.", ex);
        }
    }

    /// <summary>
    /// Deletes a short URL by code.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <returns>True if deleted; otherwise false.</returns>
    public async Task<bool> DeleteAsync(string code)
    {
        try
        {
            return await _repo.DeleteAsync(code);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete short URL.", ex);
        }
    }

    /// <summary>
    /// Deletes all short URLs.
    /// </summary>
    public async Task DeleteAllAsync()
    {
        try
        {
            await _repo.DeleteAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete all URLs.", ex);
        }
    }

    /// <summary>
    /// Gets the original URL from a short code.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <returns>The original URL if found.</returns>
    public async Task<string?> GetOriginalUrlAsync(string code)
    {
        try
        {
            var entity = await _repo.GetByCodeAsync(code);

            if (entity is null)
                return null;

            await _repo.IncrementClickAsync(code);

            return entity.OriginalUrl;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve original URL.", ex);
        }
    }
}