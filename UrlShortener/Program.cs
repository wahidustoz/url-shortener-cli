using OfflineBootcamp.UrlShortener;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using Net.Codecrete.QrCodeGenerator;
using System.IO;
using System.Drawing;
class Program
{
    static void Main(string[] args)
    {
        var configService = new ConfigService();
        var urlShortener = new UrlShortener(configService);
        var qrCodeService = new QrCodeService();

        int sahifaRaqami = 1;
        bool davomEtmoqda = true;

        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("URL Qisqartiruvchi").Color(Spectre.Console.Color.Green));

        while (davomEtmoqda)
        {
            var variantlar = new[] { "URL qisqartirish", "Asl URLni olish", "Barcha URLlarni ko'rsatish", "Chiqish" };
            var tanlov = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Variantlardan birini tanlang:")
                    .PageSize(4)
                    .AddChoices(variantlar));

            switch (tanlov)
            {
                case "URL qisqartirish":
                    string uzunUrl = AnsiConsole.Ask<string>("Qisqartiriladigan [green]URLni kiriting:[/]");
                    string qisqaUrl = urlShortener.ShortenUrl(uzunUrl);
                    AnsiConsole.MarkupLine($"[blue]Qisqartirilgan URL:[/] {qisqaUrl}");
                    qrCodeService.GenerateQrCode(qisqaUrl);
                    break;

                case "Asl URLni olish":
                    string qisqaKod = AnsiConsole.Ask<string>("Asl URLni olish uchun [green]qisqa kodni kiriting:[/]");
                    string? aslUrl = urlShortener.GetOriginalUrl(qisqaKod);
                    if (aslUrl == null)
                    {
                        AnsiConsole.MarkupLine("[red]Xato:[/] URL topilmadi.");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[blue]Asl URL:[/] {aslUrl}");
                    }
                    break;

                case "Barcha URLlarni ko'rsatish":
                    int jamiUrllar = urlShortener.GetTotalUrlCount();
                    int jamiSahifalar = (int)Math.Ceiling(jamiUrllar / 10.0); // Har bir sahifada 10 ta URL

                    var urllar = urlShortener.GetPaginatedUrls(sahifaRaqami);
                    var jadval = new Table();
                    jadval.AddColumn("Qisqa Kod");
                    jadval.AddColumn("Asl URL");
                    jadval.AddColumn("Ko'rishlar soni");

                    foreach (var url in urllar)
                    {
                        jadval.AddRow(url.ShortCode!, url.OriginalUrl!, url.AccessCount.ToString());
                    }

                    AnsiConsole.Write(jadval);

                    if (sahifaRaqami > 1)
                    {
                        AnsiConsole.MarkupLine($"[yellow]Sahifa {sahifaRaqami}/{jamiSahifalar}[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]Sahifa 1/{jamiSahifalar}[/]");
                    }

                    string? amal = AnsiConsole.Prompt(new SelectionPrompt<string?>().AddChoices(
                        sahifaRaqami > 1 ? "Oldingi Sahifa" : null,
                        sahifaRaqami < jamiSahifalar ? "Keyingi Sahifa" : null,
                        "Orqaga"));

                    if (amal == "Keyingi Sahifa" && sahifaRaqami < jamiSahifalar)
                    {
                        sahifaRaqami++;
                    }
                    else if (amal == "Oldingi Sahifa" && sahifaRaqami > 1)
                    {
                        sahifaRaqami--;
                    }
                    break;

                case "Chiqish":
                    davomEtmoqda = false;
                    break;
            }

            System.Threading.Thread.Sleep(1000);
        }
    }


    public class UrlShortener : IUrlShortener
    {
        private readonly IConfigService _configService;
        private readonly InMemoryUrlRepository _urlRepository;
        private readonly IShortCodeGenerator _shortCodeGenerator;

        public UrlShortener(IConfigService configService)
        {
            _configService = configService;
            _urlRepository = new InMemoryUrlRepository();
            _shortCodeGenerator = new ShortCodeGenerator();
        }

        public string ShortenUrl(string longUrl)
        {
            if (_urlRepository.ContainsLongUrl(longUrl))
            {
                return _urlRepository.GetShortCode(longUrl);
            }

            string shortCode = _shortCodeGenerator.GenerateShortCode(longUrl);  
            _urlRepository.AddUrl(longUrl, shortCode);  
            return $"{_configService.GetBaseHostname()}/{shortCode}";
        }

        public string GetOriginalUrl(string shortCode)
        {
            var url = _urlRepository.GetUrlByShortCode(shortCode);
            return url?.OriginalUrl!;
        }

        public IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize = 10)
        {
            return _urlRepository.GetPaginatedUrls(pageNumber, pageSize);
        }

        public int GetTotalUrlCount()
        {
            return _urlRepository.GetTotalUrlCount();
        }
    }


    public class ShortCodeGenerator : IShortCodeGenerator
    {
        private static readonly string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        public string GenerateShortCode(string longUrl)
        {
            var shortCode = GenerateRandomShortCode();

            return shortCode;
        }

        private string GenerateRandomShortCode(int length = 8)
        {
            return new string(Enumerable.Repeat(chars, length)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray());
        }
    }
    public class InMemoryUrlRepository
    {
        private readonly Dictionary<string, Url> _shortCodeToUrl = new();
        private readonly Dictionary<string, string> _longUrlToShortCode = new();

        public bool ContainsLongUrl(string longUrl) => _longUrlToShortCode.ContainsKey(longUrl);

        public string GetShortCode(string longUrl) => _longUrlToShortCode[longUrl];

        public Url? GetUrlByShortCode(string shortCode)  
        {
            if (_shortCodeToUrl.ContainsKey(shortCode))
            {
                return _shortCodeToUrl[shortCode];
            }
            return null; 
        }

        public void AddUrl(string longUrl, string shortCode)
        {
            var url = new Url { OriginalUrl = longUrl, ShortCode = shortCode };
            _shortCodeToUrl[shortCode] = url;
            _longUrlToShortCode[longUrl] = shortCode;
        }

        public IEnumerable<Url> GetPaginatedUrls(int pageNumber, int pageSize)
        {
            return _shortCodeToUrl.Values.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public int GetTotalUrlCount()
        {
            return _shortCodeToUrl.Count;
        }
    }

    public class QrCodeService : IQrCodeService
    {
        public void GenerateQrCode(string shortUrl)
        {
            var qrCode = QrCode.EncodeText(shortUrl, QrCode.Ecc.Medium);
            int size = qrCode.Size;
            int pixelSize = 10;

            using (var bitmap = new Bitmap(size * pixelSize, size * pixelSize))
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        System.Drawing.Color color = qrCode.GetModule(x, y) ? System.Drawing.Color.Black : System.Drawing.Color.White;
                        for (int dy = 0; dy < pixelSize; dy++)
                        {
                            for (int dx = 0; dx < pixelSize; dx++)
                            {
                                bitmap.SetPixel(x * pixelSize + dx, y * pixelSize + dy, color);
                            }
                        }
                    }
                }

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "qrcode.png");
                bitmap.Save(filePath);
                AnsiConsole.MarkupLine($"[green]QR kod {filePath} ga saqlandi[/]");

                var qrCodeText = qrCode.ToString();
                AnsiConsole.WriteLine(qrCodeText!);
            }
        }
    }
    public class ConfigService : IConfigService
    {
        public string GetBaseHostname()
        {
            string hostname = Environment.GetEnvironmentVariable("HOSTNAME") ?? "https://url.ilmhub.uz";
            return hostname;
        }
    }
}
