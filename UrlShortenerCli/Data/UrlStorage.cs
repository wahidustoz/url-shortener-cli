using UrlShortenerCli.Models;
namespace UrlShortenerCli.Data;

public static class UrlStorage
{
    public static Dictionary<string, string> ShortAndLongUrls { get; } = new();
    public static Dictionary<string, Url> AllUrls { get; } = new();
    public static int currentId = 1;
}
