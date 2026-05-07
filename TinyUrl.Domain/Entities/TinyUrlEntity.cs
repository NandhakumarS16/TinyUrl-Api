namespace TinyUrl.Domain.Entities
{
    // Represents a shortened URL entity in the system.
    public class TinyUrlEntity
    {
        // Gets or sets the unique identifier for the URL record.
        public int Id { get; set; }

        // Gets or sets the original full URL provided by the user.
        public string OriginalUrl { get; set; } = string.Empty;

        // Gets or sets the generated short code used to access the original URL.
        public string ShortCode { get; set; } = string.Empty;

        // Gets or sets a value indicating whether the URL is private (restricted access).
        public bool IsPrivate { get; set; }

        // Gets or sets the number of times the shortened URL has been accessed.
        public int Clicks { get; set; }

        // Gets or sets the UTC date and time when the URL was created.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}