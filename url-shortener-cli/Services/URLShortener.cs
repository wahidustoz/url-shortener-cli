// URLShortener.cs
using System.Security.Cryptography;
using System.Text;

class URLShortener : IURLShortener  
{  
    public string ShortenURL(string originalURL)  
    {  
        if (string.IsNullOrWhiteSpace(originalURL))  
            throw new ArgumentException("URL can't be null or empty", nameof(originalURL));  

        string hash = GenerateMD5(originalURL);  
        return $"http://short.url/{hash}";  
    }  

    private string GenerateMD5(string input)  
    {  
        using var md5 = MD5.Create();  
        byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));  
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower().Substring(0, 8);  
    }  
}
