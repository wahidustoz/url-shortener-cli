using UrlShortenerCli.Services;
using Spectre.Console;
using UrlShortenerCli.Data;

while (true)
{
    AnsiConsole.MarkupLine("[bold green]Welcome![/]");
    UrlShortener urlShortener = new();
    UrlDisplayService urlDisplayService = new();

    while (true)
    {
        AnsiConsole.MarkupLine("[bold blue]URL SHORTENER[/]");

        int option = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("Select an option:")
                .AddChoices(1, 2, 3)
                .UseConverter(opt => opt switch
                {
                    1 => "Make short URL",
                    2 => "Get long URL via short URL",
                    3 => "View all short-long URLs"
                }));

        switch (option)
        {
            case 1:
                Console.Clear();

                string longUrlForMakingItShort = AnsiConsole.Ask<string>("Enter the [green]Long URL[/]: ").Trim();
                if (DataBaseChecker.UrlExistenceChecker(longUrlForMakingItShort))
                {
                    AnsiConsole.MarkupLine($"[bold green]Short URL:[/] {UrlStorage.ShortAndLongUrls[longUrlForMakingItShort]}");
                    break;
                }

                string shortUrlForStoringToDb = urlShortener.ShortenUrl(longUrlForMakingItShort);

                AnsiConsole.MarkupLine($"[bold green]Short URL:[/] {shortUrlForStoringToDb}");
                break;
            case 2:
                Console.Clear();

                var shortUrlForGettingLongUrlFromDb = AnsiConsole.Ask<string>("Enter the [green]Short URL[/]: ").Trim();
                string longUrlForShowing = urlShortener.GetOriginalUrl(shortUrlForGettingLongUrlFromDb);

                AnsiConsole.MarkupLine($"[bold green][/] {longUrlForShowing}");

                break;
            case 3:
                Console.Clear();
                urlDisplayService.DisplayPaginatedUrls(1);
                Console.Clear();
                break;
        }
    }
}
