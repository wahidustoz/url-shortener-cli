using Spectre.Console;
using System.Collections.Generic;
using System.Linq;

public class UrlDisplayService : IUrlDisplayService
{
    public void DisplayUrls(IEnumerable<Url> urls)
    {
        foreach (var url in urls)
        {
            AnsiConsole.MarkupLine($"[blue]{url.ShortCode}[/] - [green]{url.OriginalUrl}[/]");
        }
    }

 
}
