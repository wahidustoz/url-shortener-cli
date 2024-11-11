using System.IO;
using System.Collections.Generic;

class URLStorage : IURLStorage  
{  
    private const string FilePath = "url_mapping.txt";  

    public Dictionary<string, string> LoadURLMapping()  
    {  
        var mapping = new Dictionary<string, string>();  

        if (File.Exists(FilePath))  
        {  
            foreach (var line in File.ReadAllLines(FilePath))  
            {  
                var parts = line.Split(";", StringSplitOptions.RemoveEmptyEntries);  
                if (parts.Length == 2)  
                {  
                    mapping[parts[0]] = parts[1];  
                }  
            }  
        }  

        return mapping;  
    }  

    public void SaveURLMapping(Dictionary<string, string> urlMapping)  
    {  
        using var writer = new StreamWriter(FilePath, false);  
        foreach (var (shortUrl, originalUrl) in urlMapping)  
        {  
            writer.WriteLine($"{shortUrl};{originalUrl}");  
        }  
    }  
}
