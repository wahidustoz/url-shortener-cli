namespace urlsh;

    public class UrlShortener
    {
        // private readonly Dictionary<string, string> longUrlToShortUrl = new();
        // private readonly Dictionary<string, string> shortUrlToLongUrl = new();
        private readonly List<string> endings = new();
        private readonly List<char> letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToList();
        private Random rand = new Random();

        public string GenerateShortUrl(string url)
        {
            var urlEND = "";
            for (int i = 0; i < 7; i++)
            {
                urlEND += letters[rand.Next(0, letters.Count)];
            }

            var res = CheckUniqueness(urlEND, url);

            return res;

        }
        public string CheckUniqueness(string ending, string url)
        {
            foreach (var end in endings)
            {
                if (ending.Equals(end))
                {
                    GenerateShortUrl(url);
                }
            }

            endings.Add(ending);

            return "https://ilmhub.uz/" + ending;
        }
    }