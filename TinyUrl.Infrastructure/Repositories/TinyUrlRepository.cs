using Microsoft.EntityFrameworkCore;
using TinyUrl.Application.Interfaces.Repositories;
using TinyUrl.Domain.Entities;
using TinyUrl.Infrastructure.Data;

namespace TinyUrl.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Tiny URL data operations.
/// </summary>
public class TinyUrlRepository : ITinyUrlRepository
{
    private readonly AppDbContext _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="TinyUrlRepository"/> class.
    /// </summary>
    /// <param name="db">Application database context.</param>
    public TinyUrlRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Gets a short URL entity by code.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <returns>Matching URL entity if found.</returns>
    public async Task<TinyUrlEntity?> GetByCodeAsync(string code)
    {
        return await _db.TinyUrls
            .FirstOrDefaultAsync(x => x.ShortCode == code);
    }

    /// <summary>
    /// Gets all public short URLs.
    /// </summary>
    /// <param name="search">Optional search keyword.</param>
    /// <returns>List of public short URLs.</returns>
    public async Task<List<TinyUrlEntity>> GetAllPublicAsync(
        string? search = null)
    {
        IQueryable<TinyUrlEntity> query = _db.TinyUrls
            .Where(x => !x.IsPrivate);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.OriginalUrl.Contains(search) ||
                x.ShortCode.Contains(search));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new short URL entity.
    /// </summary>
    /// <param name="entity">Tiny URL entity.</param>
    /// <returns>Saved URL entity.</returns>
    public async Task<TinyUrlEntity> AddAsync(TinyUrlEntity entity)
    {
        _db.TinyUrls.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Updates an existing short URL.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <param name="url">Updated original URL.</param>
    /// <param name="isPrivate">Updated privacy status.</param>
    /// <returns>Updated URL entity if found.</returns>
    public async Task<TinyUrlEntity?> UpdateAsync(
        string code,
        string? url,
        bool? isPrivate)
    {
        var entity = await _db.TinyUrls
            .FirstOrDefaultAsync(x => x.ShortCode == code);

        if (entity is null)
            return null;

        if (url is not null)
            entity.OriginalUrl = url;

        if (isPrivate.HasValue)
            entity.IsPrivate = isPrivate.Value;

        await _db.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Deletes a short URL by code.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <returns>True if deleted; otherwise false.</returns>
    public async Task<bool> DeleteAsync(string code)
    {
        var entity = await _db.TinyUrls
            .FirstOrDefaultAsync(x => x.ShortCode == code);

        if (entity is null)
            return false;

        _db.TinyUrls.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Deletes all short URLs.
    /// </summary>
    public async Task DeleteAllAsync()
    {
        // EF Core 7+ bulk delete
        await _db.TinyUrls.ExecuteDeleteAsync();
    }

    /// <summary>
    /// Increments the click count for a short URL.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    public async Task IncrementClickAsync(string code)
    {
        // EF Core 7+ bulk update
        await _db.TinyUrls
            .Where(x => x.ShortCode == code)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(
                    entity => entity.Clicks,
                    entity => entity.Clicks + 1));
    }

    /// <summary>
    /// Checks whether a short code already exists.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <returns>True if the code exists; otherwise false.</returns>
    public async Task<bool> CodeExistsAsync(string code)
    {
        return await _db.TinyUrls
            .AnyAsync(x => x.ShortCode == code);
    }
}