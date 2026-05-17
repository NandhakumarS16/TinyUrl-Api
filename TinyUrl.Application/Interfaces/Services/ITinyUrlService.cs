using TinyUrl.Application.DTOs.TinyUrlDto;

namespace TinyUrl.Application.Interfaces.Services;

public interface ITinyUrlService
{
    Task<TinyUrlResponseDto> AddAsync(TinyUrlAddDto dto, string baseUrl);
    Task<List<TinyUrlResponseDto>> GetPublicAsync(string? search, string baseUrl);
    Task<TinyUrlResponseDto?> UpdateAsync(string code, TinyUrlUpdateDto dto, string baseUrl);
    Task<bool> DeleteAsync(string code);
    Task DeleteAllAsync();
    Task<string?> GetOriginalUrlAsync(string code);
}