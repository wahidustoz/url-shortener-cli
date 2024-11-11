namespace Dotnet.Bootcamp.UrlShorterer;

using System;
using Net.Codecrete.QrCodeGenerator;
using Spectre.Console;

public interface IUrlShortener
{
    string ShortenUrl(string longUrl);
    string GetOriginalUrl(string shortCode);
    IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10); // Paginasiyalashni qo'llab-quvvatlash
}
public interface IShortCodeGenerator
{
    string GenerateShortCode(string longUrl);
}

public interface IUrlDisplayService
{
    void DisplayUrls(IEnumerable<Url> urls);
}

public interface IQrCodeService
{
    void GenerateQrCode(string shortUrl);
}

public interface IConfigService
{
    string GetBaseHostname();
}

public class Url
{
    public string? OriginalUrl { get; set; }
    public string? ShortCode { get; set; }
    public int AccessCount { get; set; } = 0;  
    public DateTime DateCreated { get; set; } = DateTime.Now;
}

public class UrlShortener : IUrlShortener
{
    private readonly Dictionary<string, Url> _urlDatabase = new Dictionary<string, Url>();
    private readonly IShortCodeGenerator _codeGenerator;

    public UrlShortener(IShortCodeGenerator codeGenerator)
    {
        _codeGenerator = codeGenerator;
    }

    public string ShortenUrl(string longUrl)
    {
        // Check if URL already exists
        var existingUrl = _urlDatabase.Values.FirstOrDefault(url => url.OriginalUrl == longUrl);
        if (existingUrl != null)
        {
            return existingUrl.ShortCode!;
        }

        // Generate a new shortcode and create Url object
        string shortCode = _codeGenerator.GenerateShortCode(longUrl);
        var newUrl = new Url
        {
            OriginalUrl = longUrl,
            ShortCode = shortCode
        };
        _urlDatabase[shortCode] = newUrl;

        return shortCode;
    }

    public string GetOriginalUrl(string shortCode)
    {
        if (_urlDatabase.TryGetValue(shortCode, out var url))
        {
            url.AccessCount++; // Increment access count on each retrieval
            return url.OriginalUrl!;
        }
        return null!; // Return null if shortcode doesn't exist
    }

    public IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10)
    {
        return _urlDatabase.Values
            .OrderByDescending(url => url.DateCreated)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}

public class ShortCodeGenerator : IShortCodeGenerator
{
    public string GenerateShortCode(string longUrl)
    {
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(longUrl));
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }


}

public class UrlDisplayService : IUrlDisplayService
{
    public void DisplayUrls(IEnumerable<Url> urls)
    {
        var table = new Table();
        table.AddColumn("ShortCode");
        table.AddColumn("Original URL");

        foreach (var url in urls)
        {
            table.AddRow(url.ShortCode!, url.OriginalUrl!);
        }

        AnsiConsole.Write(table);
    }
}
public class ConfigService : IConfigService
{
    public string GetBaseHostname()
    {
        return Environment.GetEnvironmentVariable("BASE_HOSTNAME") ?? "https://url.ilmhub.uz";
    }
}
public class QrCodeService : IQrCodeService
{
    public void GenerateQrCode(string shortUrl)
    {
        var qr = QrCode.EncodeText(shortUrl, QrCode.Ecc.Medium);

        for (int y = 0; y < qr.Size; y++)
        {
            for (int x = 0; x < qr.Size; x++)
            {
                AnsiConsole.Markup(qr.GetModule(x, y) ? "[black on white]  [/]" : "[white on black]  [/]");
            }
            AnsiConsole.WriteLine();
        }
    }
}