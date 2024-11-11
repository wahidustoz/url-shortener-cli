namespace Services.UrlShortenerService;

public class UrlShortenerService : IUrlShortenerService
{
    public string GetOriginalUrl(string shortCode)
    {
        if (AllDatas.AllUrls.ContainsKey(shortCode) is false)
            return null!;

        return AllDatas.AllUrls[shortCode]?.OriginalUrl!;
    }

    public IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10)
    {
        int i = 1;

        foreach(var shortcodes in AllDatas.AllUrls.Keys)
        {
            if(i > pageNumber * pageSize)
                break;

            if(i++ <= (pageNumber - 1) * pageSize)
                continue;
            
            yield return AllDatas.AllUrls[shortcodes];          
        }
    }

    public string ShortenUrl(string longUrl)
    {
        var service = new ShortCodeGeneratorService();

        if(AllDatas.allUrls.ContainsKey(longUrl) is true)
            return AllDatas.allUrls[longUrl];
            
        var shortUrl = service.GenerateShortCode(longUrl);
        var url = new Url()
        {
            ShortCode = shortUrl,
            OriginalUrl = longUrl,
            DateCreated = DateTime.UtcNow
        };

        AllDatas.AllUrls.Add(shortUrl, url);

        return shortUrl;
    }
}
