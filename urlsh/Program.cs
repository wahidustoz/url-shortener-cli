using urlsh;
using Spectre.Console;

Dictionary<string, string> longUrlToShortUrl = new();
Dictionary<string, string> shortUrlToLongUrl = new();

while (true)
{
    var url = AnsiConsole.Ask<string>("[green] Enter a URL:[/]");

    if (!url.Contains("https://") && !url.Contains("http://"))
    {
        AnsiConsole.Markup("[red]Invalid URL.[/][yellow] Please enter a valid URL.[/]\n");
        continue;
    }

    if (longUrlToShortUrl.ContainsKey(url))
    {
        Console.WriteLine($"[green] URL {url} has already been shortened to {longUrlToShortUrl[url]} [/] \n");
        continue;
    }

    var shortUrl = new UrlShortener().GenerateShortUrl(url);
    longUrlToShortUrl[url] = shortUrl;
    shortUrlToLongUrl[shortUrl] = url;

    Console.WriteLine($"[green]URL {url} has been shortened to {shortUrl}[/]");
}