using Spectre.Console;

class Program
{
    static void Main()
    {
        var urlShortener = new UrlShortener(new ShortCodeGenerator());
        var urlDisplayService = new UrlDisplayService();
        var qrCodeService = new QrCodeService();
        var configService = new ConfigService();

        var app = new UrlShortenerConsoleApp(urlShortener, urlDisplayService, qrCodeService, configService);
        app.Run();
    }
}
