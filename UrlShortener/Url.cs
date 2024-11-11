namespace OfflineBootcamp.UrlShortener;

public class Url
{
    public string? OriginalUrl { get; set; }
    public string? ShortCode { get; set; }
    public int AccessCount { get; set; } = 0; 
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
public interface IConfigService
{
    string GetBaseHostname();
}
public interface IQrCodeService
{
    void GenerateQrCode(string shortUrl);
}
public interface IShortCodeGenerator
{
    string GenerateShortCode(string longUrl);
}
public interface IUrlShortener
{
    string ShortenUrl(string longUrl);
    string GetOriginalUrl(string shortCode);
    IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10);
}

