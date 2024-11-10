using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UrlShortenerApi;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем UrlShortener как Singleton
builder.Services.AddSingleton(new UrlShortener(Environment.GetEnvironmentVariable("HOSTNAME") ?? "https://url.ilmhub.uz"));

// Добавляем поддержку контроллеров
builder.Services.AddControllers();

var app = builder.Build();

// Включаем маршрутизацию для контроллеров
app.MapControllers();

app.Run();

