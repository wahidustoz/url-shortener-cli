using Services.UrlShortenerService;
using Spectre.Console;

public class UrlDisplayService : IUrlDisplayService
{
    public void DisplayPaginatedUrls(int pageNumber)
    {
        if(pageNumber < 1) 
            return;

        Console.Clear();
        var paginatedUrls = new UrlShortenerService().GetPaginatedUrls(pageNumber);

        var table = new Table();
        table.Border = TableBorder.Double;
        table.BorderColor(ConsoleColor.Magenta);
        table.AddColumns("[bold maroon]ID[/]", "[bold green]Original_URL[/]", "[bold yellow]Shorted_URL[/]", "[bold blue]Created_At[/]");
        table.Expand();
        
        int id = (pageNumber - 1) * 10 + 1;

        foreach(var url in paginatedUrls)
            table.AddRow($"[bold green]{id++}[/]", $"[bold green]{url.OriginalUrl}[/]",
             $"[bold yellow]{url.ShortCode}[/]", $"[bold blue]{url.DateCreated}[/]");

        AnsiConsole.Write(table);
        Console.WriteLine();

        var optionsTable = new Table();
        optionsTable.Border = TableBorder.Double;
        optionsTable.BorderColor(ConsoleColor.Magenta);
        optionsTable.AddColumns("[bold yellow]Previous Page: ⬅️[/]", "[bold green]Next Page: ➡️[/]",
         "[bold maroon]Chiqish uchun: Backspace[/]");

        AnsiConsole.Write(optionsTable);
    }

    public void DisplayUrls(IEnumerable<Url> urls)
    {
        int i = 1;
        DisplayPaginatedUrls(i);
        while(true)
        {
            var temp = Console.ReadKey(intercept: true);
            if (temp.Key == ConsoleKey.LeftArrow && i - 1 >= 1)
            {
                DisplayPaginatedUrls(--i);
            }
            else if (temp.Key == ConsoleKey.RightArrow && i + 1 <= AllDatas.AllUrls.Count / 10 + 1 )
            {
                DisplayPaginatedUrls(++i);
            }
            else if(temp.Key == ConsoleKey.Backspace)
                break;
        }
    }
}
