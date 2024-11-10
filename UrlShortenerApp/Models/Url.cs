public class Url
{
    public string OriginalUrl { get; set; }
    public string ShortCode { get; set; }
    public int AccessCount { get; set; } = 0;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
