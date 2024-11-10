namespace UrlShortenerApi;
public class UrlShortener
{
    private readonly Dictionary<string, string> urlDictionary = new();
    private readonly Dictionary<string, string> shortCodeDictionary = new();
    private int counter = 1;  // Счетчик для создания уникальных кодов
    private readonly string baseHost;  // Основной хост для сокращенных ссылок

    public UrlShortener(string host = "https://ilmhub.uz")
    {
        baseHost = host;
    }

    // Метод для сокращения URL
    public string ShortenUrl(string longUrl)
    {
        if (shortCodeDictionary.ContainsKey(longUrl))
        {
            // Если URL уже сокращен, возвращаем существующий полный URL
            return $"{baseHost}/{shortCodeDictionary[longUrl]}";
        }
        else
        {
            // Генерируем уникальный короткий код
            string shortCode = GenerateShortCode();

            // Сохраняем URL и короткий код в словари
            urlDictionary[shortCode] = longUrl;
            shortCodeDictionary[longUrl] = shortCode;

            return $"{baseHost}/{shortCode}";  // Возвращаем полный URL
        }
    }

    // Метод для получения оригинального URL по короткому коду
    public string GetOriginalUrl(string shortCode)
    {
        if (urlDictionary.TryGetValue(shortCode, out string longUrl))
        {
            return longUrl;
        }
        else
        {
            return "Код не найден.";
        }
    }

    // Метод для генерации короткого кода
    private string GenerateShortCode()
    {
        return "url" + counter++;
    }
}
