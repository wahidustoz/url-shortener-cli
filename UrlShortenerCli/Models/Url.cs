namespace UrlShortenerCli.Models;

public class Url
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public int AccessCount { get; set; } = 0; // Qisqartirilgan URL har safar murojaat qilganda oshadi
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}