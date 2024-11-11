using Spectre.Console;
class ConsoleApp : IConsoleApp  
{  
    private readonly IURLShortener _urlShortener;  
    private readonly IURLStorage _urlStorage;  
    private readonly Dictionary<string, string> _urlMapping;  

    public ConsoleApp(IURLShortener urlShortener, IURLStorage urlStorage)  
    {  
        _urlShortener = urlShortener;  
        _urlStorage = urlStorage;  
        _urlMapping = _urlStorage.LoadURLMapping();  
    }  

    public void Run()  
    {  
        while (true)  
        {  
            AnsiConsole.Markup("[blue]Enter a command (shorten/view/exit): [/] ");  
            string command = Console.ReadLine()?.ToLower();  

            switch (command)  
            {  
                case "shorten":  
                    ShortenURL();  
                    break;  

                case "view":  
                    DisplayURLs();  
                    break;  

                case "exit":  
                    _urlStorage.SaveURLMapping(_urlMapping);  
                    return;  

                default:  
                    AnsiConsole.MarkupLine("[red]Invalid command. Please enter 'shorten', 'view' or 'exit'.[/]");  
                    break;  
            }  
        }  
    }  

    private void ShortenURL()  
    {  
        AnsiConsole.Markup("[blue]Enter the URL to shorten: [/] ");  
        string originalURL = Console.ReadLine();  

        if (!string.IsNullOrWhiteSpace(originalURL))  
        {  
            string shortURL = _urlShortener.ShortenURL(originalURL);  
            _urlMapping[shortURL] = originalURL;  
            AnsiConsole.MarkupLine($"Shortened URL: {shortURL}\n");  
        }  
    }  

    private void DisplayURLs()  
    {  
        Console.Clear();  
        AnsiConsole.MarkupLine("[green]Shortened URLs:[/]");  

        foreach (var (shortUrl, originalUrl) in _urlMapping)  
        {  
            AnsiConsole.MarkupLine($"[green]{shortUrl}[/] - [blue]{originalUrl}[/]");  
        }  
    }  
}
