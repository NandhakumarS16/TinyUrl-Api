using TinyUrl.Domain.Entities;

namespace TinyUrl.Application.Interfaces.Repositories;

public interface ITinyUrlRepository
{
    Task<TinyUrlEntity?> GetByCodeAsync(string code);
    Task<List<TinyUrlEntity>> GetAllPublicAsync(string? search = null);
    Task<TinyUrlEntity> AddAsync(TinyUrlEntity entity);
    Task<TinyUrlEntity?> UpdateAsync(string code, string? url, bool? isPrivate);
    Task<bool> DeleteAsync(string code);
    Task DeleteAllAsync();
    Task IncrementClickAsync(string code);
    Task<bool> CodeExistsAsync(string code);
}