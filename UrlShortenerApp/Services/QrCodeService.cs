using System;
using Net.Codecrete.QrCodeGenerator;
using Spectre.Console;

public class QrCodeService : IQrCodeService
{
    public void GenerateQrCode(string shortUrl)
    {
        var qr = QrCode.EncodeText(shortUrl, QrCode.Ecc.Medium);
        
        // QR kodni ASCII formatda konsolda ko‘rsatish
        for (int y = 0; y < qr.Size; y++)
        {
            for (int x = 0; x < qr.Size; x++)
            {
                AnsiConsole.Markup(qr.GetModule(x, y) ? "[black on white]  [/]" : "[white on black]  [/]");
            }
            AnsiConsole.WriteLine();
        }
    }
}
