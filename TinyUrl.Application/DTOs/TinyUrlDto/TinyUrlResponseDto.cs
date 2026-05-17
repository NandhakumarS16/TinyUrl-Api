namespace TinyUrl.Application.DTOs.TinyUrlDto;

/// <summary>
/// Response model representing a Tiny URL entry.
/// </summary>
public class TinyUrlResponseDto
{
    /// <summary>
    /// Unique identifier of the Tiny URL record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The original long URL provided by the user.
    /// </summary>
    public string OriginalUrl { get; set; } = string.Empty;

    /// <summary>
    /// The generated short code for the URL.
    /// </summary>
    public string ShortCode { get; set; } = string.Empty;

    /// <summary>
    /// Fully constructed short URL.
    /// </summary>
    public string ShortUrl { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the URL is private or public.
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Number of times the short URL has been accessed.
    /// </summary>
    public int Clicks { get; set; }

    /// <summary>
    /// Timestamp when the Tiny URL was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}