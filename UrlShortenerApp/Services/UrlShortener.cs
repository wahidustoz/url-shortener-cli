using System.Collections.Generic;

public class UrlShortener : IUrlShortener
{
    private readonly IShortCodeGenerator _shortCodeGenerator;
    private readonly Dictionary<string, Url> _urlMappings = new();

    public UrlShortener(IShortCodeGenerator shortCodeGenerator)
    {
        _shortCodeGenerator = shortCodeGenerator;
    }

    public string ShortenUrl(string longUrl)
    {
        var shortCode = _shortCodeGenerator.GenerateShortCode(longUrl);

        var url = new Url { OriginalUrl = longUrl, ShortCode = shortCode };

        // Mappingga qo'shish
        _urlMappings[shortCode] = url;

        return shortCode;
    }

    public string GetOriginalUrl(string shortCode)
    {
        return _urlMappings.TryGetValue(shortCode, out var url) ? url.OriginalUrl : null;
    }

    public IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10)
    {
        return _urlMappings.Values.Skip(pageNumber*pageSize).Take(pageSize);
    }
}
