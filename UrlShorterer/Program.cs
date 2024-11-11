
using Dotnet.Bootcamp.UrlShorterer;
using Spectre.Console;
var exit = true;
List<Url> urlList = new();
IShortCodeGenerator shortCodeGenerator = new ShortCodeGenerator();
IUrlDisplayService urlDisplayService = new UrlDisplayService();
IConfigService configService = new ConfigService();
IQrCodeService qrCodeService = new QrCodeService();

AnsiConsole.Write(
    new FigletText("UrlShorterer")
        .LeftJustified()
        .Centered()
        .Color(Color.Gold1)
);

while (exit)
{
    var menyu = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title("[bold green]Bosh menyu[/]")
        .AddChoices(new[] { "Url qisqartirish", "Url lar ro'yxati", "Chiqish" }));

    if (menyu == "Url qisqartirish")
    {
        var url = AnsiConsole.Ask<string>("[bold green]Url kiriting: [/]");

        string shortCode = shortCodeGenerator.GenerateShortCode(url);
        var shortUrl = $"{configService.GetBaseHostname()}/{shortCode}";

        var urlEntry = new Url { OriginalUrl = url, ShortCode = shortUrl };
        urlList.Add(urlEntry);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[bold cyan]Sizning qisqartirilgan URLingiz: {shortUrl}[/]");
        AnsiConsole.WriteLine();

        qrCodeService.GenerateQrCode(shortUrl);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold cyan]Sizning qr codingiz[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();


    }
    else if (menyu == "Url lar ro'yxati")
    {
        if (urlList.Count == 0)
        {
            AnsiConsole.MarkupLine("[bold red]Url lar ro'yxati bo'sh[/]");
        }
        else
        {
            urlDisplayService.DisplayUrls(urlList);
        }
    }
    else if (menyu == "Chiqish")
    {
        exit = false;
    }
}






