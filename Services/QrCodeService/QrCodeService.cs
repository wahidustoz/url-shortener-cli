using Net.Codecrete.QrCodeGenerator;
using Spectre.Console;

public class QrCodeService : IQrCodeService
{
    public void DisplayQrCode()
    {
        while(true)
        {
            Console.Clear();

            var selectedUrl = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                    .Title("Qaysi URLga mos QR Code yaratmoqchisiz!")
                    .PageSize(10)
                    .MoreChoicesText("[grey]Pastda yana boshqa variantlar bo'lishi mumkin![/]")
                    .AddChoices(AllDatas.AllUrls.Keys.ToArray()));

            GenerateQrCode(selectedUrl);
            
            var continueValue = AnsiConsole.Prompt(
                new TextPrompt<bool>("Davom etishni xoxlaysizmi!")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));
            if(continueValue is false)
                break;
        }
    }

    public void GenerateQrCode(string shortUrl)
    {
        QrCode qrCode = QrCode.EncodeText(shortUrl, QrCode.Ecc.Medium);

        AnsiConsole.MarkupLine("[bold Green]QrCode Generated![/]");
        for (int y = 0; y < qrCode.Size; y += 2)
        {
            for (int x = 0; x < qrCode.Size; x++)
            {
                bool top = qrCode.GetModule(x, y);            
                bool bottom = y + 1 < qrCode.Size && qrCode.GetModule(x, y + 1); 

                if (top && bottom)
                    AnsiConsole.Markup("[black on white]█[/]"); 
                else if (top)
                    AnsiConsole.Markup("[black on white]▀[/]");
                else if (bottom)
                    AnsiConsole.Markup("[black on white]▄[/]");
                else
                    AnsiConsole.Markup(" ");
            }
            Console.WriteLine(); 
        }
        AnsiConsole.MarkupLine(shortUrl);
    }
}