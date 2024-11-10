using Spectre.Console;
using System.Collections.Generic;
using System.Linq;

public class UrlShortenerConsoleApp
{
    private readonly IUrlShortener _urlShortener;
    private readonly IUrlDisplayService _urlDisplayService;
    private readonly IQrCodeService _qrCodeService;
    private readonly IConfigService _configService;

    public UrlShortenerConsoleApp(IUrlShortener urlShortener, IUrlDisplayService urlDisplayService, IQrCodeService qrCodeService, IConfigService configService)
    {
        _urlShortener = urlShortener;
        _urlDisplayService = urlDisplayService;
        _qrCodeService = qrCodeService;
        _configService = configService;
    }

    public void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold blue]Tanlovni tanlang:[/]")
                    .PageSize(5)
                    .AddChoices(new[] {
                        "URL qisqartirish",
                        "Asl URL'ni olish",
                        "Barcha qisqartirilgan URL'larni ko'rsatish",
                        "QR kod yaratish",
                        "Chiqish"
                    }));

            switch (choice)
            {
                case "URL qisqartirish":
                    ShortenUrl();
                    break;
                case "Asl URL'ni olish":
                    GetOriginalUrl();
                    break;
                case "Barcha qisqartirilgan URL'larni ko'rsatish":
                    ShowPaginatedUrls();
                    break;
                case "QR kod yaratish":
                    GenerateQrCode();
                    break;
                case "Chiqish":
                    AnsiConsole.MarkupLine("[bold red]Dasturdan chiqish...[/]");
                    return;
            }
        }
    }

    private void ShortenUrl()
    {
        var longUrl = AnsiConsole.Ask<string>("[green]Uzun URL ni kiriting:[/]");
        var shortCode = _urlShortener.ShortenUrl(longUrl);
        var hostname = _configService.GetBaseHostname();
        var shortUrl = $"{hostname}/{shortCode}";

        AnsiConsole.MarkupLine($"[bold yellow]Qisqartirilgan URL:[/] [blue]{shortUrl}[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("[gray]Davom etish uchun Enter ni bosing...[/]"));
    }

    private void GetOriginalUrl()
    {
        var shortCode = AnsiConsole.Ask<string>("[green]Qisqartirilgan kodni kiriting:[/]");
        var originalUrl = _urlShortener.GetOriginalUrl(shortCode);

        if (originalUrl == null)
        {
            AnsiConsole.MarkupLine("[red]Xatolik: Bunday kod topilmadi.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[bold yellow]Asl URL:[/] [blue]{originalUrl}[/]");
        }
        
        AnsiConsole.Prompt(new TextPrompt<string>("[gray]Davom etish uchun Enter ni bosing...[/]"));
    }

    private void ShowPaginatedUrls()
    {
        int pageNumber = 1;
        const int pageSize = 10;

        while (true)
        {
            var urls = _urlShortener.GetPaginatedUrls(pageNumber, pageSize).ToList();

            if (!urls.Any())
            {
                AnsiConsole.MarkupLine("[red]Hech qanday URL mavjud emas.[/]");
                break;
            }

            // URL ma'lumotlarini jadval ko'rinishida chiqarish
            var table = new Table();
            table.AddColumn("[bold]Qisqartirilgan Kod[/]");
            table.AddColumn("[bold]Asl URL[/]");
            table.AddColumn("[bold]Kiritilgan Vaqt[/]");
            table.Border(TableBorder.Rounded);

            foreach (var url in urls)
            {
                table.AddRow(url.ShortCode, url.OriginalUrl, url.DateCreated.ToString());
            }

            AnsiConsole.Write(table);

            // Navigatsiya variantlari
            var navigation = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Navigatsiya uchun tanlang:[/]")
                    .AddChoices(new[] { "Keyingi sahifa", "Oldingi sahifa", "Chiqish" }));

            if (navigation == "Keyingi sahifa")
            {
                pageNumber++;
            }
            else if (navigation == "Oldingi sahifa" && pageNumber > 1)
            {
                pageNumber--;
            }
            else
            {
                break;
            }
        }
    }

    private void GenerateQrCode()
    {
        var shortCode = AnsiConsole.Ask<string>("[green]QR kod yaratish uchun qisqartirilgan kodni kiriting:[/]");
        var hostname = _configService.GetBaseHostname();
        var shortUrl = $"{hostname}/{shortCode}";

        _qrCodeService.GenerateQrCode(shortUrl);
        
        AnsiConsole.MarkupLine($"[bold yellow]QR kod {shortUrl} uchun yaratildi.[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("[gray]Davom etish uchun Enter ni bosing...[/]"));
    }
}
