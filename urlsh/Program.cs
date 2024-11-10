using System;
using System.Collections.Generic;
using url.shortener.cli.urlsh;

string host = Environment.GetEnvironmentVariable("HOSTNAME") ?? "https://url.ilmhub.uz";
if (args.Length > 0)
{
    host = args[0];
}

UrlShortener urlShortener = new UrlShortener(host);

Console.WriteLine("Введите длинный URL для сокращения:");
string longUrl = Console.ReadLine() ?? throw new ArgumentException("Invalid URL");
string shortUrl = urlShortener.ShortenUrl(longUrl);
Console.WriteLine($"Сокращенный URL: {shortUrl}");

Console.WriteLine("Введите короткий код для получения оригинального URL:");
string inputCode = Console.ReadLine() ?? throw new ArgumentException("Invalid URL");
string originalUrl = urlShortener.GetOriginalUrl(inputCode.Replace(host + "/", ""));
Console.WriteLine($"Оригинальный URL: {originalUrl}");

