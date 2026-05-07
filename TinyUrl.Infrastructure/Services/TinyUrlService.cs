using TinyUrl.Application.DTOs;
using TinyUrl.Application.Interfaces.Repositories;
using TinyUrl.Application.Interfaces.Services;
using TinyUrl.Domain.Entities;

namespace TinyUrl.Infrastructure.Services;

public class TinyUrlService : ITinyUrlService
{
    private readonly ITinyUrlRepository _repo;

    // 62 alphanumeric characters → 62^6 = ~56 billion unique codes
    private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public TinyUrlService(ITinyUrlRepository repo)
    {
        _repo = repo;
    }

    // ── Generate a unique 6-character short code ─────────────────────────────
    private async Task<string> GenerateUniqueCodeAsync()
    {
        string code;
        var random = new Random();
        do
        {
            code = new string(
                Enumerable.Range(0, 6)
                          .Select(_ => Chars[random.Next(Chars.Length)])
                          .ToArray()
            );
        }
        while (await _repo.CodeExistsAsync(code));

        return code;
    }

    private static TinyUrlResponseDto MapToDto(TinyUrlEntity entity, string baseUrl) => new()
    {
        Id = entity.Id,
        OriginalUrl = entity.OriginalUrl,
        ShortCode = entity.ShortCode,
        ShortUrl = $"{baseUrl}/{entity.ShortCode}",
        IsPrivate = entity.IsPrivate,
        Clicks = entity.Clicks,
        CreatedAt = entity.CreatedAt
    };

    public async Task<TinyUrlResponseDto> AddAsync(TinyUrlAddDto dto, string baseUrl)
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

    public async Task<List<TinyUrlResponseDto>> GetPublicAsync(string? search, string baseUrl)
    {
        var list = await _repo.GetAllPublicAsync(search);
        return list.Select(x => MapToDto(x, baseUrl)).ToList();
    }

    public async Task<TinyUrlResponseDto?> UpdateAsync(string code, TinyUrlUpdateDto dto, string baseUrl)
    {
        var updated = await _repo.UpdateAsync(code, dto.Url, dto.IsPrivate);
        return updated is null ? null : MapToDto(updated, baseUrl);
    }

    public Task<bool> DeleteAsync(string code) => _repo.DeleteAsync(code);
    public Task DeleteAllAsync() => _repo.DeleteAllAsync();

    public async Task<string?> GetOriginalUrlAsync(string code)
    {
        var entity = await _repo.GetByCodeAsync(code);
        if (entity is null) return null;

        await _repo.IncrementClickAsync(code);
        return entity.OriginalUrl;
    }
}