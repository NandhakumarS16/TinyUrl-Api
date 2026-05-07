namespace TinyUrl.Application.DTOs;

public class TinyUrlAddDto
{
    public string Url { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
}

public class TinyUrlUpdateDto
{
    public string? Url { get; set; }
    public bool? IsPrivate { get; set; }
}

public class TinyUrlResponseDto
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public int Clicks { get; set; }
    public DateTime CreatedAt { get; set; }
}