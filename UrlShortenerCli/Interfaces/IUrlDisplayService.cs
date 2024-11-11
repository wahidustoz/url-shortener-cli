using UrlShortenerCli.Models;

namespace UrlShortenerCli.Interfaces;

public interface IUrlDisplayService
{
    void DisplayUrls(IEnumerable<Url> urls);
    void DisplayPaginatedUrls(int pageNumber);
}
