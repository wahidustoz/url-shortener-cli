using System;
using Net.Codecrete.QrCodeGenerator;
using Spectre.Console;


namespace UrlShortenerCli.Services;
public static class QrCodeShower
{
    public static void DisplayQrCodeInConsole(QrCode qrCode)
    {
        // Loop through the QR code matrix and output using AnsiConsole
        for (int y = 0; y < qrCode.Size; y += 2) // Use two rows to make it more square-like
        {
            for (int x = 0; x < qrCode.Size; x++)
            {
                // Use two rows per loop for a compact display
                bool top = qrCode.GetModule(x, y);            // Get the top pixel
                bool bottom = y + 1 < qrCode.Size && qrCode.GetModule(x, y + 1); // Get the bottom pixel (if within bounds)

                // Display solid block or spaces based on top and bottom pixels
                if (top && bottom)
                    AnsiConsole.Markup("[black on white]█[/]"); // Full block for both pixels
                else if (top)
                    AnsiConsole.Markup("[black on white]▀[/]"); // Upper half block
                else if (bottom)
                    AnsiConsole.Markup("[black on white]▄[/]"); // Lower half block
                else
                    AnsiConsole.Markup(" "); // Empty space for no pixels
            }
            Console.WriteLine(); // New line after each QR row
        }
    }
}

