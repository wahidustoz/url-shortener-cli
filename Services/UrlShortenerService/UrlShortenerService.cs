namespace Services.UrlShortenerService;
public class UrlShortenerService : IUrlShortenerService
{
    public string GetOriginalUrl(string shortCode)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public string ShortenUrl(string longUrl)
    {
        throw new NotImplementedException();
    }
}
