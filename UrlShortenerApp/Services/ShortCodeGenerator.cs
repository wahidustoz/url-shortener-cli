using System;
using System.Security.Cryptography;
using System.Text;

public class ShortCodeGenerator : IShortCodeGenerator
{
    private int _codeLength = 6; // Kodning boshlang'ich uzunligi

    public string GenerateShortCode(string longUrl)
    {
        using (var md5 = MD5.Create())
        {
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl + DateTime.UtcNow.Ticks));
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString.Substring(0, _codeLength);
        }
    }
}
