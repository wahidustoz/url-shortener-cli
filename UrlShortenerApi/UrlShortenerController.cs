using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi;  // Подключаем пространство имен, где находится ваш класс UrlShortener

namespace UrlShortenerAPI.Controllers
{
    // Указываем, что это контроллер API
    [ApiController]
    // Определяем маршрут для контроллера, он будет доступен по адресу /UrlShortener
    [Route("[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        // Создаем экземпляр UrlShortener, который будет использоваться для сокращения ссылок
        private static readonly UrlShortener urlShortener = new(Environment.GetEnvironmentVariable("HOSTNAME") ?? "https://ilmhub.uz");

        // POST: /UrlShortener/shorten
        // Этот метод будет обрабатывать POST-запросы для создания коротких ссылок
        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
            {
                return BadRequest("URL не может быть пустым.");
            }

            // Сокращаем URL и возвращаем клиенту сокращенную ссылку
            string shortUrl = urlShortener.ShortenUrl(longUrl);
            return Ok(new { shortUrl });
        }

        // GET: /UrlShortener/{shortCode}
        // Этот метод обрабатывает GET-запросы для переадресации по короткому URL
        [HttpGet("{shortCode}")]
        public IActionResult RedirectToOriginalUrl(string shortCode)
        {
            string originalUrl = urlShortener.GetOriginalUrl(shortCode);

            // Если короткий код не найден, возвращаем ошибку 404
            if (originalUrl == "Код не найден.")
            {
                return NotFound("Ссылка не найдена.");
            }

            // Перенаправляем пользователя на оригинальный URL
            return Redirect(originalUrl);
        }
    }
}
