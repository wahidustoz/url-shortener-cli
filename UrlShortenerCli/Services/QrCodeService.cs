using System;
using Net.Codecrete.QrCodeGenerator;
using UrlShortenerCli.Interfaces;

namespace UrlShortenerCli.Services;

public class QrCodeService : IQrCodeService
{
    public void GenerateQrCode(string shortUrl)
    {
        QrCode qrCode = QrCode.EncodeText(shortUrl, QrCode.Ecc.Medium);

        QrCodeShower.DisplayQrCodeInConsole(qrCode);
    }
}
