namespace TinyUrl.Application.DTOs.TinyUrlDto;

/// <summary>
/// Request model for creating a new Tiny URL.
/// </summary>
public class TinyUrlAddDto
{
    /// <summary>
    /// The original URL to be shortened.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the generated short URL should be private.
    /// </summary>
    public bool IsPrivate { get; set; }
}