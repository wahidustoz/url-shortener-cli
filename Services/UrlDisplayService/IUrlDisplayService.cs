public interface IUrlDisplayService
{
    void DisplayUrls(IEnumerable<Url> urls);
    void DisplayPaginatedUrls(int pageNumber);
}