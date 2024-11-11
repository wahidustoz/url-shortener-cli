using Services.UrlShortenerService;
using Spectre.Console;

Dictionary<string, int> consoleOptions = new Dictionary<string, int>();
consoleOptions.Add("Kiritilgan URL ni qisqartirish", 1);
consoleOptions.Add("Qisqartirilgan URL ni uzaytirish", 2);
consoleOptions.Add("Hamma URLlar ro'yxatini olish", 3);
consoleOptions.Add("URL uchun QRCode generatsiya qilish", 4);
consoleOptions.Add("Dasturdan chiqish", 5);

var urlService = new UrlShortenerService();

bool continueValue = true;
while(continueValue)
{
    Console.Clear();
    var temp = AnsiConsole.Prompt(
           new SelectionPrompt<string>()
                .Title("Qaysi xizmatdan foydalanmoqchisiz!")
                .PageSize(10)
                .MoreChoicesText("[grey]Pastda yana boshqa variantlar bo'lishi mumkin![/]")
                .AddChoices(consoleOptions.Keys.ToArray()));

    if (consoleOptions[temp] == 5)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Amaliyot yakunlandi!");
        break;
    }
    GetProcess(temp);

}

void GetProcess(string temp)
{
    while(true)
    {
        int option = consoleOptions[temp];

        if(option == 1)
        {
            Console.Clear();
            string longUrl = AnsiConsole.Prompt(
                new TextPrompt<string>("Qisqartirmoqchi bo'lgan URLni kiriting:"));

            string shortedUrl = urlService.ShortenUrl(longUrl);
            AnsiConsole.MarkupLine($"Urlning qisqartirilgan shakli: {shortedUrl}");
            
            if(GetContinueValue() is false)
                break;

            Console.Clear();
            continue;
        }
        else if (option == 2)
        {
            Console.Clear();
             string shortedUrl = AnsiConsole.Prompt(
                new TextPrompt<string>("URLning qisqa shaklini kiriting:"));
            
            string originalUrl = urlService.GetOriginalUrl(shortedUrl);
            if(originalUrl is not null)
                AnsiConsole.MarkupLine($"Kiritilgan qisqa URLning haqiqiy ko'rinishi: {originalUrl}");
            else
                AnsiConsole.MarkupLine("Bunday qisqartimal URL mavjud emas!");

            if(GetContinueValue() is false)
                break;
            
            Console.Clear();
            continue;            
        }       

        else if(consoleOptions[temp] == 3)
        {
            new UrlDisplayService().DisplayUrls(AllDatas.AllUrls.Values);
            break;
        }  
        else if(consoleOptions[temp] == 4)
        {
            var qrCodeGeneratorService = new QrCodeService();

            qrCodeGeneratorService.DisplayQrCode();
            break;
        }
        Console.Clear();
    }
}

bool GetContinueValue()
{
    return AnsiConsole.Prompt(
                new TextPrompt<bool>("Davom etishni xoxlaysizmi!")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));
}
