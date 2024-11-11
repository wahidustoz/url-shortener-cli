using UrlShortenerCli.Interfaces;
using UrlShortenerCli.Models;
using UrlShortenerCli.Data;
using Net.Codecrete.QrCodeGenerator;

namespace UrlShortenerCli.Services;

public class UrlShortener : IUrlShortener
{
    QrCodeService qrService = new QrCodeService();
    ShortCodeGenerator shortCode = new ShortCodeGenerator();

    public string ShortenUrl(string longUrl)
    {
        var tempUrl = new Url();

        
        string shortUrl = shortCode.GenerateShortCode(longUrl);

        qrService.GenerateQrCode(shortUrl);
        
        UrlStorage.ShortAndLongUrls[longUrl] = shortUrl;

        tempUrl = new Url()
        {
            Id = UrlStorage.currentId,
            OriginalUrl = longUrl,
            ShortCode = shortUrl,
        };

        UrlStorage.currentId++;

        UrlStorage.AllUrls[UrlStorage.currentId.ToString()] = tempUrl;

        return shortUrl;
    }

    public string GetOriginalUrl(string shortCode)
    {
        var originalUrl = UrlStorage.ShortAndLongUrls.FirstOrDefault(x => x.Value == shortCode).Key;

        if (originalUrl != null)
        {
            qrService.GenerateQrCode(shortCode);

            return $"Long URL -> {originalUrl}";
        }
        else
        {
            return "Short code not found.";
        }
    }

    public IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10) =>
        UrlStorage.AllUrls.Values.Skip((pageNumber - 1) * pageSize).Take(pageSize);
}
