using System;
using UrlShortenerCli.Data;
using UrlShortenerCli.Models;

using Spectre.Console;

namespace UrlShortenerCli.Services;

public static class DataBaseChecker
{
    public static bool UrlExistenceChecker(string url)
    {
        var tempUrl = new Url();
        if (UrlStorage.ShortAndLongUrls.ContainsKey(url))
        {
            tempUrl = new Url()
            {
                
                Id = UrlStorage.currentId,
                OriginalUrl = url,
                ShortCode = UrlStorage.ShortAndLongUrls[url],
            };

            UrlStorage.currentId++;

            UrlStorage.AllUrls[UrlStorage.currentId.ToString()] = tempUrl;
            return true;
        }
        return false;
    }
}
