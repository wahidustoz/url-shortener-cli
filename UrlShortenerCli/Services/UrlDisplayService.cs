using System;
using Microsoft.VisualBasic;
using Spectre.Console;
using UrlShortenerCli.Interfaces;
using UrlShortenerCli.Models;
using UrlShortenerCli.Services;

namespace UrlShortenerCli.Services;

public class UrlDisplayService : IUrlDisplayService
{
    private UrlShortener urlShortener = new();
    private const int PageSize = 10;

    public void DisplayUrls(IEnumerable<Url> urls)
    {
        

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Short Code");
        table.AddColumn("Original URL");
        table.AddColumn("Access Count");
        table.AddColumn("Date Created");

        foreach (var url in urls)
        {
            table.AddRow(
                url.Id.ToString(),
                url.ShortCode,
                url.OriginalUrl,
                url.AccessCount.ToString(),
                url.DateCreated.ToString(""));
        }

        AnsiConsole.Write(table);
    }

    public void DisplayPaginatedUrls(int pageNumber)
    {
        bool continuePagination = true;

        while (continuePagination)
        {
            Console.Clear();
            var urls = urlShortener.GetPaginatedUrls(pageNumber, PageSize);
            DisplayUrls(urls);

            // Display navigation options
            AnsiConsole.MarkupLine($"[grey]Page {pageNumber}[/]");
            var key = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an option:")
                    .AddChoices(new[] { "Previous Page", "Next Page", "Go to Main Menu" }));

            switch (key)
            {
                case "Previous Page":
                    if (pageNumber > 1) pageNumber--;
                    break;
                case "Next Page":
                    if (urlShortener.GetPaginatedUrls(pageNumber + 1, PageSize).Any()) pageNumber++;
                    break;
                case "Go to Main Menu":
                    continuePagination = false;
                    break;
            }
        }
    }
}
