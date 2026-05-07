using Microsoft.EntityFrameworkCore;
using TinyUrl.Application.Interfaces.Repositories;
using TinyUrl.Domain.Entities;
using TinyUrl.Infrastructure.Data;

namespace TinyUrl.Infrastructure.Repositories;

public class TinyUrlRepository : ITinyUrlRepository
{
    private readonly AppDbContext _db;

    public TinyUrlRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TinyUrlEntity?> GetByCodeAsync(string code) =>
        await _db.TinyUrls
                 .FirstOrDefaultAsync(x => x.ShortCode == code);

    public async Task<List<TinyUrlEntity>> GetAllPublicAsync(string? search = null)
    {
        var query = _db.TinyUrls.Where(x => !x.IsPrivate);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x =>
                x.OriginalUrl.Contains(search) ||
                x.ShortCode.Contains(search));

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<TinyUrlEntity> AddAsync(TinyUrlEntity entity)
    {
        _db.TinyUrls.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<TinyUrlEntity?> UpdateAsync(string code, string? url, bool? isPrivate)
    {
        var entity = await _db.TinyUrls.FirstOrDefaultAsync(x => x.ShortCode == code);
        if (entity is null) return null;

        if (url is not null) entity.OriginalUrl = url;
        if (isPrivate.HasValue) entity.IsPrivate = isPrivate.Value;

        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(string code)
    {
        var entity = await _db.TinyUrls.FirstOrDefaultAsync(x => x.ShortCode == code);
        if (entity is null) return false;

        _db.TinyUrls.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAllAsync()
    {
        // ExecuteDeleteAsync — EF Core 7+ bulk delete (no round-trip per row)
        await _db.TinyUrls.ExecuteDeleteAsync();
    }

    public async Task IncrementClickAsync(string code)
    {
        // ExecuteUpdateAsync — EF Core 7+ bulk update (single SQL UPDATE, no SELECT first)
        await _db.TinyUrls
                 .Where(x => x.ShortCode == code)
                 .ExecuteUpdateAsync(s => s.SetProperty(e => e.Clicks, e => e.Clicks + 1));
    }

    public async Task<bool> CodeExistsAsync(string code) =>
        await _db.TinyUrls.AnyAsync(x => x.ShortCode == code);
}