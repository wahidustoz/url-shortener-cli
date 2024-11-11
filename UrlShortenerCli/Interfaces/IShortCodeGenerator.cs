using System;

namespace UrlShortenerCli.Interfaces;

public interface IShortCodeGenerator
{
    string GenerateShortCode(string longUrl);
}
