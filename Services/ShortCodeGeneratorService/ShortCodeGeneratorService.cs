using System.Security.Cryptography;
using System.Text;

public class ShortCodeGeneratorService : IShortCodeGeneratorService
{
    public string GenerateShortCode(string longUrl)
    {
        if(AllDatas.allUrls.ContainsKey(longUrl))
            return AllDatas.allUrls[longUrl];

        string hash = GenerateHash(longUrl);
        string shortUrl = GetBaseUrl(longUrl) + hash;

        AllDatas.allUrls[longUrl] = shortUrl;
        
        return shortUrl;
    }
    private string GetBaseUrl(string url)
    {
        string trimmedUrl = url.Trim();
        trimmedUrl = trimmedUrl.Replace("https://", "");
        trimmedUrl = trimmedUrl.Replace("http://", "");
        return "https://" + trimmedUrl.Split('/')[0] + "/";
    }

    private string GenerateHash(string input)
    {
        using var sha256 = SHA256.Create();

        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        var builder = new StringBuilder();
        for(int i = 0; i < 4; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        
        return builder.ToString();
    }
}

