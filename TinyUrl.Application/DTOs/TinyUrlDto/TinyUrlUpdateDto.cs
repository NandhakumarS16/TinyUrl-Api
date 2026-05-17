namespace TinyUrl.Application.DTOs.TinyUrlDto;

/// <summary>
/// Request model for updating an existing Tiny URL.
/// </summary>
public class TinyUrlUpdateDto
{
    /// <summary>
    /// Updated original URL. If null, the existing URL remains unchanged.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Updated privacy status of the Tiny URL. If null, the existing value remains unchanged.
    /// </summary>
    public bool? IsPrivate { get; set; }
}