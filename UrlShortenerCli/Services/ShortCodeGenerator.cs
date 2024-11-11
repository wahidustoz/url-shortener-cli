using UrlShortenerCli.Data;
using UrlShortenerCli.Interfaces;

namespace UrlShortenerCli.Services;

public class ShortCodeGenerator : IShortCodeGenerator
{
    public string GenerateShortCode(string longUrl)
    {
        string fullShortUrl;
        int lenghtOfShortenedUrl = 1;

        do
        {
            lenghtOfShortenedUrl++;
            Random random = new Random();
            string charactersForShortUrl = "ABCDEGHIJKLMNOPQRSTUVWXVZ1234567890";

            string shortUrl = "";

            for (int i = 0; i < lenghtOfShortenedUrl; i++)
            {
                int randomIndex = random.Next(charactersForShortUrl.Length);
                shortUrl += charactersForShortUrl[randomIndex];
            }

            fullShortUrl = $"https://url.ilmhub.uz/{shortUrl}";
        } while (UrlStorage.ShortAndLongUrls.ContainsValue(fullShortUrl));

        return fullShortUrl;
    }
}
