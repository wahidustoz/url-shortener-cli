using System;

namespace UrlShortenerCli.Interfaces;

public interface IQrCodeService
{
    void GenerateQrCode(string shortUrl);   
}
